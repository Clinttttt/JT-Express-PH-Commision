import { Link, NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "../../../context/AuthContext";
import styles from "./Navbar.module.css";

const NAV_LINKS = [
  { to: "/services", label: "Services" },
  { to: "/rates", label: "Rates" },
  { to: "/tracking", label: "Track Parcel" },
  { to: "/branches", label: "Branches" },
] as const;

export default function Navbar() {
  const { isLoggedIn, username, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  return (
    <nav className={styles.nav} aria-label="Main navigation">
      <div className={styles.inner}>
        <Link to="/" className={styles.brand} aria-label="J&T Express Philippines home">
          J&T Express <span className={styles.brandSub}>PH</span>
        </Link>
        
        <ul className={styles.links} role="list">
          {NAV_LINKS.map(({ to, label }) => (
            <li key={to}>
              <NavLink
                to={to}
                className={({ isActive }) =>
                  isActive ? `${styles.link} ${styles.linkActive}` : styles.link
                }
                aria-current={({ isActive }) => (isActive ? "page" : undefined)}
              >
                {label}
              </NavLink>
            </li>
          ))}
        </ul>

        <div className={styles.authSection}>
          {isLoggedIn ? (
            <div className={styles.userInfo}>
              <div className={styles.adminLinks}>
                <Link to="/admin/branches" className={styles.adminLink}>
                  Manage Branches
                </Link>
                <Link to="/admin/rates" className={styles.adminLink}>
                  Manage Rates
                </Link>
                <Link to="/admin/shipments" className={styles.adminLink}>
                  Manage Shipments
                </Link>
              </div>
              <span className={styles.username} aria-label={`Logged in as ${username}`}>
                {username}
              </span>
              <button className={styles.logoutBtn} onClick={handleLogout} aria-label="Logout">
                Logout
              </button>
            </div>
          ) : (
            <Link to="/login" className={styles.loginBtn}>
              Admin Login
            </Link>
          )}
        </div>
      </div>
    </nav>
  );
}
