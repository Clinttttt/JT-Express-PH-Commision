import apiClient from "../client";
import type { ApiResponse } from "../../types";

export interface LoginResponse {
  token: string;
  username: string;
}

export interface SetupStatusResponse {
  hasAdmin: boolean;
}

export const checkSetupStatus = async (): Promise<SetupStatusResponse> => {
  const res = await apiClient.get<ApiResponse<SetupStatusResponse>>("/auth/setup-status");
  return res.data.data!;
};

export const signup = async (username: string, password: string): Promise<LoginResponse> => {
  const res = await apiClient.post<ApiResponse<LoginResponse>>("/auth/signup", {
    username,
    password,
  });
  return res.data.data!;
};

export const login = async (username: string, password: string): Promise<LoginResponse> => {
  const res = await apiClient.post<ApiResponse<LoginResponse>>("/auth/login", {
    username,
    password,
  });
  return res.data.data!;
};
