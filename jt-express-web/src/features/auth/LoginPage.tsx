import { useState, type FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import { login as loginApi } from "../../api/endpoints/authApi";
import { useAuth } from "../../context/AuthContext";
import ErrorMessage from "../../components/shared/ErrorMessage/ErrorMessage";
import styles from "./AuthPage.module.css";

export default function LoginPage() {
  const navigate = useNavigate();
  const { login } = useAuth();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!username || !password) {
      setError("Username and password are required.");
      return;
    }

    setLoading(true);
    try {
      const result = await loginApi(username, password);
      login(result.token, result.username);
      navigate("/services");
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.card}>
        <h1 className={styles.title}>Admin Login</h1>
        <p className={styles.subtitle}>Sign in to manage services</p>

        {error && <ErrorMessage message={error} />}

        <form onSubmit={handleSubmit} className={styles.form}>
          <div className={styles.formGroup}>
            <label htmlFor="username">Username</label>
            <input
              id="username"
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Enter username"
              required
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter password"
              required
            />
          </div>

          <button type="submit" className={styles.btn} disabled={loading}>
            {loading ? "Signing in..." : "Sign In"}
          </button>
        </form>
      </div>
    </div>
  );
}
