import { useState } from "react";
import LoadingSpinner from "../../components/shared/LoadingSpinner/LoadingSpinner";
import ErrorMessage from "../../components/shared/ErrorMessage/ErrorMessage";
import { useBranches } from "./hooks/useBranches";
import styles from "./BranchesPage.module.css";

const REGIONS = ["All", "Metro Manila", "Luzon", "Visayas", "Mindanao"] as const;

export default function BranchesPage() {
  const [selectedRegion, setSelectedRegion] = useState<string>("All");
  const { data, loading, error } = useBranches(selectedRegion === "All" ? undefined : selectedRegion);

  const groupedBranches = data ? data.reduce((acc, branch) => {
    const region = branch.region;
    if (!acc[region]) acc[region] = [];
    acc[region].push(branch);
    return acc;
  }, {} as Record<string, typeof data>) : {};

  return (
    <div className="page-container">
      <div className="page-header">
        <h1 className="page-title">Branch Locator</h1>
        <p className="page-subtitle">Visit any of our 1,500+ branches nationwide</p>
      </div>

      <div className={styles.filters}>
        {REGIONS.map((region) => (
          <button
            key={region}
            className={`${styles.filterBtn} ${selectedRegion === region ? styles.filterBtnActive : ""}`}
            onClick={() => setSelectedRegion(region)}
          >
            {region}
          </button>
        ))}
      </div>

      {loading && <LoadingSpinner message="Loading branches..." />}
      {error && <ErrorMessage message={error} />}
      {data && (
        <div className={styles.branchesContainer}>
          {Object.entries(groupedBranches).map(([region, branches]) => (
            <div key={region} className={styles.regionSection}>
              <h2 className={styles.regionTitle}>{region}</h2>
              <div className={styles.grid}>
                {branches?.map((branch) => (
                  <div key={branch.id} className={styles.card}>
                    <h3 className={styles.name}>{branch.name}</h3>
                    <p className={styles.address}>📍 {branch.address}</p>
                    <div className={styles.details}>
                      <p className={styles.info}>📞 {branch.phone}</p>
                      <p className={styles.info}>🕐 {branch.hours}</p>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
