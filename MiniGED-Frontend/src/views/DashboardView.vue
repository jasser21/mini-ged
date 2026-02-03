<template>
  <div class="dashboard-container">
    <!-- Sidebar -->
    <aside class="sidebar" :class="{ collapsed: sidebarCollapsed }">
      <div class="sidebar-header">
        <div class="logo">
          <img src="../assets/images/Logo1.png" alt="MINIGED Logo" class="logo-icon" />
          <span class="logo-text" v-show="!sidebarCollapsed">Mini<span class="logo-text-highlight">GED</span></span>
        </div>
      </div>


      <nav class="sidebar-nav">
        <ul class="nav-list">
          <li class="nav-item" :class="{ active: $route.path === '' }" @click="navigateToHome">
            <span class="nav-icon">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                stroke="currentColor" class="size-6">
                <path stroke-linecap="round" stroke-linejoin="round"
                  d="m2.25 12 8.954-8.955c.44-.439 1.152-.439 1.591 0L21.75 12M4.5 9.75v10.125c0 .621.504 1.125 1.125 1.125H9.75v-4.875c0-.621.504-1.125 1.125-1.125h2.25c.621 0 1.125.504 1.125 1.125V21h4.125c.621 0 1.125-.504 1.125-1.125V9.75M8.25 21h8.25" />
              </svg>

            </span>
            <span class="nav-text" v-show="!sidebarCollapsed">Accueil</span>
          </li>
          <li class="nav-item" :class="{ active: $route.path === '/documents' }" @click="navigateToDocuments">
            <span class="nav-icon">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                stroke="currentColor" class="size-6">
                <path stroke-linecap="round" stroke-linejoin="round"
                  d="M15.75 17.25v3.375c0 .621-.504 1.125-1.125 1.125h-9.75a1.125 1.125 0 0 1-1.125-1.125V7.875c0-.621.504-1.125 1.125-1.125H6.75a9.06 9.06 0 0 1 1.5.124m7.5 10.376h3.375c.621 0 1.125-.504 1.125-1.125V11.25c0-4.46-3.243-8.161-7.5-8.876a9.06 9.06 0 0 0-1.5-.124H9.375c-.621 0-1.125.504-1.125 1.125v3.5m7.5 10.375H9.375a1.125 1.125 0 0 1-1.125-1.125v-9.25m12 6.625v-1.875a3.375 3.375 0 0 0-3.375-3.375h-1.5a1.125 1.125 0 0 1-1.125-1.125v-1.5a3.375 3.375 0 0 0-3.375-3.375H9.75" />
              </svg>

            </span>
            <span class="nav-text" v-show="!sidebarCollapsed">Mes documents</span>
            <span class="nav-badge" v-show="!sidebarCollapsed">{{
              documents.length
            }}</span>
          </li>
          <li class="nav-item" :class="{ active: $route.path === '/users' }" @click="navigateToUsers">
            <span class="nav-icon">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                stroke="currentColor" class="size-6">
                <path stroke-linecap="round" stroke-linejoin="round"
                  d="M15 19.128a9.38 9.38 0 0 0 2.625.372 9.337 9.337 0 0 0 4.121-.952 4.125 4.125 0 0 0-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 0 1 8.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0 1 11.964-3.07M12 6.375a3.375 3.375 0 1 1-6.75 0 3.375 3.375 0 0 1 6.75 0Zm8.25 2.25a2.625 2.625 0 1 1-5.25 0 2.625 2.625 0 0 1 5.25 0Z" />
              </svg>

            </span>
            <span class="nav-text" v-show="!sidebarCollapsed">Utilisateurs</span>
          </li>
          <!-- <li class="nav-item">
            <span class="nav-icon">📄</span>
            <span class="nav-text" v-show="!sidebarCollapsed">Mes résumés</span>
          </li> -->
          <li class="nav-item">
            <span class="nav-icon">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                stroke="currentColor" class="size-6">
                <path stroke-linecap="round" stroke-linejoin="round"
                  d="M9.594 3.94c.09-.542.56-.94 1.11-.94h2.593c.55 0 1.02.398 1.11.94l.213 1.281c.063.374.313.686.645.87.074.04.147.083.22.127.325.196.72.257 1.075.124l1.217-.456a1.125 1.125 0 0 1 1.37.49l1.296 2.247a1.125 1.125 0 0 1-.26 1.431l-1.003.827c-.293.241-.438.613-.43.992a7.723 7.723 0 0 1 0 .255c-.008.378.137.75.43.991l1.004.827c.424.35.534.955.26 1.43l-1.298 2.247a1.125 1.125 0 0 1-1.369.491l-1.217-.456c-.355-.133-.75-.072-1.076.124a6.47 6.47 0 0 1-.22.128c-.331.183-.581.495-.644.869l-.213 1.281c-.09.543-.56.94-1.11.94h-2.594c-.55 0-1.019-.398-1.11-.94l-.213-1.281c-.062-.374-.312-.686-.644-.87a6.52 6.52 0 0 1-.22-.127c-.325-.196-.72-.257-1.076-.124l-1.217.456a1.125 1.125 0 0 1-1.369-.49l-1.297-2.247a1.125 1.125 0 0 1 .26-1.431l1.004-.827c.292-.24.437-.613.43-.991a6.932 6.932 0 0 1 0-.255c.007-.38-.138-.751-.43-.992l-1.004-.827a1.125 1.125 0 0 1-.26-1.43l1.297-2.247a1.125 1.125 0 0 1 1.37-.491l1.216.456c.356.133.751.072 1.076-.124.072-.044.146-.086.22-.128.332-.183.582-.495.644-.869l.214-1.28Z" />
                <path stroke-linecap="round" stroke-linejoin="round" d="M15 12a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z" />
              </svg>

            </span>
            <span class="nav-text" v-show="!sidebarCollapsed">Paramètres</span>
          </li>
          <li class="nav-item">
            <span class="nav-icon">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                stroke="currentColor" class="size-6">
                <path stroke-linecap="round" stroke-linejoin="round"
                  d="m11.25 11.25.041-.02a.75.75 0 0 1 1.063.852l-.708 2.836a.75.75 0 0 0 1.063.853l.041-.021M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0Zm-9-3.75h.008v.008H12V8.25Z" />
              </svg>

            </span>
            <span class="nav-text" v-show="!sidebarCollapsed">Aide & Assistance</span>
          </li>
        </ul>
      </nav>

      <div class="sidebar-footer">
        <div class="user-profile">
          <div class="user-avatar">
            {{ "U" }}
          </div>
          <div class="user-info" v-show="!sidebarCollapsed">
            <div class="user-name">
              {{ CurrentUser?.fullName || "Utilisateur" }}
            </div>
          </div>
        </div>
      </div>
    </aside>

    <!-- Main Content -->
    <main class="main-content">
      <header class="content-header">
        <div class="flex">
          <button class="sidebar-toggle" @click="toggleSidebar">
            <span v-if="sidebarCollapsed"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"
                fill="currentColor" class="size-5">
                <path fill-rule="evenodd"
                  d="M15.28 9.47a.75.75 0 0 1 0 1.06l-4.25 4.25a.75.75 0 1 1-1.06-1.06L13.69 10 9.97 6.28a.75.75 0 0 1 1.06-1.06l4.25 4.25ZM6.03 5.22l4.25 4.25a.75.75 0 0 1 0 1.06l-4.25 4.25a.75.75 0 0 1-1.06-1.06L8.69 10 4.97 6.28a.75.75 0 0 1 1.06-1.06Z"
                  clip-rule="evenodd" />
              </svg>


            </span>
            <span v-else><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" class="size-5">
                <path fill-rule="evenodd"
                  d="M4.72 9.47a.75.75 0 0 0 0 1.06l4.25 4.25a.75.75 0 1 0 1.06-1.06L6.31 10l3.72-3.72a.75.75 0 1 0-1.06-1.06L4.72 9.47Zm9.25-4.25L9.72 9.47a.75.75 0 0 0 0 1.06l4.25 4.25a.75.75 0 1 0 1.06-1.06L11.31 10l3.72-3.72a.75.75 0 0 0-1.06-1.06Z"
                  clip-rule="evenodd" />
              </svg>


            </span>
          </button>
          <!-- <nav class="breadcrumb">
            <span class="breadcrumb-item">Tableau de bord</span>
            <span class="breadcrumb-item">Documents</span>
          </nav> -->

        </div>
        <div class="header-actions">
          <div class="relative flex items-center">
            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                stroke="currentColor" class="w-5 h-5 text-gray-400">
                <path stroke-linecap="round" stroke-linejoin="round"
                  d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
              </svg>
            </div>
            <input type="text" placeholder="Recherche..." @click="openSearchModal"
              class="w-64 py-2 pl-10 pr-4 bg-gray-50 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent text-sm" />
          </div>
          <SearchModal v-if="isSearchOpen" @close="isSearchOpen = false" />
          <button class="settings-btn">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
              stroke="currentColor" class="size-6">
              <path stroke-linecap="round" stroke-linejoin="round"
                d="M10.5 6h9.75M10.5 6a1.5 1.5 0 1 1-3 0m3 0a1.5 1.5 0 1 0-3 0M3.75 6H7.5m3 12h9.75m-9.75 0a1.5 1.5 0 0 1-3 0m3 0a1.5 1.5 0 0 0-3 0m-3.75 0H7.5m9-6h3.75m-3.75 0a1.5 1.5 0 0 1-3 0m3 0a1.5 1.5 0 0 0-3 0m-9.75 0h9.75" />
            </svg>

          </button>
          <button class="notifications-btn">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
              stroke="currentColor" class="size-6">
              <path stroke-linecap="round" stroke-linejoin="round"
                d="M14.857 17.082a23.848 23.848 0 0 0 5.454-1.31A8.967 8.967 0 0 1 18 9.75V9A6 6 0 0 0 6 9v.75a8.967 8.967 0 0 1-2.312 6.022c1.733.64 3.56 1.085 5.455 1.31m5.714 0a24.255 24.255 0 0 1-5.714 0m5.714 0a3 3 0 1 1-5.714 0" />
            </svg>

          </button>


          <DropdownComponent button-text="Custom Button" :items="customItems" @item-clicked="handleItemClick" />
        </div>
      </header>

      <router-view />
    </main>
  </div>


</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";
import { getUserDocuments } from "../services/documentService";
import SearchModal from "../components/SearchFeature/SearchModal.vue";
import DropdownComponent from "../components/DropdownComponent.vue";

const authStore = useAuthStore();

const handleLogout = () => {
  authStore.logout();
  window.location.href = "/login";
  console.log('User logged out');
};
const customItems = [
  { text: 'Profile', href: '#', action: 'profile' },
  { text: 'Messages', href: '#', action: 'messages' },
  { text: 'Logout', href: '#', action: 'logout' },
];

const handleItemClick = (action) => {
  console.log('Item clicked:', action);
  if (action.action == 'logout') {
    handleLogout();
  }
  // Handle the action
};
const router = useRouter();
const sidebarCollapsed = ref(false);
const isSearchOpen = ref(false);
const CurrentUser = computed(() => authStore.user);
const toggleSidebar = () => {
  sidebarCollapsed.value = !sidebarCollapsed.value;
};


const openSearchModal = () => {
  isSearchOpen.value = true;
};


const navigateToDocuments = () => {
  router.push("/documents");
};
const navigateToUsers = () => {
  router.push("/users");
};
const navigateToHome = () => {
  console.log("Navigating to home");
  router.push("/");
};

const documents = ref([]);
const loading = ref(false);
const error = ref(null);

onMounted(async () => {
  loading.value = true;
  try {
    const response = await getUserDocuments();
    documents.value = response;
  } catch (err) {
    error.value = "Erreur lors du chargement des documents";
    console.error(err);
  } finally {
    loading.value = false;
  }
});
</script>

<style scoped>
.dashboard-container {
  display: flex;
  height: 100vh;
  background-color: #f8f9fa;
}

/* Sidebar Styles */
.sidebar {
  width: 280px;
  background-color: white;

  display: flex;
  flex-direction: column;
  transition: width 0.3s ease;
  position: relative;
}

.sidebar.collapsed {
  width: 80px;
}

@media (max-width: 768px) {
  .sidebar {
    position: fixed;
    left: 0;
    top: 0;
    height: 100vh;
    z-index: 1000;
    transform: translateX(0);
  }

  .sidebar.collapsed {
    transform: translateX(-100%);
    width: 280px;
  }
}

.sidebar-header {
  border-bottom: 1px solid #e9ecef;
  display: flex;
  height: 90px;
  justify-content: start;
  padding: 1rem;
  align-items: center;
  border-right: 1px solid #e9ecef;
}

.sidebar-toggle {
  background: none;
  border: none;
  font-size: 18px;
  cursor: pointer;
  padding: 0.5rem;
  border-radius: 4px;
  transition: background-color 0.2s;
}

.sidebar-toggle:hover {
  background-color: #f7fafc;
}

.sidebar.collapsed .sidebar-toggle {
  margin-left: auto;
  margin-right: auto;
}

.logo {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.logo-icon {
  width: 42px;
  height: 42px;
  border-radius: 8px;
  object-fit: contain;
}

.logo-text {
  font-weight: bold;
  font-size: 18px;
  color: #2d3748;
  font-family: 'Inter', sans-serif;
}

.logo-text-highlight {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  background-clip: text;
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  font-family: 'Inter', sans-serif;
  color: #667eea;
  /* Fallback color */
}


.search-input:focus {
  outline: none;
  border-color: #667eea;
  background-color: white;
}

.sidebar-nav {
  flex: 1;
  padding: 1rem 0;
}

.nav-list {
  list-style: none;
  margin: 0;
  border-right: 1px solid #e9ecef;
  padding: 0;
}

.nav-item {
  display: flex;
  align-items: center;
  padding: 0.75rem 1.5rem;
  cursor: pointer;
  transition: background-color 0.2s;
  position: relative;
}

.nav-item:hover {
  background-color: #f7fafc;
}

.nav-item.active {
  background-color: #edf2f7;
  border-right: 3px solid #667eea;
}

.sidebar.collapsed .nav-item {
  padding: 0.75rem;
  justify-content: center;
}

.sidebar.collapsed .nav-icon {
  margin-right: 0;
}

.nav-icon {
  margin-right: 0.75rem;
  font-size: 16px;
}

.nav-text {
  flex: 1;
  font-size: 14px;
  color: #4a5568;
}

.nav-badge {
  background-color: #e2e8f0;
  color: #718096;
  font-size: 12px;
  padding: 0.25rem 0.5rem;
  border-radius: 12px;
  min-width: 20px;
  text-align: center;
}

.sidebar-footer {
  padding: 1rem 1.5rem;
  border-top: 1px solid #e9ecef;
}

.user-profile {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.user-avatar {
  width: 40px;
  height: 40px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: rgb(255, 255, 255);
  font-weight: bold;
  font-family: 'Inter', 'Segoe UI', system-ui, sans-serif;
  font-size: large;
}

.user-info {
  flex: 1;
}

.user-name {
  font-size: 14px;
  font-weight: 500;
  color: #2d3748;
}

.user-role {
  font-size: 12px;
  color: #718096;
}

/* Main Content Styles */
.main-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  transition: margin-left 0.3s ease;
}

@media (max-width: 768px) {
  .main-content {
    margin-left: 0;
  }

  .dashboard-container:not(.sidebar-collapsed) .main-content {
    margin-left: 0;
  }
}

.content-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0rem 1rem;
  background-color: white;
  border-bottom: 1px solid #e9ecef;
  height: 90px;
}

@media (max-width: 768px) {
  .content-header {
    padding: 1rem;
    flex-direction: column;
    gap: 1rem;
  }

  .breadcrumb {
    font-size: 12px;
  }
}

.breadcrumb {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 14px;
  color: #718096;
}

.breadcrumb-current {
  color: #2d3748;
  font-weight: 500;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.settings-btn,
.notifications-btn,
.user-menu {
  width: 40px;
  height: 40px;
  background-color: #f7fafc;
  /* same as bg-gray-100 */
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.settings-btn:hover,
.notifications-btn:hover,
.user-menu:hover {
  background-color: #edf2f7;
}

.page-content {
  flex: 1;
  padding: 2rem;
  overflow-y: auto;
}

@media (max-width: 768px) {
  .page-content {
    padding: 1rem;
  }
}

.page-title-section {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

@media (max-width: 768px) {
  .page-title-section {
    flex-direction: column;
    gap: 1rem;
    align-items: flex-start;
  }

  .page-title {
    font-size: 1.5rem;
  }
}

.page-title {
  font-size: 2rem;
  font-weight: bold;
  color: #2d3748;
  margin: 0;
}

.add-document-btn {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 8px;
  font-weight: 500;
  cursor: pointer;
  transition: transform 0.2s;
}

.add-document-btn:hover {
  transform: translateY(-1px);
}

.view-controls {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

@media (max-width: 768px) {
  .view-controls {
    flex-direction: column;
    gap: 1rem;
    align-items: stretch;
  }

  .view-tabs {
    justify-content: center;
  }

  .filter-search {
    width: 100%;
  }
}

.view-tabs {
  display: flex;
  gap: 0.5rem;
}

.view-tab {
  padding: 0.5rem 1rem;
  border: 1px solid #e2e8f0;
  background-color: white;
  border-radius: 6px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s;
}

.view-tab:hover {
  border-color: #cbd5e0;
}

.view-tab.active {
  background-color: #667eea;
  color: white;
  border-color: #667eea;
}

.filter-search {
  padding: 0.5rem 1rem;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  font-size: 14px;
  width: 200px;
}

.documents-table-container {
  background-color: white;
  border-radius: 12px;
  border: 1px solid #e9ecef;
  overflow: hidden;
}

@media (max-width: 768px) {
  .documents-table-container {
    overflow-x: auto;
  }

  .documents-table {
    min-width: 600px;
  }

  .documents-table th,
  .documents-table td {
    padding: 0.5rem;
    font-size: 12px;
  }
}

.documents-table {
  width: 100%;
  border-collapse: collapse;
}

.documents-table th {
  padding: 1rem;
  text-align: left;
  font-weight: 500;
  color: #4a5568;
  border-bottom: 1px solid #e9ecef;
  background-color: #f8f9fa;
  font-size: 14px;
}

.documents-table td {
  padding: 1rem;
  border-bottom: 1px solid #f1f3f4;
}

.document-row:hover {
  background-color: #f8f9fa;
}

.document-name-cell {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.document-icon {
  font-size: 18px;
}

.document-name {
  font-weight: 500;
  color: #2d3748;
}

.document-size {
  color: #718096;
  font-size: 14px;
}

.user-avatars {
  display: flex;
  align-items: center;
  gap: -0.25rem;
}

.user-avatar-small {
  width: 32px;
  height: 32px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 12px;
  font-weight: bold;
  border: 2px solid white;
  margin-left: -0.25rem;
}

.user-avatar-small:first-child {
  margin-left: 0;
}

.extra-users {
  width: 32px;
  height: 32px;
  background-color: #e2e8f0;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #718096;
  font-size: 11px;
  font-weight: bold;
  border: 2px solid white;
  margin-left: -0.25rem;
}

.document-date {
  color: #718096;
  font-size: 14px;
}

.document-actions {
  display: flex;
  gap: 0.5rem;
}

.action-btn {
  padding: 0.5rem 1rem;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  font-size: 13px;
  cursor: pointer;
  transition: all 0.2s;
}

.action-btn.delete {
  color: #e53e3e;
  background-color: white;
}

.action-btn.delete:hover {
  background-color: #fed7d7;
  border-color: #feb2b2;
}

.action-btn.edit {
  color: #667eea;
  background-color: white;
}

.action-btn.edit:hover {
  background-color: #e6fffa;
  border-color: #b2f5ea;
}

.sort-icon {
  margin-left: 0.5rem;
  color: #cbd5e0;
}

.select-all,
.row-select {
  margin-right: 0.5rem;
}

/* Grid View Styles */
.documents-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
}

@media (max-width: 768px) {
  .documents-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
  }
}

.document-card {
  background-color: white;
  border: 1px solid #e9ecef;
  border-radius: 12px;
  padding: 1.5rem;
  transition: all 0.2s;
  cursor: pointer;
}

.document-card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  border-color: #667eea;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.card-select {
  margin: 0;
}

.card-menu {
  color: #cbd5e0;
  cursor: pointer;
  padding: 0.25rem;
}

.card-menu:hover {
  color: #718096;
}

.card-icon {
  font-size: 48px;
  text-align: center;
  margin-bottom: 1rem;
}

.card-content {
  text-align: center;
}

.card-title {
  font-size: 16px;
  font-weight: 500;
  color: #2d3748;
  margin: 0 0 0.5rem 0;
}

.card-size {
  color: #718096;
  font-size: 14px;
  margin: 0 0 0.75rem 0;
}

.card-users {
  margin-bottom: 0.75rem;
  display: flex;
  justify-content: center;
}

.card-date {
  color: #718096;
  font-size: 13px;
  margin: 0 0 1rem 0;
}

.card-actions {
  display: flex;
  gap: 0.5rem;
  justify-content: center;
}

/* List View Styles */
.documents-list {
  background-color: white;
  border-radius: 12px;
  border: 1px solid #e9ecef;
  overflow: hidden;
}

@media (max-width: 768px) {
  .document-list-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .list-item-content {
    width: 100%;
  }

  .list-users {
    margin-right: 0;
    width: 100%;
  }

  .list-actions {
    width: 100%;
    justify-content: flex-end;
  }
}

.document-list-item {
  display: flex;
  align-items: center;
  padding: 1rem 1.5rem;
  border-bottom: 1px solid #f1f3f4;
  transition: background-color 0.2s;
}

.document-list-item:hover {
  background-color: #f8f9fa;
}

.document-list-item:last-child {
  border-bottom: none;
}

.list-item-content {
  display: flex;
  align-items: center;
  flex: 1;
  gap: 1rem;
}

.list-select {
  margin: 0;
}

.list-icon {
  font-size: 24px;
}

.list-details {
  flex: 1;
}

.list-title {
  font-size: 16px;
  font-weight: 500;
  color: #2d3748;
  margin: 0 0 0.25rem 0;
}

.list-meta {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 13px;
  color: #718096;
}

.list-separator {
  color: #cbd5e0;
}

.list-users {
  margin-right: 2rem;
}

.list-actions {
  display: flex;
  gap: 0.5rem;
}
</style>
