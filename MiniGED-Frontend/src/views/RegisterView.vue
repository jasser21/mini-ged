<template>
  <div class="auth-container">
    <div class="auth-left">
      <div class="brand">
        <div>
          <img class="brand-icon" src="../assets/images/logo1.png" alt="Logo" />
        </div>
        <h2>Mini GED</h2>
      </div>

      <div class="illustration">
        <div class="illustration-circle">
          <div>
            <img
              class="person-icon"
              src="../assets/images/illustration.svg"
              alt="Illustration"
            />
          </div>
        </div>
      </div>

      <div class="left-content">
        <h3>Plateforme GED intelligente avec IA intégrée</h3>
        <p>
          Gérez, classez et retrouvez vos documents en un clic grâce à
          l'intelligence artificielle
        </p>
      </div>
    </div>

    <div class="auth-right">
      <div class="form-container">
        <div class="form-header">
          <h2>Bienvenue sur Mini GED</h2>
        </div>

        <form @submit.prevent="handleRegister" class="auth-form">
          <div class="form-group">
            <input
              v-model="userData.Username"
              type="text"
              placeholder="Nom d'utilisateur"
              required
              :disabled="loading"
            />
          </div>

          <div class="form-group">
            <input
              v-model="userData.FullName"
              type="text"
              placeholder="Nom complet"
              required
              :disabled="loading"
            />
          </div>

          <div class="form-group">
            <input
              v-model="userData.Email"
              type="email"
              placeholder="Email"
              required
              :disabled="loading"
            />
          </div>

          <div class="form-group">
            <input
              v-model="userData.PhoneNumber"
              type="tel"
              placeholder="+2160000000"
              required
              :disabled="loading"
            />
          </div>

          <div class="form-group">
            <input
              v-model="userData.Password"
              type="password"
              placeholder="Mot de passe"
              required
              :disabled="loading"
            />
          </div>

          <div class="form-group">
            <input
              v-model="confirmPassword"
              type="password"
              placeholder="Confirmer le mot de passe"
              required
              :disabled="loading"
            />
          </div>

          <div v-if="error" class="error-message">
            {{ error }}
          </div>

          <div v-if="success" class="success-message">
            {{ success }}
          </div>

          <button type="submit" :disabled="loading" class="submit-btn">
            {{ loading ? "Inscription..." : "S'INSCRIRE" }}
          </button>
        </form>

        <div class="form-footer">
          <p>
            Vous avez déjà un compte ?
            <router-link to="/login" class="link">Se connecter</router-link>
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";

const router = useRouter();
const authStore = useAuthStore();

const userData = ref({
  Username: "",
  FullName: "",
  Email: "",
  Password: "",
  PhoneNumber: "",
});

const confirmPassword = ref("");

const loading = ref(false);
const error = ref(null);
const success = ref(null);

const handleRegister = async () => {
  if (userData.value.Password !== confirmPassword.value) {
    error.value = "Les mots de passe ne correspondent pas";
    return;
  }

  try {
    loading.value = true;
    error.value = null;
    success.value = null;

    await authStore.register(userData.value);
    success.value =
      "Inscription réussie ! Vous pouvez maintenant vous connecter.";

    setTimeout(() => {
      router.push("/login");
    }, 2000);
  } catch (err) {
    error.value = authStore.error;
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
.auth-container {
  display: flex;
  min-height: 100vh;
  background: #f5f5f5;
}

.auth-left {
  flex: 1;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  display: flex;
  flex-direction: column;
  padding: 3rem;
  position: relative;
  overflow: hidden;
}

.auth-left::before {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(
    135deg,
    rgba(102, 126, 234, 0.9) 0%,
    rgba(118, 75, 162, 0.9) 100%
  );
}

.brand {
  display: flex;
  align-items: center;
  gap: 1rem;
  z-index: 2;
  position: relative;
}

.brand-icon {
  font-size: 2rem;
  width: 60px;
  height: 60px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.brand h2 {
  font-size: 1.8rem;
  font-weight: 600;
  margin: 0;
}

.illustration {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 2;
  position: relative;
}

.illustration-circle {
  width: 300px;
  height: 300px;
  background: rgba(255, 255, 255, 0.15);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  backdrop-filter: blur(10px);
}

.person-icon {
  width: 250px;
  height: 250px;
}

.left-content {
  z-index: 2;
  position: relative;
  text-align: center;
}

.left-content h3 {
  font-size: 1.8rem;
  margin-bottom: 1rem;
  font-weight: 600;
  line-height: 1.3;
}

.left-content p {
  font-size: 1.1rem;
  opacity: 0.9;
  line-height: 1.5;
}

.auth-right {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 3rem;
  background: white;
}

.form-container {
  width: 100%;
  max-width: 400px;
}

.form-header h2 {
  font-size: 1.8rem;
  color: #333;
  margin-bottom: 2rem;
  font-weight: 600;
  line-height: 1.3;
}

.auth-form {
  margin-bottom: 2rem;
}

.form-group {
  margin-bottom: 1.5rem;
}

.form-group input {
  width: 100%;
  padding: 1rem;
  border: 2px solid #e1e5e9;
  border-radius: 8px;
  font-size: 1rem;
  transition: border-color 0.3s ease;
  background: #f8f9fa;
}

.form-group input:focus {
  outline: none;
  border-color: #667eea;
  background: white;
}

.error-message {
  color: #dc3545;
  margin-bottom: 1rem;
  padding: 0.5rem;
  background: #f8d7da;
  border-radius: 4px;
  font-size: 0.9rem;
}

.success-message {
  color: #28a745;
  margin-bottom: 1rem;
  padding: 0.5rem;
  background: #d4edda;
  border-radius: 4px;
  font-size: 0.9rem;
}

.submit-btn {
  width: 100%;
  padding: 1rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.submit-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(102, 126, 234, 0.3);
}

.submit-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
  transform: none;
}

.form-footer {
  text-align: center;
  color: #666;
}

.form-footer .link {
  color: #667eea;
  text-decoration: none;
  font-weight: 600;
}

.form-footer .link:hover {
  text-decoration: underline;
}

@media (max-width: 768px) {
  .auth-container {
    flex-direction: column;
  }

  .auth-left {
    min-height: 40vh;
    padding: 2rem;
  }

  .illustration-circle {
    width: 180px;
    height: 180px;
  }

  .person-icon {
    width: 150px;
    height: 150px;
  }

  .left-content h3 {
    font-size: 1.5rem;
  }

  .auth-right {
    padding: 2rem;
  }
}
</style>
