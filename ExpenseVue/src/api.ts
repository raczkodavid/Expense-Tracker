import axios from "axios";
import type { AxiosInstance } from "axios";

// Get API URL from environment variables, with fallback
const apiUrl = import.meta.env.VITE_API_URL || "http://localhost:5111/api";

// create an instance of axios
const api: AxiosInstance = axios.create({
  baseURL: apiUrl,
  timeout: 10000,
  headers: {
    "Content-Type": "application/json",
  },
});

// interceptors (token, error handling, logging) : TODO

export default api;
