import { useState, type FormEvent } from "react";
import LoadingSpinner from "../../components/shared/LoadingSpinner/LoadingSpinner";
import ErrorMessage from "../../components/shared/ErrorMessage/ErrorMessage";
import { useRates } from "./hooks/useRates";
import { calculateRate } from "../../api/endpoints/ratesApi";
import type { RateCalculationResult } from "../../types";
import styles from "./RatesPage.module.css";

export default function RatesPage() {
  const { data, loading, error } = useRates();
  const [zone, setZone] = useState("");
  const [weight, setWeight] = useState("");
  const [calculating, setCalculating] = useState(false);
  const [calcError, setCalcError] = useState<string | null>(null);
  const [validationError, setValidationError] = useState<string>("");
  const [result, setResult] = useState<RateCalculationResult | null>(null);

  const validateForm = (): boolean => {
    if (!zone) {
      setValidationError("Please select a zone");
      return false;
    }
    if (!weight) {
      setValidationError("Please enter weight");
      return false;
    }
    const weightNum = parseFloat(weight);
    if (isNaN(weightNum) || weightNum <= 0) {
      setValidationError("Weight must be a positive number");
      return false;
    }
    if (weightNum > 1000) {
      setValidationError("Weight cannot exceed 1000 kg");
      return false;
    }
    setValidationError("");
    return true;
  };

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    if (!validateForm()) return;

    setCalculating(true);
    setCalcError(null);
    setResult(null);

    try {
      const res = await calculateRate(zone, parseFloat(weight));
      setResult(res);
    } catch (err) {
      setCalcError((err as Error).message);
    } finally {
      setCalculating(false);
    }
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1 className="page-title">Shipping Rates</h1>
        <p className="page-subtitle">Calculate your delivery cost</p>
      </div>

      {loading && <LoadingSpinner message="Loading rates..." />}
      {error && <ErrorMessage message={error} />}
      
      {data && (
        <>
          <div className={styles.tableWrapper}>
            <h2 className={styles.tableTitle}>Shipping Rates by Zone</h2>
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Zone</th>
                  <th>Coverage</th>
                  <th>First Kg</th>
                  <th>Per Additional Kg</th>
                </tr>
              </thead>
              <tbody>
                {data.map((rate, idx) => {
                  const zoneInfo: Record<string, string> = {
                    "Metro Manila": "NCR and nearby provinces",
                    "Luzon": "Northern and Central Luzon",
                    "Visayas": "Central Philippines",
                    "Mindanao": "Southern Philippines",
                    "Island Provinces": "Remote island areas"
                  };
                  return (
                    <tr key={rate.zone} className={idx % 2 === 0 ? styles.rowEven : ""}>
                      <td className={styles.zone}>{rate.zone}</td>
                      <td className={styles.coverage}>{zoneInfo[rate.zone] || "Coverage area"}</td>
                      <td className={styles.price}>₱{rate.firstKg}</td>
                      <td className={styles.price}>₱{rate.succeedingKg}</td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>

          <div className={styles.calculator}>
            <h2 className={styles.calcTitle}>Calculate Your Rate</h2>
            <form onSubmit={handleSubmit} className={styles.form}>
              <div className={styles.formGroup}>
                <label htmlFor="zone">Zone</label>
                <select
                  id="zone"
                  value={zone}
                  onChange={(e) => {
                    setZone(e.target.value);
                    if (validationError) validateForm();
                  }}
                  className={styles.select}
                  required
                >
                  <option value="">Select zone</option>
                  {data.map((rate) => (
                    <option key={rate.zone} value={rate.zone}>
                      {rate.zone}
                    </option>
                  ))}
                </select>
              </div>

              <div className={styles.formGroup}>
                <label htmlFor="weight">Weight (kg)</label>
                <input
                  id="weight"
                  type="number"
                  min="0.1"
                  step="0.1"
                  max="1000"
                  value={weight}
                  onChange={(e) => {
                    setWeight(e.target.value);
                    if (validationError) validateForm();
                  }}
                  className={styles.input}
                  placeholder="e.g. 2.5"
                  required
                />
              </div>

              {validationError && <span className={styles.validationError}>{validationError}</span>}

              <button type="submit" className={styles.btn} disabled={calculating}>
                {calculating ? "Calculating..." : "Calculate"}
              </button>
            </form>

            {calcError && <ErrorMessage message={calcError} />}
            {result && (
              <div className={styles.result}>
                <p className={styles.resultLabel}>Estimated Rate</p>
                <p className={styles.resultValue}>{result.formattedRate}</p>
                <p className={styles.resultDetails}>
                  {result.zone} • {result.weight} kg
                </p>
              </div>
            )}
          </div>
        </>
      )}
    </div>
  );
}
