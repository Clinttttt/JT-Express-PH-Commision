import { useState, type FormEvent } from "react";
import { Link } from "react-router-dom";
import { resetPassword } from "../../api/endpoints/authApi";
import ErrorMessage from "../../components/shared/ErrorMessage/ErrorMessage";
import styles from "./AuthPage.module.css";

export default function ForgotPasswordPage() {
  const [username, setUsername] = useState("");
  const [restorationKey, setRestorationKey] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [newKey, setNewKey] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!username || !restorationKey || !newPassword || !confirmPassword) {
      setError("All fields are required.");
      return;
    }

    if (newPassword !== confirmPassword) {
      setError("Passwords do not match.");
      return;
    }

    if (newPassword.length < 6) {
      setError("Password must be at least 6 characters.");
      return;
    }

    setLoading(true);
    try {
      const result = await resetPassword(username, restorationKey, newPassword);
      setNewKey(result.restorationKey);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  if (newKey) {
    return (
      <div className={styles.container}>
        <div className={styles.card}>
          <h1 className={styles.title}>Password Reset Successful</h1>
          <p className={styles.subtitle}>
            Your password has been reset. Here's your new restoration key:
          </p>
          
          <div className={styles.keyBox}>
            <code className={styles.keyCode}>{newKey}</code>
          </div>

          <div className={styles.warningBox}>
            <strong>⚠️ Important:</strong> Save this new key! Your old restoration key is no longer valid.
          </div>

          <Link to="/login" className={styles.btn}>
            Go to Login
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className={styles.container}>
      <div className={styles.card}>
        <h1 className={styles.title}>Reset Password</h1>
        <p className={styles.subtitle}>Use your restoration key to reset your password</p>

        {error && <ErrorMessage message={error} />}

        <form onSubmit={handleSubmit} className={styles.form}>
          <div className={styles.formGroup}>
            <label htmlFor="username">Username</label>
            <input
              id="username"
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Enter your username"
              required
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="restorationKey">Restoration Key</label>
            <input
              id="restorationKey"
              type="text"
              value={restorationKey}
              onChange={(e) => setRestorationKey(e.target.value)}
              placeholder="Enter your restoration key"
              required
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="newPassword">New Password</label>
            <input
              id="newPassword"
              type="password"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              placeholder="Enter new password (min 6 characters)"
              required
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="confirmPassword">Confirm New Password</label>
            <input
              id="confirmPassword"
              type="password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              placeholder="Confirm new password"
              required
            />
          </div>

          <button type="submit" className={styles.btn} disabled={loading}>
            {loading ? "Resetting..." : "Reset Password"}
          </button>
        </form>

        <Link to="/login" className={styles.backLink}>
          Back to Login
        </Link>
      </div>
    </div>
  );
}
