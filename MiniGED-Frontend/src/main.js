import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import router from "./router";
import "./assets/main.css";
import "./style.css";
import Vue3Toastify from 'vue3-toastify';

const pinia = createPinia();
const app = createApp(App);
app.use(pinia);
app.use(router);
app.use(Vue3Toastify, {
  // Options
    position: "top-right",
    timeout: 3000,
    closeButton: true,
    progress: true,
});
app.mount("#app");
