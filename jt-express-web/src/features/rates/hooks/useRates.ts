import { useState, useEffect } from "react";
import { fetchRates } from "../../../api/endpoints/ratesApi";
import type { Rate } from "../../../types";

export function useRates() {
  const [data, setData] = useState<Rate[] | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchRates()
      .then(setData)
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  return { data, loading, error };
}
