import { useState, useCallback } from "react";
import { fetchRates, createRate, updateRate, deleteRate } from "../../../api/endpoints/ratesApi";
import type { Rate } from "../../../types";

export function useRatesAdmin() {
  const [data, setData] = useState<Rate[] | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadRates = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const rates = await fetchRates();
      setData(rates);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  }, []);

  const create = useCallback(async (rate: Omit<Rate, "id">) => {
    try {
      const newRate = await createRate(rate);
      setData((prev) => (prev ? [...prev, newRate] : [newRate]));
      return newRate;
    } catch (err) {
      throw new Error((err as Error).message);
    }
  }, []);

  const update = useCallback(async (id: number, rate: Omit<Rate, "id">) => {
    try {
      const updated = await updateRate(id, rate);
      setData((prev) => prev?.map((r) => (r.id === id ? updated : r)) ?? null);
      return updated;
    } catch (err) {
      throw new Error((err as Error).message);
    }
  }, []);

  const remove = useCallback(async (id: number) => {
    try {
      await deleteRate(id);
      setData((prev) => prev?.filter((r) => r.id !== id) ?? null);
    } catch (err) {
      throw new Error((err as Error).message);
    }
  }, []);

  return { data, loading, error, loadRates, create, update, remove };
}
