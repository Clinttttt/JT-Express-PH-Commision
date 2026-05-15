import { useState, useEffect } from "react";
import { fetchBranches } from "../../../api/endpoints/branchesApi";
import type { Branch } from "../../../types";

export function useBranches(region?: string) {
  const [data, setData] = useState<Branch[] | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setLoading(true);
    setError(null);
    fetchBranches(region)
      .then(setData)
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, [region]);

  return { data, loading, error };
}
