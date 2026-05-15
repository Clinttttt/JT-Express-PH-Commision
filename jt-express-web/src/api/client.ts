import axios from "axios";
import env from "../config/env";

const apiClient = axios.create({
  baseURL: env.apiBaseUrl,
  headers: { "Content-Type": "application/json" },
  timeout: 10_000,
});

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

apiClient.interceptors.response.use(
  (response) => {
    const body = response.data;
    if (body && body.success === false) {
      return Promise.reject(new Error(body.error ?? "An error occurred."));
    }
    return response;
  },
  (error) => {
    const message =
      error.response?.data?.error ??
      error.message ??
      "Network error. Please try again.";
    return Promise.reject(new Error(message));
  }
);

export default apiClient;
