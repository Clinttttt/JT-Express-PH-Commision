const env = {
  apiBaseUrl: import.meta.env.VITE_API_BASE_URL as string,
} as const;

if (!env.apiBaseUrl) {
  throw new Error("VITE_API_BASE_URL is not defined. Check your .env file.");
}

export default env;
