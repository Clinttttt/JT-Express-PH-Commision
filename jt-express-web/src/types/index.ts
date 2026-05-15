export interface ApiResponse<T> {
  success: boolean;
  data: T | null;
  error: string | null;
}

export interface Service {
  id: number;
  name: string;
  description: string;
  icon: string;
  priceLabel: string;
}

export interface Rate {
  id: number;
  zone: string;
  firstKg: number;
  succeedingKg: number;
}

export interface RateCalculationResult {
  zone: string;
  weight: number;
  estimatedRate: number;
  formattedRate: string;
}

export interface TrackingEvent {
  date: string;
  status: string;
  location: string;
}

export interface TrackingEventWithId extends TrackingEvent {
  id: number;
}

export interface TrackingResult {
  trackingNumber: string;
  status: string;
  sender: string;
  recipient: string;
  estimatedDelivery: string;
  currentLocation: string;
  timeline: TrackingEvent[];
}

export interface Shipment {
  id: number;
  trackingNumber: string;
  status: string;
  sender: string;
  recipient: string;
  estimatedDelivery: string;
  currentLocation: string;
  timeline: TrackingEventWithId[];
}

export interface Branch {
  id: number;
  name: string;
  address: string;
  region: string;
  phone: string;
  hours: string;
  latitude: number;
  longitude: number;
}
