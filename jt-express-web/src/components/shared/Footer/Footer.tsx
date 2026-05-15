import { Link } from "react-router-dom";
import styles from "./Footer.module.css";

export default function Footer() {
  return (
    <footer className={styles.footer}>
      <div className={styles.inner}>
        <div className={styles.content}>
          <div className={styles.section}>
            <h3 className={styles.title}>J&T Express</h3>
            <p className={styles.desc}>Fast, reliable delivery across the Philippines</p>
            <div className={styles.social}>
              <a href="https://www.facebook.com/JTExpressPH" target="_blank" rel="noopener noreferrer" className={styles.socialLink}>Facebook</a>
              <a href="https://twitter.com/JTExpressPH" target="_blank" rel="noopener noreferrer" className={styles.socialLink}>Twitter</a>
              <a href="https://www.instagram.com/jtexpress.ph" target="_blank" rel="noopener noreferrer" className={styles.socialLink}>Instagram</a>
            </div>
          </div>

          <div className={styles.section}>
            <h4 className={styles.sectionTitle}>Services</h4>
            <Link to="/services" className={styles.link}>Express Delivery</Link>
            <Link to="/services" className={styles.link}>Standard Delivery</Link>
            <Link to="/services" className={styles.link}>Cash on Delivery</Link>
          </div>

          <div className={styles.section}>
            <h4 className={styles.sectionTitle}>Support</h4>
            <Link to="/tracking" className={styles.link}>Track Parcel</Link>
            <Link to="/branches" className={styles.link}>Find Branch</Link>
            <a href="https://www.jtexpress.ph/contact" target="_blank" rel="noopener noreferrer" className={styles.link}>Contact Us</a>
          </div>

          <div className={styles.section}>
            <h4 className={styles.sectionTitle}>Company</h4>
            <a href="https://www.jtexpress.ph/about" target="_blank" rel="noopener noreferrer" className={styles.link}>About Us</a>
            <a href="https://www.jtexpress.ph/careers" target="_blank" rel="noopener noreferrer" className={styles.link}>Careers</a>
            <a href="https://www.jtexpress.ph/privacy" target="_blank" rel="noopener noreferrer" className={styles.link}>Privacy Policy</a>
          </div>
        </div>

        <div className={styles.bottom}>
          <p className={styles.text}>
            © 2026 J&T Express Philippines. All rights reserved.
          </p>
        </div>
      </div>
    </footer>
  );
}
