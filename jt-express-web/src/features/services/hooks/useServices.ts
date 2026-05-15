import { useState, useEffect, useCallback } from "react";
import { fetchServices } from "../../../api/endpoints/servicesApi";
import type { Service } from "../../../types";

export function useServices() {
  const [data, setData] = useState<Service[] | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadServices = useCallback(() => {
    setLoading(true);
    setError(null);
    fetchServices()
      .then(setData)
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  useEffect(() => {
    loadServices();
  }, [loadServices]);

  return { data, loading, error, refetch: loadServices };
}
