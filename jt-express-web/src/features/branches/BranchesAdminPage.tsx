import { useEffect, useState, FormEvent } from "react";
import { useBranchesAdmin } from "./hooks/useBranchesAdmin";
import type { Branch } from "../../types";
import styles from "./BranchesAdminPage.module.css";

const REGIONS = ["Metro Manila", "Luzon", "Visayas", "Mindanao"];

export default function BranchesAdminPage() {
  const { data, loading, error, loadBranches, create, update, remove } = useBranchesAdmin();
  const [showForm, setShowForm] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formError, setFormError] = useState<string | null>(null);
  const [formData, setFormData] = useState<Omit<Branch, "id">>({
    name: "",
    address: "",
    region: "",
    phone: "",
    hours: "",
    latitude: 0,
    longitude: 0,
  });

  useEffect(() => {
    loadBranches();
  }, [loadBranches]);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setFormError(null);

    try {
      if (editingId) {
        await update(editingId, formData);
      } else {
        await create(formData);
      }
      setShowForm(false);
      setEditingId(null);
      setFormData({
        name: "",
        address: "",
        region: "",
        phone: "",
        hours: "",
        latitude: 0,
        longitude: 0,
      });
    } catch (err) {
      setFormError((err as Error).message);
    }
  };

  const handleEdit = (branch: Branch) => {
    setFormData({
      name: branch.name,
      address: branch.address,
      region: branch.region,
      phone: branch.phone,
      hours: branch.hours,
      latitude: branch.latitude,
      longitude: branch.longitude,
    });
    setEditingId(branch.id);
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (confirm("Are you sure?")) {
      try {
        await remove(id);
      } catch (err) {
        setFormError((err as Error).message);
      }
    }
  };

  const handleCancel = () => {
    setShowForm(false);
    setEditingId(null);
    setFormData({
      name: "",
      address: "",
      region: "",
      phone: "",
      hours: "",
      latitude: 0,
      longitude: 0,
    });
    setFormError(null);
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1 className="page-title">Manage Branches</h1>
        <button className={styles.addBtn} onClick={() => setShowForm(true)}>
          + Add Branch
        </button>
      </div>

      {error && <div className={styles.error}>{error}</div>}

      {showForm && (
        <div className={styles.formContainer}>
          <h2>{editingId ? "Edit Branch" : "Add New Branch"}</h2>
          <form onSubmit={handleSubmit} className={styles.form}>
            <input
              type="text"
              placeholder="Branch Name"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              required
            />
            <input
              type="text"
              placeholder="Address"
              value={formData.address}
              onChange={(e) => setFormData({ ...formData, address: e.target.value })}
              required
            />
            <select
              value={formData.region}
              onChange={(e) => setFormData({ ...formData, region: e.target.value })}
              required
            >
              <option value="">Select Region</option>
              {REGIONS.map((r) => (
                <option key={r} value={r}>
                  {r}
                </option>
              ))}
            </select>
            <input
              type="tel"
              placeholder="Phone"
              value={formData.phone}
              onChange={(e) => setFormData({ ...formData, phone: e.target.value })}
              required
            />
            <input
              type="text"
              placeholder="Hours (e.g., 9AM-6PM)"
              value={formData.hours}
              onChange={(e) => setFormData({ ...formData, hours: e.target.value })}
              required
              style={{ marginTop: '22px' }}
            />
            <div className={styles.formGroup}>
              <label>Latitude</label>
              <input
                type="number"
                step="0.0001"
                value={formData.latitude}
                onChange={(e) => setFormData({ ...formData, latitude: parseFloat(e.target.value) })}
                required
              />
            </div>
            <div className={styles.formGroup}>
              <label>Longitude</label>
              <input
                type="number"
                step="0.0001"
                value={formData.longitude}
                onChange={(e) => setFormData({ ...formData, longitude: parseFloat(e.target.value) })}
                required
              />
            </div>
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
        <div className={styles.table}>
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Address</th>
                <th>Region</th>
                <th>Phone</th>
                <th>Hours</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {data?.map((branch) => (
                <tr key={branch.id}>
                  <td>{branch.name}</td>
                  <td>{branch.address}</td>
                  <td>{branch.region}</td>
                  <td>{branch.phone}</td>
                  <td>{branch.hours}</td>
                  <td className={styles.actions}>
                    <button className={styles.editBtn} onClick={() => handleEdit(branch)}>
                      Edit
                    </button>
                    <button className={styles.deleteBtn} onClick={() => handleDelete(branch.id)}>
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
