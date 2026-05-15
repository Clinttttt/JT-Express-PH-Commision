import { useState, type FormEvent } from "react";
import LoadingSpinner from "../../components/shared/LoadingSpinner/LoadingSpinner";
import ErrorMessage from "../../components/shared/ErrorMessage/ErrorMessage";
import { useTracking } from "./hooks/useTracking";
import styles from "./TrackingPage.module.css";

const STATUS_COLORS: Record<string, string> = {
  "Delivered": "success",
  "Out for Delivery": "info",
  "In Transit": "warning",
  "Arrived at Hub": "purple",
  "Parcel Picked Up": "neutral",
};

export default function TrackingPage() {
  const [trackingNumber, setTrackingNumber] = useState("");
  const [validationError, setValidationError] = useState("");
  const { data, loading, error, track } = useTracking();

  const validateTrackingNumber = (value: string): boolean => {
    if (!value.trim()) {
      setValidationError("Tracking number is required");
      return false;
    }
    if (value.length < 10) {
      setValidationError("Tracking number must be at least 10 characters");
      return false;
    }
    setValidationError("");
    return true;
  };

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    const trimmed = trackingNumber.trim();
    if (validateTrackingNumber(trimmed)) {
      track(trimmed);
    }
  };

  const handleRetry = () => {
    setTrackingNumber("");
    setValidationError("");
  };

  return (
    <div className="page-container">
      <div className="page-header">
        <h1 className="page-title">Track Your Parcel</h1>
        <p className="page-subtitle">Enter your tracking number to see real-time updates</p>
      </div>

      <form onSubmit={handleSubmit} className={styles.form}>
        <div className={styles.inputWrapper}>
          <input
            type="text"
            value={trackingNumber}
            onChange={(e) => {
              setTrackingNumber(e.target.value);
              if (validationError) validateTrackingNumber(e.target.value);
            }}
            placeholder="Enter tracking number e.g. JT123456789PH"
            className={`${styles.input} ${validationError ? styles.inputError : ""}`}
            required
          />
          {validationError && <span className={styles.errorText}>{validationError}</span>}
        </div>
        <button type="submit" className={styles.btn} disabled={loading}>
          Track
        </button>
      </form>

      {loading && <LoadingSpinner message="Looking up your parcel..." />}
      {error && <ErrorMessage message={error} onRetry={handleRetry} />}
      
      {data && (
        <div className={styles.result}>
          <div className={styles.header}>
            <h2 className={styles.trackingNum}>{data.trackingNumber}</h2>
            <span className={`${styles.badge} ${styles[`badge${STATUS_COLORS[data.status] || "neutral"}`]}`}>
              {data.status}
            </span>
          </div>

          <div className={styles.infoGrid}>
            <div className={styles.infoItem}>
              <span className={styles.infoLabel}>Sender</span>
              <span className={styles.infoValue}>{data.sender}</span>
            </div>
            <div className={styles.infoItem}>
              <span className={styles.infoLabel}>Recipient</span>
              <span className={styles.infoValue}>{data.recipient}</span>
            </div>
            <div className={styles.infoItem}>
              <span className={styles.infoLabel}>Estimated Delivery</span>
              <span className={styles.infoValue}>{data.estimatedDelivery}</span>
            </div>
            <div className={styles.infoItem}>
              <span className={styles.infoLabel}>Current Location</span>
              <span className={styles.infoValue}>{data.currentLocation}</span>
            </div>
          </div>

          <div className={styles.timeline}>
            <h3 className={styles.timelineTitle}>Tracking Timeline</h3>
            <div className={styles.timelineList}>
              {data.timeline.map((event, idx) => (
                <div key={idx} className={styles.timelineItem}>
                  <div className={`${styles.timelineDot} ${idx === 0 ? styles.timelineDotActive : ""}`} />
                  <div className={styles.timelineContent}>
                    <p className={styles.timelineDate}>{event.date}</p>
                    <p className={styles.timelineStatus}>{event.status}</p>
                    <p className={styles.timelineLocation}>{event.location}</p>
                  </div>
                </div>
              ))}
            </div>
          </div>

          <button className={styles.trackAnotherBtn} onClick={handleRetry}>
            Track Another Parcel
          </button>
        </div>
      )}
    </div>
  );
}
