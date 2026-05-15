import apiClient from "../client";
import type { ApiResponse, TrackingResult } from "../../types";

export const trackParcel = async (trackingNumber: string): Promise<TrackingResult> => {
  const res = await apiClient.get<ApiResponse<TrackingResult>>(
    `/tracking/${encodeURIComponent(trackingNumber)}`
  );
  return res.data.data!;
};
