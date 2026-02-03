<template>
  <div class="documents-list-container">
    <!-- Header -->
    <header class="documents-header">
      <div class="breadcrumb">
        <span></span>
        <span>Accueil</span>
        <span>></span>
        <span>Gestionnaire de documents</span>
        <span>></span>
        <span class="breadcrumb-current">Mes documents</span>
      </div>
    </header>

    <!-- Page Content -->
    <div class="page-content">
      <div class="page-title-section">
        <h1 class="page-title">Mes Documents</h1>
        <button class="add-document-btn" @click="addDocument">
          <span>Ajouter un document</span>
          <span class="btn-icon">📤</span>
        </button>
        <AddDocumentModal
          :visible="isAddDocumentModalVisible"
          @close="closeAddDocumentModal"
          @added="loadDocuments"
        />
      </div>

      <!-- Document View Toggle -->
      <div class="view-controls">
        <div class="view-tabs">
          <button
            class="view-tab"
            :class="{ active: currentView === 'tabs' }"
            @click="setView('tabs')"
          >
            Vue par onglets
          </button>
          <button
            class="view-tab"
            :class="{ active: currentView === 'grid' }"
            @click="setView('grid')"
          >
            Vue en grille
          </button>
          <button
            class="view-tab"
            :class="{ active: currentView === 'list' }"
            @click="setView('list')"
          >
            Vue en liste
          </button>
        </div>
        <div class="relative max-w-sm">
          <input
            id="search-input"
            type="text"
            placeholder="Search..."
            class="w-64 pl-10 pr-4 py-2 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:outline-none"
            v-model="searchQuery"
          />
          <img
            src="/search-icon.svg"
            alt="Rechercher"
            class="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400"
          />
        </div>
      </div>

      <!-- Documents Views -->
      <!-- Table View (Vue par onglets) -->
      <div v-if="currentView === 'tabs'" class="documents-table-container">
        <div v-if="loading" class="loading">Chargement des documents...</div>
        <div v-else-if="error" class="error">{{ error }}</div>
        <table v-else class="documents-table">
          <thead>
            <tr>
              <th>
                <input type="checkbox" class="select-all" />
                Nom du document
                
              </th>
              <th>
                Description
                
              </th>
              <th>Accès utilisateur</th>
              <th>
                Dernière modification
               
              </th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="document in filteredDocuments"
              :key="document.id"
              class="document-row"
            >
              <td class="document-name-cell">
                <input type="checkbox" class="row-select" />
                <div class="document-icon">📄</div>
                <span class="document-name">{{ document.title }}</span>
              </td>
              <td class="document-description">
                <div class="description-content">
                  {{ document.description || "Aucune description" }}
                </div>
              </td>
              <td class="document-users">
                <div class="user-avatars">
                  <div class="user-avatar-small">
                    {{ authStore.user?.username[0].toUpperCase() }}
                  </div>
                </div>
              </td>
              <td class="document-date">
                {{ formatDate(document.uploadedAt) }}
              </td>
              <td class="document-actions">
                <button
                  class="action-btn view"
                  @click="viewDocument(document.id)"
                >
                  Voir
                </button>
                <button
                  class="action-btn edit"
                  @click="editDocument(document.id)"
                >
                  Modifier
                </button>
                <button
                  class="action-btn delete"
                  @click="confirmDeleteDocument(document.id)"
                >
                  Supprimer
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Grid View (Vue en grille) -->
      <div v-if="currentView === 'grid'" class="documents-grid">
        <div v-if="loading" class="loading">Chargement des documents...</div>
        <div v-else-if="error" class="error">{{ error }}</div>
        <div
          v-else
          v-for="document in filteredDocuments"
          :key="document.id"
          class="document-card"
        >
          <div class="card-header">
            <input type="checkbox" class="card-select" />
            <div class="card-menu">⋮</div>
          </div>
          <div class="card-icon">📄</div>
          <div class="card-content">
            <h3 class="card-title">{{ document.title }}</h3>
            <p class="card-description">
              {{ document.description || "Aucune description" }}
            </p>
            <div class="card-users">
              <div class="user-avatars">
                <div class="user-avatar-small">
                  {{ authStore.user?.username[0].toUpperCase() }}
                </div>
              </div>
            </div>
            <p class="card-date">{{ formatDate(document.uploadedAt) }}</p>
          </div>
          <div class="card-actions">
            <button class="action-btn view" @click="viewDocument(document.id)">
              Voir
            </button>
            <button class="action-btn edit" @click="editDocument(document.id)">
              Modifier
            </button>
            <button
              class="action-btn delete"
              @click="confirmDeleteDocument(document.id)"
            >
              Supprimer
            </button>
          </div>
        </div>
      </div>

      <!-- List View (Vue en liste) -->
      <div v-if="currentView === 'list'" class="documents-list">
        <div v-if="loading" class="loading">Chargement des documents...</div>
        <div v-else-if="error" class="error">{{ error }}</div>
        <div
          v-else
          v-for="document in filteredDocuments"
          :key="document.id"
          class="document-list-item"
        >
          <div class="list-item-content">
            <input type="checkbox" class="list-select" />
            <div class="list-icon">📄</div>
            <div class="list-details">
              <h3 class="list-title">{{ document.title }}</h3>
              <div class="list-meta">
                <span class="list-description">{{
                  document.description || "Aucune description"
                }}</span>
                <span class="list-separator">•</span>
                <span class="list-date">{{
                  formatDate(document.uploadedAt)
                }}</span>
              </div>
            </div>
          </div>
          <div class="list-users">
            <div class="user-avatars">
              <div class="user-avatar-small">
                {{ authStore.user?.username[0].toUpperCase() }}
              </div>
            </div>
          </div>
          <div class="list-actions">
            <button class="action-btn view" @click="viewDocument(document.id)">
              Voir
            </button>
            <button class="action-btn edit" @click="editDocument(document.id)">
              Modifier
            </button>
            <button
              class="action-btn delete"
              @click="confirmDeleteDocument(document.id)"
            >
              Supprimer
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal de confirmation de suppression -->
    <DeleteDocumentModal
      :visible="showDeleteModal"
      :deleting="deleting"
      @close="cancelDelete"
      @confirm="confirmDelete"
    />
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useRouter } from "vue-router";
import {
  getUserDocuments,
  deleteDocument as deleteDocumentService,
} from "../services/documentService";
import { useAuthStore } from "../stores/auth";
import AddDocumentModal from "./AddDocumentModal.vue";
import DeleteDocumentModal from "./DeleteDocumentModal.vue"; // Assurez-vous que ce chemin est correct
const isAddDocumentModalVisible = ref(false);
const showAddDocumentModal = () => {
  isAddDocumentModalVisible.value = true;
};
const closeAddDocumentModal = () => {
  isAddDocumentModalVisible.value = false;
};
const authStore = useAuthStore();
const router = useRouter();
const currentView = ref("tabs");
const documents = ref([]);
const loading = ref(false);
const error = ref(null);
const searchQuery = ref("");
const showDeleteModal = ref(false);
const documentToDelete = ref(null);
const deleting = ref(false);

const setView = (view) => {
  currentView.value = view;
};

const filteredDocuments = computed(() => {
  if (!searchQuery.value) {
    return documents.value;
  }
  return documents.value.filter(
    (doc) =>
      doc.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
      doc.description.toLowerCase().includes(searchQuery.value.toLowerCase())
  );
});

const loadDocuments = async () => {
  loading.value = true;
  error.value = null;
  try {
    const response = await getUserDocuments();
    documents.value = response;
  } catch (err) {
    error.value = "Erreur lors du chargement des documents";
    console.error("Erreur:", err);
  } finally {
    loading.value = false;
  }
};

const formatDate = (dateString) => {
  const date = new Date(dateString);
  return date.toLocaleDateString("fr-FR");
};

const addDocument = () => {
  // Logique pour ajouter un document
  console.log("Ajouter un document");
  showAddDocumentModal();
};

const editDocument = (id) => {
  // Logique pour modifier un document
  console.log("Modifier document:", id);
};

const confirmDeleteDocument = (id) => {
  documentToDelete.value = id;
  showDeleteModal.value = true;
};

const cancelDelete = () => {
  showDeleteModal.value = false;
  documentToDelete.value = null;
  deleting.value = false;
};

const confirmDelete = async () => {
  if (!documentToDelete.value) return;

  deleting.value = true;
  try {
    await deleteDocumentService(documentToDelete.value);
    // Retirer le document de la liste locale
    documents.value = documents.value.filter(
      (doc) => doc.id !== documentToDelete.value
    );
    showDeleteModal.value = false;
    documentToDelete.value = null;
  } catch (err) {
    error.value = "Erreur lors de la suppression du document";
    console.error("Erreur:", err);
  } finally {
    deleting.value = false;
  }
};

const viewDocument = (id) => {
  router.push(`/document/${id}`);
};

onMounted(() => {
  loadDocuments();
});
</script>

<style scoped>
.documents-list-container {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.documents-header {
  padding: 1rem 2rem;
  background-color: white;
  border-bottom: 1px solid #e9ecef;
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

.page-content {
  flex: 1;
  padding: 2rem;
  overflow-y: auto;
}

.page-title-section {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
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

.loading,
.error {
  text-align: center;
  padding: 2rem;
  color: #718096;
}

.error {
  color: #e53e3e;
}

/* Table View Styles */
.documents-table-container {
  background-color: white;
  border-radius: 12px;
  border: 1px solid #e9ecef;
  overflow: hidden;
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

.document-description {
  color: #718096;
  font-size: 14px;
  max-width: 200px;
}

.description-content {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
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
}

.document-date {
  color: #718096;
  font-size: 14px;
}

.document-actions {
  display: flex;
  align-items: center;
  justify-content: center;
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

.action-btn.view {
  color: #38a169;
  background-color: white;
}

.action-btn.view:hover {
  background-color: #f0fff4;
  border-color: #9ae6b4;
}

.sort-icon {
  margin-left: 0.5rem;
  color: #cbd5e0;
}

/* Grid View Styles */
.documents-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
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

.card-menu {
  color: #cbd5e0;
  cursor: pointer;
  padding: 0.25rem;
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

.card-description {
  color: #718096;
  font-size: 14px;
  margin: 0 0 0.75rem 0;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  max-height: 40px;
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

.list-description {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  max-width: 300px;
  display: inline-block;
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

@media (max-width: 768px) {
  .page-content {
    padding: 1rem;
  }

  .page-title-section {
    flex-direction: column;
    gap: 1rem;
    align-items: flex-start;
  }

  .view-controls {
    flex-direction: column;
    gap: 1rem;
  }

  .documents-grid {
    grid-template-columns: 1fr;
  }

  .documents-table-container {
    overflow-x: auto;
  }

  .documents-table {
    min-width: 600px;
  }

  .search-filter {
    width: 100%;
    margin-top: 1rem;
    display: flex;
    justify-content: flex-end;
    align-items: center;
    padding: 0.5rem;
    background-color: #f8f9fa;
    border-radius: 6px;
    border: 1px solid #e2e8f0;
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  }
}
</style>
