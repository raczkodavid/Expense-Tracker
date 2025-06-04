import axios from "axios";
import type { AxiosInstance } from "axios";

// Get API URL from environment variables, with fallback
const apiUrl = import.meta.env.VITE_API_URL || "/api";

// create an instance of axios
const api: AxiosInstance = axios.create({
  baseURL: apiUrl,
  timeout: 10000,
  headers: {
    "Content-Type": "application/json",
  },
});

export default api;
