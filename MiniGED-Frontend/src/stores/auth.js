import { defineStore } from "pinia";
import AuthService from "../services/auth";

const useAuthStore = defineStore("auth", {
  state: () => ({
    user: AuthService.getCurrentUser(),
    isAuthenticated: AuthService.isAuthenticated(),
    loading: false,
    error: null,
    refreshTokenTimeout: null
  }),

  actions: {
    async login(credentials) {
      this.loading = true;
      this.error = null;

      try {
        const { user, accessToken } = await AuthService.login(credentials);
        this.user = user;
        this.isAuthenticated = true;
        this.startRefreshTokenTimer(accessToken);
      } catch (error) {
        this.error = error.response?.data?.message || "Erreur de connexion";
        throw error;
      } finally {
        this.loading = false;
      }
    },

    async refreshToken() {
      try {
        const newAccessToken = await AuthService.refreshToken();
        this.startRefreshTokenTimer(newAccessToken);
      } catch (error) {
        this.logout();
      }
    },

    startRefreshTokenTimer(accessToken) {
      const jwtBase64 = accessToken.split('.')[1];
      const jwtToken = JSON.parse(atob(jwtBase64));

      const expires = new Date(jwtToken.exp * 1000);
      const timeout = expires.getTime() - Date.now() - (60 * 1000);

      this.refreshTokenTimeout = setTimeout(() => this.refreshToken(), timeout);
    },

    stopRefreshTokenTimer() {
      clearTimeout(this.refreshTokenTimeout);
    },

    logout() {
      AuthService.logout();
      this.user = null;
      this.isAuthenticated = false;
      this.stopRefreshTokenTimer();
    }
  }
});

export { useAuthStore };