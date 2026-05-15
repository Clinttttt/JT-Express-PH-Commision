import apiClient from "../client";
import type { ApiResponse, Rate, RateCalculationResult } from "../../types";

export const fetchRates = async (): Promise<Rate[]> => {
  const res = await apiClient.get<ApiResponse<Rate[]>>("/rates");
  return res.data.data ?? [];
};

export const calculateRate = async (
  zone: string,
  weight: number
): Promise<RateCalculationResult> => {
  const res = await apiClient.get<ApiResponse<RateCalculationResult>>(
    "/rates/calculate",
    { params: { zone, weight } }
  );
  return res.data.data!;
};

export const createRate = async (data: Omit<Rate, "id">): Promise<Rate> => {
  const res = await apiClient.post<ApiResponse<Rate>>("/rates", data);
  return res.data.data!;
};

export const updateRate = async (id: number, data: Omit<Rate, "id">): Promise<Rate> => {
  const res = await apiClient.put<ApiResponse<Rate>>(`/rates/${id}`, data);
  return res.data.data!;
};

export const deleteRate = async (id: number): Promise<void> => {
  await apiClient.delete(`/rates/${id}`);
};
