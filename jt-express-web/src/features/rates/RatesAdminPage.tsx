import { useEffect, useState, FormEvent } from "react";
import { useRatesAdmin } from "./hooks/useRatesAdmin";
import type { Rate } from "../../types";
import styles from "./RatesAdminPage.module.css";

export default function RatesAdminPage() {
  const { data, loading, error, loadRates, create, update, remove } = useRatesAdmin();
  const [showForm, setShowForm] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formError, setFormError] = useState<string | null>(null);
  const [formData, setFormData] = useState<Omit<Rate, "id">>({
    zone: "",
    firstKg: 0,
    succeedingKg: 0,
  });

  useEffect(() => {
    loadRates();
  }, [loadRates]);

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
      setFormData({ zone: "", firstKg: 0, succeedingKg: 0 });
    } catch (err) {
      setFormError((err as Error).message);
    }
  };

  const handleEdit = (rate: Rate) => {
    setFormData({
      zone: rate.zone,
      firstKg: rate.firstKg,
      succeedingKg: rate.succeedingKg,
    });
    setEditingId(rate.id);
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
    setFormData({ zone: "", firstKg: 0, succeedingKg: 0 });
    setFormError(null);
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1 className="page-title">Manage Rates</h1>
        <button className={styles.addBtn} onClick={() => setShowForm(true)}>
          + Add Rate
        </button>
      </div>

      {error && <div className={styles.error}>{error}</div>}

      {showForm && (
        <div className={styles.formContainer}>
          <h2>{editingId ? "Edit Rate" : "Add New Rate"}</h2>
          <form onSubmit={handleSubmit} className={styles.form}>
            <input
              type="text"
              placeholder="Zone Name"
              value={formData.zone}
              onChange={(e) => setFormData({ ...formData, zone: e.target.value })}
              required
              style={{ marginTop: '22px' }}
            />
            <div className={styles.formGroup}>
              <label>First Kg Rate</label>
              <input
                type="number"
                step="0.01"
                value={formData.firstKg}
                onChange={(e) => setFormData({ ...formData, firstKg: parseFloat(e.target.value) })}
                required
              />
            </div>
            <div className={styles.formGroup}>
              <label>Succeeding Kg Rate</label>
              <input
                type="number"
                step="0.01"
                value={formData.succeedingKg}
                onChange={(e) => setFormData({ ...formData, succeedingKg: parseFloat(e.target.value) })}
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
                <th>Zone</th>
                <th>First Kg</th>
                <th>Succeeding Kg</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {data?.map((rate) => (
                <tr key={rate.id}>
                  <td>{rate.zone}</td>
                  <td>₱{rate.firstKg}</td>
                  <td>₱{rate.succeedingKg}</td>
                  <td className={styles.actions}>
                    <button className={styles.editBtn} onClick={() => handleEdit(rate)}>
                      Edit
                    </button>
                    <button className={styles.deleteBtn} onClick={() => handleDelete(rate.id)}>
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
