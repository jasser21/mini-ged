<template>
  <div class="document-details-container">
    <!-- Header -->
    <header class="documents-header">
      <div class="breadcrumb">
        <span class="breadcrumb-link" @click="navigateToHome"></span>
        <span class="breadcrumb-link" @click="navigateToHome">Accueil</span>
        <span>></span>
        <span class="breadcrumb-link" @click="navigateToDocuments">Gestionnaire de documents</span>
        <span>></span>
        <span class="breadcrumb-link" @click="navigateToDocuments">Mes documents</span>
        <span>></span>
        <span class="breadcrumb-current">{{
          document.title || "Document"
        }}</span>
      </div>
    </header>

    <!-- Document Content -->
    <div class="document-content">
      <div v-if="loading" class="loading">
        <div class="spinner"></div>
        Chargement du document...
      </div>

      <div v-else-if="error" class="error">
        {{ error }}
      </div>

      <div v-else class="document-layout" :class="{
        'with-panel': selectedFile && showPreviewPanel,
        'without-panel': !selectedFile || !showPreviewPanel,
      }">
        <!-- Left Panel - Document Details -->
        <div class="left-panel">
          <!-- Search Bar -->
          <div class="search-section">
            <div class="search-input-container">
              <span class="search-icon">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"
                  stroke-width="1.5" stroke="currentColor" class="size-6">
                  <path stroke-linecap="round" stroke-linejoin="round"
                    d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
                </svg>
              </span>
              <input type="text" placeholder="Search files and folders..." class="search-files-input"
                v-model="searchQuery" />
            </div>
          </div>

          <!-- Navigation Tabs -->
          <div class="nav-tabs">
            <button class="nav-tab" :class="{ active: activeTab === 'details' }" @click="activeTab = 'details'">
              Détails
            </button>
            <button class="nav-tab" :class="{ active: activeTab === 'files' }" @click="activeTab = 'files'">
              Files
            </button>
            <button class="nav-tab" :class="{ active: activeTab === 'actions' }" @click="activeTab = 'actions'">
              Actions
            </button>

          </div>

          <!-- Document Details Section -->
          <div v-if="activeTab === 'details'" class="details-section">
            <h3 class="section-title">Document Details</h3>

            <div class="detail-field">
              <label class="detail-label">Titre</label>
              <input type="text" :value="document.title" class="detail-input" readonly />
            </div>

            <div class="detail-field">
              <label class="detail-label">Description</label>
              <textarea :value="document.description || 'Aucune description'" class="detail-textarea" readonly
                rows="4"></textarea>
            </div>

            <div class="detail-field">
              <label class="detail-label">Date de création</label>
              <input type="text" :value="formatDate(document.uploadedAt)" class="detail-input" readonly />
            </div>
          </div>
          <!-- Files Section -->
          <div v-if="activeTab === 'files'" class="files-section">
            <button class="my-2 action-btn primary" @click="AddFiles">
              <span class="btn-icon">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                  stroke="currentColor" class="size-6">
                  <path stroke-linecap="round" stroke-linejoin="round"
                    d="M12 9v6m3-3H9m12 0a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z" />
                </svg>

              </span>
              Ajouter des fichiers
            </button>
            <h3 class="mx-1 section-title">Fichiers ({{ filteredFiles.length }})</h3>

            <!--
                 another button for adding files here
            -->


            <div v-if="filteredFiles.length === 0" class="flex flex-col items-center justify-center p-8 text-gray-500 text-center rounded-lg border border-dashed border-gray-300 bg-gray-50 mb-4">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-12 h-12 mb-3 text-gray-400">
              <path stroke-linecap="round" stroke-linejoin="round" d="M19.5 14.25v-2.625a3.375 3.375 0 0 0-3.375-3.375h-1.5A1.125 1.125 0 0 1 13.5 7.125v-1.5a3.375 3.375 0 0 0-3.375-3.375H8.25m6.75 12H9m1.5-12H5.625c-.621 0-1.125.504-1.125 1.125v17.25c0 .621.504 1.125 1.125 1.125h12.75c.621 0 1.125-.504 1.125-1.125V11.25a9 9 0 0 0-9-9Z" />
              </svg>
              <p class="font-medium">Aucun fichier trouvé.</p>
              <p class="text-sm mt-1">Ajoutez des fichiers ou modifiez votre recherche.</p>
            </div>


            <div class="files-list">
              <div v-for="file in filteredFiles" :key="file.id" class="file-item" @click="selectFile(file)">
                <div class="file-icon">
                  {{ getFileIcon(file.fileName) }}
                </div>
                <div class="file-info">
                  <div class="file-name">{{ file.fileName }}</div>
                  <div class="file-meta">
                    {{ getFileType(file.fileName) }} •
                    {{ formatFileSize(file.fileSize) }}
                  </div>
                </div>
                <div class="file-actions">
                    <button class="file-action-btn download-btn" @click.stop="downloadFile(file)" title="Télécharger">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-4 h-4">
                      <path stroke-linecap="round" stroke-linejoin="round" d="M3 16.5v2.25A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75V16.5M16.5 12 12 16.5m0 0L7.5 12m4.5 4.5V3" />
                    </svg>
                    </button>
                    <button class="file-action-btn delete-btn" @click.stop="deleteFile(file)" title="Supprimer">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="w-4 h-4">
                      <path fill-rule="evenodd" d="M16.5 4.478v.227a48.816 48.816 0 0 1 3.878.512.75.75 0 1 1-.256 1.478l-.209-.035-1.005 13.07a3 3 0 0 1-2.991 2.77H8.084a3 3 0 0 1-2.991-2.77L4.087 6.66l-.209.035a.75.75 0 0 1-.256-1.478A48.567 48.567 0 0 1 7.5 4.705v-.227c0-1.564 1.213-2.9 2.816-2.951a52.662 52.662 0 0 1 3.369 0c1.603.051 2.815 1.387 2.815 2.951Zm-6.136-1.452a51.196 51.196 0 0 1 3.273 0C14.39 3.05 15 3.684 15 4.478v.113a49.488 49.488 0 0 0-6 0v-.113c0-.794.609-1.428 1.364-1.452Zm-.355 5.945a.75.75 0 1 0-1.5.058l.347 9a.75.75 0 1 0 1.499-.058l-.346-9Zm5.48.058a.75.75 0 1 0-1.498-.058l-.347 9a.75.75 0 0 0 1.5.058l.345-9Z" clip-rule="evenodd" />
                    </svg>
                    </button>
                </div>
              </div>
            </div>
          </div>

          <!-- Actions Section -->
          <div v-if="activeTab === 'actions'" class="actions-section">
            <h3 class="section-title">Actions</h3>

            <div class="action-buttons">
              <button class="action-btn primary" @click="AddFiles">
                <span class="btn-icon">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                    stroke="currentColor" class="size-6">
                    <path stroke-linecap="round" stroke-linejoin="round"
                      d="M12 9v6m3-3H9m12 0a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z" />
                  </svg>

                </span>
                Ajouter des fichiers
              </button>

              <button class="action-btn secondary">
                <span class="btn-icon">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                    stroke="currentColor" class="size-6">
                    <path stroke-linecap="round" stroke-linejoin="round"
                      d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10" />
                  </svg>

                </span>
                Modifier le document
              </button>

              <button class="action-btn danger" @click="deleteDocument">
                <span class="btn-icon">
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                    stroke="currentColor" class="size-5">
                    <path stroke-linecap="round" stroke-linejoin="round"
                      d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
                  </svg>
                </span>

                Supprimer le document
              </button>
            </div>
            <!-- Add File Modal -->

          </div>
        </div>
        <!-- Right Panel - Document Preview -->
        <div v-if="selectedFile && showPreviewPanel" class="right-panel" :class="{ closing: isClosing }">
          <div class="document-preview">
            <div class="preview-header">
              <div class="breadcrumb-path">
                <span>Documents Personnels</span>
                <span>/</span>
                <span class="current-file">{{ selectedFile.fileName }}</span>
              </div>
              <button class="close-panel-btn" @click="togglePreviewPanel" title="Fermer l'aperçu">
                <span class="close-icon">✕</span>
              </button>
            </div>

            <div class="preview-container">
              <!-- <iframe src="http://localhost:5092/api/document/pdf/0" frameborder="0"></iframe> -->
              <FileView  :selected="selectedFile.id" :pageNumber="RoutePageNumber" :highlight-text="highlightText" />

              <!-- <div class="preview-actions">
                <button class="preview-btn">
                  <span class="btn-icon">🔍</span>
                  Aperçu
                </button>
                <button class="preview-btn">
                  <span class="btn-icon">⬇️</span>
                  Télécharger
                </button>
                <button class="preview-btn">
                  <span class="btn-icon">🔗</span>
                  Partager
                </button>
                  </div> -->
            </div>
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
  <AddFileModal v-if="showAddFileModal" :documentId="document.id" :visible="showAddFileModal" @close="closeAddFileModal"
    @added="FilesAdded" />
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import AddFileModal from "./AddFileModal.vue";
import { useAuthStore } from "../stores/auth";
import {
  getDocumentById,
  deleteDocument as deleteDocumentService,
} from "../services/documentService";
import DeleteDocumentModal from "./DeleteDocumentModal.vue";
import { toast } from "vue3-toastify";
import 'vue3-toastify/dist/index.css';

const showAddFileModal = ref(false);

const closeAddFileModal = () => {
  showAddFileModal.value = false;
};

const route = useRoute();
const router = useRouter();
const authStore = useAuthStore();

const document = ref({});
const loading = ref(false);
const error = ref(null);
const activeTab = ref("details");
const searchQuery = ref("");
const selectedFile = ref(null);
const showPreviewPanel = ref(route.query.FileId ? true : false);
const highlightText = ref(route.query.highlight || "");
const isClosing = ref(false);
const showDeleteModal = ref(false);
const deleting = ref(false);
const RoutePageNumber = ref(parseInt(route.query.page, 10) || 1);
import { watch } from "vue";
import BaseApiService from "../services/ApiService";
import FileView from "./FileView.vue";

watch(selectedFile, (newVal, oldVal) => {
  console.log("Selected file changed:", newVal);
  console.log(document.value);
});

const filteredFiles = computed(() => {
  if (!document.value.files) return [];

  if (!searchQuery.value) {
    return document.value.files;
  }

  return document.value.files.filter((file) =>
    file.fileName.toLowerCase().includes(searchQuery.value.toLowerCase())
  );
});

const loadDocument = async () => {
  loading.value = true;
  error.value = null;

  try {
    const documentId = route.params.id;
    const response = await getDocumentById(documentId);
    document.value = response;
    const FileIdRoute = parseInt(route.query.FileId, 10);
    RoutePageNumber.value = parseInt(route.query.page, 10) || 1;
    console.log("FileIdRoute:", FileIdRoute);
    console.log("showpreviewPanel:", showPreviewPanel.value);
    console.log("documentFiles:", document.value.files);
    console.log("RoutePageNumber:", RoutePageNumber.value);
    console.log("highlightText:", highlightText.value);
    console.log("route:", route.query.page);
    if (FileIdRoute) {
      selectedFile.value = document.value.files.find(file => file.id === FileIdRoute) || null;
      console.log("selectedFile:", selectedFile.value);
    }
  } catch (err) {
    error.value = "Erreur lors du chargement du document";
    console.error("Erreur:", err);
  } finally {
    loading.value = false;
  }
};

const formatDate = (dateString) => {
  const date = new Date(dateString);
  return date.toLocaleDateString("fr-FR", {
    year: "numeric",
    month: "long",
    day: "numeric",
  });
};

const formatFileSize = (bytes) => {
  if (bytes === 0) return "0 Bytes";
  const k = 1024;
  const sizes = ["Bytes", "KB", "MB", "GB"];
  const i = Math.floor(Math.log(bytes) / Math.log(k));
  return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i];
};

const getFileIcon = (fileName) => {
  const extension = fileName.split(".").pop().toLowerCase();

  switch (extension) {
    case "pdf":
      return "📄";
    case "doc":
    case "docx":
      return "📝";
    case "xls":
    case "xlsx":
      return "📊";
    case "ppt":
    case "pptx":
      return "📽️";
    case "jpg":
    case "jpeg":
    case "png":
    case "gif":
      return "🖼️";
    case "mp4":
    case "avi":
    case "mov":
      return "🎥";
    case "mp3":
    case "wav":
      return "🎵";
    case "zip":
    case "rar":
      return "📦";
    default:
      return "📄";
  }
};

const getFileType = (fileName) => {
  const extension = fileName.split(".").pop().toLowerCase();

  switch (extension) {
    case "pdf":
      return "PDF Document";
    case "doc":
    case "docx":
      return "Word Document";
    case "xls":
    case "xlsx":
      return "Excel Spreadsheet";
    case "ppt":
    case "pptx":
      return "PowerPoint Presentation";
    case "jpg":
    case "jpeg":
      return "JPEG Image";
    case "png":
      return "PNG Image";
    case "gif":
      return "GIF Image";
    case "mp4":
      return "MP4 Video";
    case "mp3":
      return "MP3 Audio";
    case "zip":
      return "ZIP Archive";
    default:
      return extension.toUpperCase() + " File";
  }
};

const selectFile = (file) => {
  selectedFile.value = file;
  showPreviewPanel.value = true;
};

const togglePreviewPanel = () => {
  if (showPreviewPanel.value) {
    isClosing.value = true;
    setTimeout(() => {
      showPreviewPanel.value = false;
      isClosing.value = false;
    }, 400);
  } else {
    showPreviewPanel.value = true;
  }
};

const AddFiles = () => {
  // Logic to add files
  showAddFileModal.value = true;

  console.log("Ajouter des fichiers");
};
const FilesAdded = () =>{
  console.log("Fichiers ajoutés");
  loadDocument();
  activeTab.value = 'files';
}
const downloadFile = (file) => {
  console.log("Télécharger le fichier:", file.fileName);
  // Logique pour télécharger le fichier
};

const deleteFile = async (file) => {
  console.log("Supprimer le fichier:", file.fileName);

  // Supprimer le fichier avec son Id qui est le nom sans extension
  const fileId = file.id;
  console.log("Id du fichier à supprimer:", fileId);  
  const response = await BaseApiService(`document/delete`).remove(fileId);
  if (response.data.success) {
    toast.success("Fichier supprimé avec succès",
      { autoclose: 3000 }
    );
    loadDocument();
    // Recharger le document pour mettre à jour la liste des fichiers
  } else {
    toast.error(response.data.message || "Erreur lors de la suppression du fichier",
    { autoclose: 3000 }
  );
  }
  
};

const deleteDocument = () => {
  showDeleteModal.value = true;
};

const cancelDelete = () => {
  showDeleteModal.value = false;
  deleting.value = false;
};

const confirmDelete = async () => {
  if (!document.value.id) return;

  deleting.value = true;
  try {
    await deleteDocumentService(document.value.id);
    // Rediriger vers la liste des documents après suppression
    router.push("/documents");
  } catch (err) {
    error.value = "Erreur lors de la suppression du document";
    console.error("Erreur:", err);
    deleting.value = false;
  }
};

const navigateToHome = () => {
  router.push("");
};

const navigateToDocuments = () => {
  router.push("/documents");
};

onMounted(() => {
  loadDocument();
  activeTab.value = 'files';
});


</script>

<style scoped>
.document-details-container {
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

.breadcrumb-link {
  cursor: pointer;
  transition: color 0.2s;
}

.breadcrumb-link:hover {
  color: #667eea;
  text-decoration: underline;
}

/* Document Content */
.document-content {
  flex: 1;
  padding: 2rem;
  overflow: hidden;
}

.loading {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100%;
  color: #718096;
  gap: 1rem;
}

.spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #f3f3f3;
  border-top: 4px solid #667eea;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }

  100% {
    transform: rotate(360deg);
  }
}

.error {
  text-align: center;
  color: #e53e3e;
  padding: 2rem;
}

.document-layout {
  display: grid;
  gap: 2rem;
  height: 100%;
  transition: grid-template-columns 0.4s cubic-bezier(0.4, 0, 0.2, 1);
}

.document-layout.with-panel {
  grid-template-columns: repeat(2, minmax(0, 1fr));
}

.document-layout.without-panel {
  grid-template-columns: 1fr;
}

.left-panel {
  grid-column: 1;
  background-color: white;
  border-radius: 12px;
  border: 1px solid #e9ecef;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.search-section {
  padding: 1.5rem;
  border-bottom: 1px solid #e9ecef;
}

.search-input-container {
  position: relative;
  display: flex;
  align-items: center;
}

.search-icon {
  position: absolute;
  left: 12px;
  color: #718096;
  font-size: 14px;
}

.search-files-input {
  flex: 1;
  padding: 0.75rem 2.5rem;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 14px;
  background-color: #f7fafc;
}

.nav-tabs {
  display: flex;
  align-items: center;
  justify-content: flex-start;
  border-bottom: 1px solid #e9ecef;
  gap: 0.5rem;
}

.nav-tab {
  padding: 1rem 1.5rem;
  border: none;
  background: none;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s;
  border-bottom: 2px solid transparent;
  flex: 0 0 auto;
}

.nav-tab:hover {
  background-color: #f7fafc;
}

.nav-tab.active {
  color: #667eea;
  border-bottom-color: #667eea;
  background-color: #f7fafc;
}

/* Details Section */
.details-section,
.files-section,
.actions-section {
  flex: 1;
  padding: 1.5rem;
  overflow-y: auto;
}

.section-title {
  font-size: 16px;
  font-weight: 400;
  color: #2d3748;
  margin: 0 0 0.5rem 0;
}

.detail-field {
  margin-bottom: 1.5rem;
}

.detail-label {
  display: block;
  font-size: 14px;
  font-weight: 500;
  color: #4a5568;
  margin-bottom: 0.5rem;
}

.detail-input,
.detail-textarea {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  font-size: 14px;
  background-color: #f7fafc;
}

.detail-textarea {
  resize: vertical;
  min-height: 80px;
}

/* Files Section */
.files-list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.file-item {
  display: flex;
  align-items: center;
  padding: 1rem;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.2s;
}

.file-item:hover {
  border-color: #667eea;
  background-color: #f7fafc;
}

.file-icon {
  font-size: 24px;
  margin-right: 0.75rem;
}

.file-info {
  flex: 1;
}

.file-name {
  font-size: 14px;
  font-weight: 500;
  color: #2d3748;
  margin-bottom: 0.25rem;
}

.file-meta {
  font-size: 12px;
  color: #718096;
}

.file-actions {
  display: flex;
  gap: 0.5rem;
}

.file-action-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 27px;
  height: 27px;
  border: none;
  background-color: #f7fafc;
  border-radius: 6px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.file-action-btn:hover {
  background-color: #edf2f7;
  color: #4a5568;
  border: 1px solid #2d3748;
}
.delete-btn:hover {
  background-color: #fed7d7;
  color: #e53e3e;
  border: 1px solid #feb2b2;
}
.download-btn:hover{
  background-color: #e6f0ff;
  color: #2563eb;
  border: 1px solid #93c5fd;
}

/* Actions Section */
.action-buttons {
  display: flex;

  gap: 1rem;
}

.action-btn {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  max-width: 300px;
  padding: 0.75rem 1rem;
  border: none;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
  flex: 0 0 auto;
}

.action-btn.primary {
  border-radius: 25px;
  color: white;

  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.action-btn.secondary {
  background-color: #f7fafc;
  color: #4a5568;
  border-radius: 25px;
  border: 1px solid #e2e8f0;
}

.action-btn.danger {
  border-radius: 25px;
  background-color: #fed7d7;
  color: #e53e3e;
  border: 1px solid #feb2b2;
}

.action-btn:hover {
  transform: translateY(-1px);
}

/* Right Panel */
.right-panel {
  background-color: white;
  border-radius: 12px;
  border: 1px solid #e9ecef;
  display: flex;
  flex-direction: column;
  transform: translateX(0);
  opacity: 1;
  transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
  animation: slideInRight 0.4s cubic-bezier(0.4, 0, 0.2, 1);
}

.right-panel.closing {
  transform: translateX(100%);
  opacity: 0;
}

@keyframes slideInRight {
  from {
    transform: translateX(100%);
    opacity: 0;
  }

  to {
    transform: translateX(0);
    opacity: 1;
  }
}

.document-preview {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.preview-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.5rem 1.5rem 0 1.5rem;
  margin-bottom: 1.5rem;
}

.breadcrumb-path {
  font-size: 14px;
  color: #718096;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  flex: 1;
}

.close-panel-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 32px;
  height: 32px;
  border: none;
  background-color: #f7fafc;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
  color: #718096;
  font-size: 14px;
}

.close-panel-btn:hover {
  background-color: #e2e8f0;
  color: #4a5568;
  transform: scale(1.05);
}

.close-panel-btn:active {
  transform: scale(0.95);
}

.close-icon {
  font-weight: bold;
}

.current-file {
  color: #2d3748;
  font-weight: 500;
}

.preview-container {
  flex: 1;
  flex-direction: column;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #bfbfbf 0%, #f2f2f2 100%);
  border-radius: 12px;
  padding: 0 0 auto 0;
  margin: 0 1.5rem 1.5rem 1.5rem;
}



.preview-actions {
  display: flex;
  gap: 1rem;
  justify-content: center;
  padding: 0 1.5rem 1.5rem 1.5rem;
}

.preview-btn {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1rem;
  border: 1px solid #e2e8f0;
  background-color: white;
  border-radius: 8px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s;
}

.preview-btn:hover {
  border-color: #667eea;
  background-color: #f7fafc;
}

.btn-icon {
  font-size: 16px;
}

@media (max-width: 1200px) {
  .document-layout {
    grid-template-columns: 1fr;
    grid-template-rows: auto 1fr;
  }

  .left-panel {
    max-height: 400px;
  }
}

@media (max-width: 768px) {
  .document-content {
    padding: 1rem;
  }

  .preview-container {
    padding: 2rem;
  }
}
</style>
