import { useState, useEffect, useRef } from "react";
import { trackParcel } from "../../../api/endpoints/trackingApi";
import type { TrackingResult } from "../../../types";

export function useTracking() {
  const [data, setData] = useState<TrackingResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const intervalRef = useRef<NodeJS.Timeout | null>(null);

  const track = async (trackingNumber: string) => {
    setLoading(true);
    setError(null);
    setData(null);

    try {
      const result = await trackParcel(trackingNumber);
      setData(result);

      // Start polling for updates every 10 seconds
      if (intervalRef.current) clearInterval(intervalRef.current);
      intervalRef.current = setInterval(async () => {
        try {
          const updated = await trackParcel(trackingNumber);
          setData(updated);
        } catch (err) {
          // Silent fail on polling
        }
      }, 10000);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    return () => {
      if (intervalRef.current) clearInterval(intervalRef.current);
    };
  }, []);

  return { data, loading, error, track };
}
