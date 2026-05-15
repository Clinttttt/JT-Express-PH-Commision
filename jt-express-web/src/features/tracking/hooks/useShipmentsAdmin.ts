import { useState, useEffect, useCallback } from "react";
import { fetchAllShipments, createShipment, updateShipment, addTrackingEvent, deleteShipment } from "../../../api/endpoints/shipmentsApi";
import type { Shipment } from "../../../types";

export function useShipmentsAdmin() {
  const [data, setData] = useState<Shipment[] | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadShipments = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const shipments = await fetchAllShipments();
      setData(shipments);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    loadShipments();
  }, [loadShipments]);

  const create = useCallback(async (shipment: Omit<Shipment, "id" | "timeline">) => {
    try {
      const newShipment = await createShipment({
        trackingNumber: shipment.trackingNumber,
        sender: shipment.sender,
        recipient: shipment.recipient,
        estimatedDelivery: shipment.estimatedDelivery,
      });
      setData((prev) => (prev ? [newShipment, ...prev] : [newShipment]));
      return newShipment;
    } catch (err) {
      throw new Error((err as Error).message);
    }
  }, []);

  const update = useCallback(async (id: number, data: { status: string; currentLocation: string }) => {
    try {
      const updated = await updateShipment(id, data);
      setData((prev) => prev?.map((s) => (s.id === id ? updated : s)) ?? null);
      return updated;
    } catch (err) {
      throw new Error((err as Error).message);
    }
  }, []);

  const addEvent = useCallback(async (shipmentId: number, event: { date: string; status: string; location: string }) => {
    try {
      await addTrackingEvent(shipmentId, event);
      await loadShipments();
    } catch (err) {
      throw new Error((err as Error).message);
    }
  }, [loadShipments]);

  const remove = useCallback(async (id: number) => {
    try {
      await deleteShipment(id);
      setData((prev) => prev?.filter((s) => s.id !== id) ?? null);
    } catch (err) {
      throw new Error((err as Error).message);
    }
  }, []);

  return { data, loading, error, loadShipments, create, update, addEvent, remove };
}
