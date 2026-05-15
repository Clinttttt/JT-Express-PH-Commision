import styles from "./LoadingSpinner.module.css";

interface Props {
  message?: string;
}

export default function LoadingSpinner({ message = "Loading..." }: Props) {
  return (
    <div className={styles.wrapper} role="status" aria-live="polite">
      <div className={styles.spinner} aria-hidden="true" />
      <p className={styles.message}>{message}</p>
    </div>
  );
}
