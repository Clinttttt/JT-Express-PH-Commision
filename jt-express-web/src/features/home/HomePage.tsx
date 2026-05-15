import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { fetchServices } from "../../api/endpoints/servicesApi";
import { fetchBranches } from "../../api/endpoints/branchesApi";
import styles from "./HomePage.module.css";

export default function HomePage() {
  const [stats, setStats] = useState({ services: 0, branches: 0 });

  useEffect(() => {
    Promise.all([fetchServices(), fetchBranches()])
      .then(([services, branches]) => {
        setStats({ services: services.length, branches: branches.length });
      })
      .catch(() => {
        setStats({ services: 0, branches: 0 });
      });
  }, []);

  return (
    <div>
      <section className={styles.hero}>
        <div className={styles.heroInner}>
          <h1 className={styles.heroTitle}>
            <span className={styles.heroAccent}>Fast.</span> Reliable. <span className={styles.heroAccent}>Nationwide.</span>
          </h1>
          <p className={styles.heroText}>
            J&T Express delivers to every corner of the Philippines with same-day pickup and real-time tracking.
          </p>
          <div className={styles.heroCtas}>
            <Link to="/tracking" className={styles.heroCta}>
              Track Your Parcel
            </Link>
            <Link to="/services" className={styles.heroCtaSecondary}>
              View Services
            </Link>
          </div>
        </div>
      </section>

      <section className={styles.stats}>
        <div className={styles.statsInner}>
          <div className={styles.statItem}>
            <div className={styles.statIcon}>SVC</div>
            <div className={styles.statNumber}>{stats.services}</div>
            <div className={styles.statLabel}>Delivery Services</div>
          </div>
          <div className={styles.statItem}>
            <div className={styles.statIcon}>BRN</div>
            <div className={styles.statNumber}>{stats.branches}</div>
            <div className={styles.statLabel}>Branches Nationwide</div>
          </div>
          <div className={styles.statItem}>
            <div className={styles.statIcon}>24/7</div>
            <div className={styles.statNumber}>Real-Time</div>
            <div className={styles.statLabel}>Parcel Tracking</div>
          </div>
        </div>
      </section>

      <section className={styles.quickAccess}>
        <div className={styles.quickAccessInner}>
          <h2 className={styles.sectionTitle}>Quick Access</h2>
          <div className={styles.cardGrid}>
            <Link to="/tracking" className={styles.card}>
              <div className={styles.cardIcon}>TRK</div>
              <h3 className={styles.cardTitle}>Track Parcel</h3>
              <p className={styles.cardDesc}>Real-time tracking updates</p>
            </Link>
            <Link to="/services" className={styles.card}>
              <div className={styles.cardIcon}>SVC</div>
              <h3 className={styles.cardTitle}>Our Services</h3>
              <p className={styles.cardDesc}>Express, Standard & more</p>
            </Link>
            <Link to="/rates" className={styles.card}>
              <div className={styles.cardIcon}>RTS</div>
              <h3 className={styles.cardTitle}>Shipping Rates</h3>
              <p className={styles.cardDesc}>Transparent pricing</p>
            </Link>
            <Link to="/branches" className={styles.card}>
              <div className={styles.cardIcon}>LOC</div>
              <h3 className={styles.cardTitle}>Find a Branch</h3>
              <p className={styles.cardDesc}>{stats.branches}+ locations</p>
            </Link>
          </div>
        </div>
      </section>

      <section className={styles.why}>
        <div className={styles.whyInner}>
          <h2 className={styles.sectionTitle}>Why Choose J&T Express?</h2>
          <div className={styles.whyGrid}>
            <div className={styles.whyItem}>
              <div className={styles.whyIcon}>PHL</div>
              <h3 className={styles.whyTitle}>Nationwide Coverage</h3>
              <p className={styles.whyText}>
                We deliver to every province across the Philippines with reliable service.
              </p>
            </div>
            <div className={styles.whyItem}>
              <div className={styles.whyIcon}>PKP</div>
              <h3 className={styles.whyTitle}>Same-Day Pickup</h3>
              <p className={styles.whyText}>
                Schedule a pickup and we'll collect your parcel the same day.
              </p>
            </div>
            <div className={styles.whyItem}>
              <div className={styles.whyIcon}>GPS</div>
              <h3 className={styles.whyTitle}>Real-Time Tracking</h3>
              <p className={styles.whyText}>
                Track your parcel every step of the way with live updates.
              </p>
            </div>
            <div className={styles.whyItem}>
              <div className={styles.whyIcon}>LOW</div>
              <h3 className={styles.whyTitle}>Affordable Rates</h3>
              <p className={styles.whyText}>
                Competitive pricing with no hidden charges.
              </p>
            </div>
            <div className={styles.whyItem}>
              <div className={styles.whyIcon}>SEC</div>
              <h3 className={styles.whyTitle}>Safe & Secure</h3>
              <p className={styles.whyText}>
                Your parcels are handled with care and insured.
              </p>
            </div>
            <div className={styles.whyItem}>
              <div className={styles.whyIcon}>TOP</div>
              <h3 className={styles.whyTitle}>Trusted Partner</h3>
              <p className={styles.whyText}>
                Trusted by millions of customers nationwide.
              </p>
            </div>
          </div>
        </div>
      </section>

      <section className={styles.cta}>
        <div className={styles.ctaInner}>
          <h2 className={styles.ctaTitle}>Ready to Ship?</h2>
          <p className={styles.ctaText}>Get started with J&T Express today</p>
          <Link to="/services" className={styles.ctaBtn}>
            Explore Services
          </Link>
        </div>
      </section>
    </div>
  );
}
