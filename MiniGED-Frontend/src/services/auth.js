import axios from "axios";
import apiClient from "./api";
import userService from "./userService";
import { defineStore } from "pinia";

const API_URL = "http://localhost:5092/api/account"; // Remplacez par votre URL d'API

class AuthService {
  async login(credentials) {
    const response = await apiClient.post(`${API_URL}/login`, credentials);

    if (response.data.accessToken && response.data.refreshToken) {
      // Stockage des deux tokens
      localStorage.setItem("token", response.data.accessToken);
      localStorage.setItem("refreshToken", response.data.refreshToken);

      // Récupération des infos utilisateur
      const user = await userService.getUserAuth();
      localStorage.setItem("user", JSON.stringify(user.data));

      return {
        user: user.data,
        accessToken: response.data.accessToken,
        refreshToken: response.data.refreshToken
      };
    }

    return null;
  }

  async refreshToken() {
    const refreshToken = localStorage.getItem("refreshToken");
    if (!refreshToken) throw new Error("Pas de refresh token");

    const response = await apiClient.post(`${API_URL}/refresh-token`, { refreshToken });

    if (response.data.accessToken) {
      localStorage.setItem("token", response.data.accessToken);
      return response.data.accessToken;
    }

    throw new Error("Impossible de rafraîchir le token");
  }

  logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("user");
  }

  getCurrentUser() {
    const user = localStorage.getItem("user");
    return user ? JSON.parse(user) : null;
  }

  getToken() {
    return localStorage.getItem("token");
  }

  isAuthenticated() {
    return !!this.getToken();
  }
}

export default new AuthService();
