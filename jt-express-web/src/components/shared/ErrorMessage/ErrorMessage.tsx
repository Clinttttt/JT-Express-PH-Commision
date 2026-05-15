import styles from "./ErrorMessage.module.css";

interface Props {
  message: string;
  onRetry?: () => void;
}

export default function ErrorMessage({ message, onRetry }: Props) {
  return (
    <div className={styles.wrapper} role="alert" aria-live="assertive">
      <p className={styles.message}>{message}</p>
      {onRetry && (
        <button className={styles.retryBtn} onClick={onRetry} aria-label="Retry loading">
          Try again
        </button>
      )}
    </div>
  );
}
