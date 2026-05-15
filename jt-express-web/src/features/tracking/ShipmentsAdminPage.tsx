import { useState, FormEvent } from "react";
import { useShipmentsAdmin } from "./hooks/useShipmentsAdmin";
import type { Shipment } from "../../types";
import styles from "./ShipmentsAdminPage.module.css";

const STATUSES = ["Parcel Picked Up", "In Transit", "Arrived at Hub", "Out for Delivery", "Delivered"];

export default function ShipmentsAdminPage() {
  const { data, loading, error, create, update, addEvent, remove } = useShipmentsAdmin();
  const [showForm, setShowForm] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formError, setFormError] = useState<string | null>(null);
  const [expandedId, setExpandedId] = useState<number | null>(null);
  const [eventForm, setEventForm] = useState<{ shipmentId: number; date: string; status: string; location: string } | null>(null);

  const [formData, setFormData] = useState({
    trackingNumber: "",
    sender: "",
    recipient: "",
    estimatedDelivery: "",
    status: "Parcel Picked Up",
    currentLocation: "Processing",
  });

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setFormError(null);

    try {
      if (editingId) {
        await update(editingId, { status: formData.status, currentLocation: formData.currentLocation });
      } else {
        await create({
          trackingNumber: formData.trackingNumber,
          sender: formData.sender,
          recipient: formData.recipient,
          estimatedDelivery: formData.estimatedDelivery,
          status: formData.status,
          currentLocation: formData.currentLocation,
        } as any);
      }
      setShowForm(false);
      setEditingId(null);
      resetForm();
    } catch (err) {
      setFormError((err as Error).message);
    }
  };

  const handleEdit = (shipment: Shipment) => {
    setFormData({
      trackingNumber: shipment.trackingNumber,
      sender: shipment.sender,
      recipient: shipment.recipient,
      estimatedDelivery: shipment.estimatedDelivery,
      status: shipment.status,
      currentLocation: shipment.currentLocation,
    });
    setEditingId(shipment.id);
    setShowForm(true);
  };

  const handleAddEvent = async (e: FormEvent) => {
    e.preventDefault();
    if (!eventForm) return;

    try {
      await addEvent(eventForm.shipmentId, {
        date: eventForm.date,
        status: eventForm.status,
        location: eventForm.location,
      });
      setEventForm(null);
    } catch (err) {
      setFormError((err as Error).message);
    }
  };

  const handleDelete = async (id: number) => {
    if (confirm("Delete this shipment?")) {
      try {
        await remove(id);
      } catch (err) {
        setFormError((err as Error).message);
      }
    }
  };

  const resetForm = () => {
    setFormData({
      trackingNumber: "",
      sender: "",
      recipient: "",
      estimatedDelivery: "",
      status: "Parcel Picked Up",
      currentLocation: "Processing",
    });
  };

  const handleCancel = () => {
    setShowForm(false);
    setEditingId(null);
    resetForm();
    setFormError(null);
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1 className="page-title">Manage Shipments</h1>
        <button className={styles.addBtn} onClick={() => setShowForm(true)}>
          + New Shipment
        </button>
      </div>

      {error && <div className={styles.error}>{error}</div>}

      {showForm && (
        <div className={styles.formContainer}>
          <h2>{editingId ? "Edit Shipment" : "Create New Shipment"}</h2>
          <form onSubmit={handleSubmit} className={styles.form}>
            {!editingId && (
              <>
                <input
                  type="text"
                  placeholder="Tracking Number"
                  value={formData.trackingNumber}
                  onChange={(e) => setFormData({ ...formData, trackingNumber: e.target.value })}
                  required
                />
                <input
                  type="text"
                  placeholder="Sender"
                  value={formData.sender}
                  onChange={(e) => setFormData({ ...formData, sender: e.target.value })}
                  required
                />
                <input
                  type="text"
                  placeholder="Recipient"
                  value={formData.recipient}
                  onChange={(e) => setFormData({ ...formData, recipient: e.target.value })}
                  required
                />
                <input
                  type="text"
                  placeholder="Estimated Delivery"
                  value={formData.estimatedDelivery}
                  onChange={(e) => setFormData({ ...formData, estimatedDelivery: e.target.value })}
                  required
                />
              </>
            )}
            <select
              value={formData.status}
              onChange={(e) => setFormData({ ...formData, status: e.target.value })}
              required
            >
              {STATUSES.map((s) => (
                <option key={s} value={s}>
                  {s}
                </option>
              ))}
            </select>
            <input
              type="text"
              placeholder="Current Location"
              value={formData.currentLocation}
              onChange={(e) => setFormData({ ...formData, currentLocation: e.target.value })}
              required
            />
            {formError && <div className={styles.error}>{formError}</div>}
            <div className={styles.formActions}>
              <button type="submit" className={styles.submitBtn}>
                {editingId ? "Update" : "Create"}
              </button>
              <button type="button" className={styles.cancelBtn} onClick={handleCancel}>
                Cancel
              </button>
            </div>
          </form>
        </div>
      )}

      {loading ? (
        <p>Loading...</p>
      ) : (
        <div className={styles.list}>
          {data?.map((shipment) => (
            <div key={shipment.id} className={styles.card}>
              <div className={styles.cardHeader}>
                <div>
                  <h3>{shipment.trackingNumber}</h3>
                  <p className={styles.status}>{shipment.status}</p>
                </div>
                <div className={styles.cardActions}>
                  <button className={styles.expandBtn} onClick={() => setExpandedId(expandedId === shipment.id ? null : shipment.id)}>
                    {expandedId === shipment.id ? "▼" : "▶"}
                  </button>
                  <button className={styles.editBtn} onClick={() => handleEdit(shipment)}>
                    Edit
                  </button>
                  <button className={styles.deleteBtn} onClick={() => handleDelete(shipment.id)}>
                    Delete
                  </button>
                </div>
              </div>

              {expandedId === shipment.id && (
                <div className={styles.cardContent}>
                  <div className={styles.info}>
                    <p><strong>Sender:</strong> {shipment.sender}</p>
                    <p><strong>Recipient:</strong> {shipment.recipient}</p>
                    <p><strong>Est. Delivery:</strong> {shipment.estimatedDelivery}</p>
                    <p><strong>Location:</strong> {shipment.currentLocation}</p>
                  </div>

                  <div className={styles.timeline}>
                    <h4>Timeline Events</h4>
                    {shipment.timeline.map((event) => (
                      <div key={event.id} className={styles.timelineItem}>
                        <p className={styles.timelineDate}>{event.date}</p>
                        <p className={styles.timelineStatus}>{event.status}</p>
                        <p className={styles.timelineLocation}>{event.location}</p>
                      </div>
                    ))}
                  </div>

                  <button
                    className={styles.addEventBtn}
                    onClick={() => setEventForm({ shipmentId: shipment.id, date: "", status: "", location: "" })}
                  >
                    + Add Event
                  </button>

                  {eventForm?.shipmentId === shipment.id && (
                    <form onSubmit={handleAddEvent} className={styles.eventForm}>
                      <input
                        type="text"
                        placeholder="Date (e.g., May 15 10:00 AM)"
                        value={eventForm.date}
                        onChange={(e) => setEventForm({ ...eventForm, date: e.target.value })}
                        required
                      />
                      <select
                        value={eventForm.status}
                        onChange={(e) => setEventForm({ ...eventForm, status: e.target.value })}
                        required
                      >
                        <option value="">Select Status</option>
                        {STATUSES.map((s) => (
                          <option key={s} value={s}>
                            {s}
                          </option>
                        ))}
                      </select>
                      <input
                        type="text"
                        placeholder="Location"
                        value={eventForm.location}
                        onChange={(e) => setEventForm({ ...eventForm, location: e.target.value })}
                        required
                      />
                      <div className={styles.eventActions}>
                        <button type="submit" className={styles.submitBtn}>
                          Add
                        </button>
                        <button type="button" className={styles.cancelBtn} onClick={() => setEventForm(null)}>
                          Cancel
                        </button>
                      </div>
                    </form>
                  )}
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
