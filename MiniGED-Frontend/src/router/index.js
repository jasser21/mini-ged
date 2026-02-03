import { createRouter, createWebHistory } from "vue-router";
import LoginView from "../views/LoginView.vue";
import RegisterView from "../views/RegisterView.vue";
import DashboardView from "../views/DashboardView.vue";
import DocumentsList from "../components/DocumentsList.vue";
import PresentationView from "../views/PresentationView.vue";
import UsersList from "../components/UsersList.vue";
import DocumentDetails from "../components/DocumentDetails.vue";
import { useAuthStore } from "../stores/auth";
import UserDetails from "../components/UserDetails.vue";
import HomeView from "../components/HomeView.vue";

const routes = [
  { path: "/login", component: LoginView },
  { path: "/register", component: RegisterView },
  {
    path: "",
    component: DashboardView,
    children: [
      { path: "", component:HomeView },
      { path: "documents", component: DocumentsList },
      { path: "document/:id", component: DocumentDetails },
      { path: "users", component: UsersList },
      {path:"user/:id", component:UserDetails},
      
    ],
  },
  {
    path: "/presentation",
    component: PresentationView,
  },
  { path: "/", redirect: "/login" },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();

  if (!authStore.isAuthenticated && to.path !== "/login") {
    // Save the full path user was trying to access
    authStore.returnUrl = to.fullPath;
    next("/login");
  } else if (
    (to.path === "/login" || to.path === "/register") &&
    authStore.isAuthenticated
  ) {
    next("/");
  } else {
    next();
  }
});


export default router;
