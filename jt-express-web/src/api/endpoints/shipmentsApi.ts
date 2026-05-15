import apiClient from "../client";
import type { ApiResponse, Shipment, TrackingEventWithId } from "../../types";

export const fetchAllShipments = async (): Promise<Shipment[]> => {
  const res = await apiClient.get<ApiResponse<Shipment[]>>("/shipments");
  return res.data.data ?? [];
};

export const createShipment = async (data: {
  trackingNumber: string;
  sender: string;
  recipient: string;
  estimatedDelivery: string;
}): Promise<Shipment> => {
  const res = await apiClient.post<ApiResponse<Shipment>>("/shipments", data);
  return res.data.data!;
};

export const updateShipment = async (
  id: number,
  data: { status: string; currentLocation: string }
): Promise<Shipment> => {
  const res = await apiClient.put<ApiResponse<Shipment>>(`/shipments/${id}`, data);
  return res.data.data!;
};

export const addTrackingEvent = async (
  shipmentId: number,
  data: { date: string; status: string; location: string }
): Promise<void> => {
  await apiClient.post(`/shipments/${shipmentId}/events`, data);
};

export const deleteShipment = async (id: number): Promise<void> => {
  await apiClient.delete(`/shipments/${id}`);
};
