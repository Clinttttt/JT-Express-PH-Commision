import { useState } from "react";
import LoadingSpinner from "../../components/shared/LoadingSpinner/LoadingSpinner";
import ErrorMessage from "../../components/shared/ErrorMessage/ErrorMessage";
import { useServices } from "./hooks/useServices";
import { createService, updateService, deleteService } from "../../api/endpoints/servicesApi";
import { useAuth } from "../../context/AuthContext";
import type { Service } from "../../types";
import styles from "./ServicesPage.module.css";

export default function ServicesPage() {
  const { isLoggedIn } = useAuth();
  const { data, loading, error, refetch } = useServices();
  const [showForm, setShowForm] = useState(false);
  const [editingService, setEditingService] = useState<Service | null>(null);
  const [formData, setFormData] = useState({ name: "", description: "", priceLabel: "" });
  const [submitting, setSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitting(true);

    try {
      if (editingService) {
        await updateService(editingService.id, formData);
      } else {
        await createService(formData);
      }
      setShowForm(false);
      setEditingService(null);
      setFormData({ name: "", description: "", priceLabel: "" });
      refetch();
    } catch (err) {
      alert((err as Error).message);
    } finally {
      setSubmitting(false);
    }
  };

  const handleEdit = (service: Service) => {
    setEditingService(service);
    setFormData({
      name: service.name,
      description: service.description,
      priceLabel: service.priceLabel,
    });
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (!confirm("Are you sure you want to delete this service?")) return;

    try {
      await deleteService(id);
      refetch();
    } catch (err) {
      alert((err as Error).message);
    }
  };

  const handleCancel = () => {
    setShowForm(false);
    setEditingService(null);
    setFormData({ name: "", description: "", priceLabel: "" });
  };

  return (
    <div className="page-container">
      <div className={styles.pageHeader}>
        <div>
          <h1 className="page-title">Our Services</h1>
          <p className="page-subtitle">Reliable delivery solutions for every need</p>
        </div>
        {isLoggedIn && (
          <button className={styles.addBtn} onClick={() => setShowForm(true)}>
            + Add Service
          </button>
        )}
      </div>

      {showForm && (
        <div className={styles.formCard}>
          <h3 className={styles.formTitle}>{editingService ? "Edit Service" : "Add New Service"}</h3>
          <form onSubmit={handleSubmit} className={styles.form}>
            <div className={styles.formGroup}>
              <label>Name</label>
              <input
                type="text"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                required
              />
            </div>
            <div className={styles.formGroup}>
              <label>Description</label>
              <textarea
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                required
              />
            </div>
            <div className={styles.formGroup}>
              <label>Price Label</label>
              <input
                type="text"
                value={formData.priceLabel}
                onChange={(e) => setFormData({ ...formData, priceLabel: e.target.value })}
                required
              />
            </div>
            <div className={styles.formActions}>
              <button type="submit" className={styles.submitBtn} disabled={submitting}>
                {submitting ? "Saving..." : "Save"}
              </button>
              <button type="button" className={styles.cancelBtn} onClick={handleCancel}>
                Cancel
              </button>
            </div>
          </form>
        </div>
      )}

      {loading && <LoadingSpinner message="Loading services..." />}
      {error && <ErrorMessage message={error} />}
      {data && (
        <div className={styles.grid}>
          {data.map((service) => (
            <div key={service.id} className={styles.card}>
              {isLoggedIn && (
                <div className={styles.cardActions}>
                  <button className={styles.editBtn} onClick={() => handleEdit(service)}>
                    Edit
                  </button>
                  <button className={styles.deleteBtn} onClick={() => handleDelete(service.id)}>
                    Delete
                  </button>
                </div>
              )}
              <h3 className={styles.name}>{service.name}</h3>
              <p className={styles.description}>{service.description}</p>
              <span className={styles.price}>{service.priceLabel}</span>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
