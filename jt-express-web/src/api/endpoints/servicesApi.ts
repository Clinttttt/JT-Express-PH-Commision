import apiClient from "../client";
import type { ApiResponse, Service } from "../../types";

export const fetchServices = async (): Promise<Service[]> => {
  const res = await apiClient.get<ApiResponse<Service[]>>("/services");
  return res.data.data ?? [];
};

export const createService = async (data: Omit<Service, "id">): Promise<Service> => {
  const res = await apiClient.post<ApiResponse<Service>>("/services", data);
  return res.data.data!;
};

export const updateService = async (id: number, data: Omit<Service, "id">): Promise<Service> => {
  const res = await apiClient.put<ApiResponse<Service>>(`/services/${id}`, data);
  return res.data.data!;
};

export const deleteService = async (id: number): Promise<void> => {
  await apiClient.delete(`/services/${id}`);
};
