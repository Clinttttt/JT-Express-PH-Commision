import apiClient from "../client";
import type { ApiResponse, Branch } from "../../types";

export const fetchBranches = async (region?: string): Promise<Branch[]> => {
  const params = region ? { region } : {};
  const res = await apiClient.get<ApiResponse<Branch[]>>("/branches", { params });
  return res.data.data ?? [];
};

export const createBranch = async (data: Omit<Branch, "id">): Promise<Branch> => {
  const res = await apiClient.post<ApiResponse<Branch>>("/branches", data);
  return res.data.data!;
};

export const updateBranch = async (id: number, data: Omit<Branch, "id">): Promise<Branch> => {
  const res = await apiClient.put<ApiResponse<Branch>>(`/branches/${id}`, data);
  return res.data.data!;
};

export const deleteBranch = async (id: number): Promise<void> => {
  await apiClient.delete(`/branches/${id}`);
};
