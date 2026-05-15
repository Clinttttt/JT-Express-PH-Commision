import { useState, useCallback } from "react";
import { fetchBranches, createBranch, updateBranch, deleteBranch } from "../../../api/endpoints/branchesApi";
import type { Branch } from "../../../types";

export function useBranchesAdmin() {
  const [data, setData] = useState<Branch[] | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadBranches = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const branches = await fetchBranches();
      setData(branches);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  }, []);

  const create = useCallback(async (branch: Omit<Branch, "id">) => {
    try {
      const newBranch = await createBranch(branch);
      setData((prev) => (prev ? [...prev, newBranch] : [newBranch]));
      return newBranch;
    } catch (err) {
      throw new Error((err as Error).message);
    }
  }, []);

  const update = useCallback(async (id: number, branch: Omit<Branch, "id">) => {
    try {
      const updated = await updateBranch(id, branch);
      setData((prev) => prev?.map((b) => (b.id === id ? updated : b)) ?? null);
      return updated;
    } catch (err) {
      throw new Error((err as Error).message);
    }
  }, []);

  const remove = useCallback(async (id: number) => {
    try {
      await deleteBranch(id);
      setData((prev) => prev?.filter((b) => b.id !== id) ?? null);
    } catch (err) {
      throw new Error((err as Error).message);
    }
  }, []);

  return { data, loading, error, loadBranches, create, update, remove };
}
