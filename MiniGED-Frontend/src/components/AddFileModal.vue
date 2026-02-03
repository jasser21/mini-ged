<template>
  <!-- Modal Overlay -->
  <div class="modal-overlay">
    <!-- Modal Container -->
    <div class="bg-white rounded-xl shadow-lg max-w-lg w-full px-8 py-4 relative z-50">
      <!-- Close Button -->
      <div class="flex justify-between items-center mb-8">
        <h2 class="text-md font-semibold text-gray-800">Ajouter un ou plusieurs fichiers</h2>
        <button class="text-gray-400 hover:text-gray-600" @click="handleCancel" aria-label="Close">
          <svg class="w-6 h-6" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12"></path>
          </svg>
        </button>
      </div>
      <!-- Modal Content -->
      <!-- Upload Complete Files -->
      <div v-for="file in FilesUploaded" :key="file.name" class="mt-4">
        <div class="flex justify-between items-center p-4 bg-gray-50 rounded-lg border border-gray-200 hover:border-gray-300 transition-colors">
          <div class="flex items-center gap-3">
            <div class="flex-shrink-0 p-2 bg-blue-50 rounded-lg">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                stroke="currentColor" class="w-5 h-5 text-blue-600">
                <path stroke-linecap="round" stroke-linejoin="round"
                  d="M19.5 14.25v-2.625a3.375 3.375 0 0 0-3.375-3.375h-1.5A1.125 1.125 0 0 1 13.5 7.125v-1.5a3.375 3.375 0 0 0-3.375-3.375H8.25m0 12.75h7.5m-7.5 3H12M10.5 2.25H5.625c-.621 0-1.125.504-1.125 1.125v17.25c0 .621.504 1.125 1.125 1.125h12.75c.621 0 1.125-.504 1.125-1.125V11.25a9 9 0 0 0-9-9Z" />
              </svg>
            </div>
            <div>
              <div class="font-medium text-gray-800 text-sm">{{ file.name }}</div>
              <div class="text-xs text-gray-500">
                {{ (file.size / 1024).toFixed(2) }} KB · {{ file.type || 'Unknown type' }}
              </div>
            </div>
          </div>
          <button class="p-1.5 text-gray-400 hover:text-red-600 rounded-full hover:bg-gray-100 transition-colors" 
            @click="FilesUploaded.splice(FilesUploaded.indexOf(file), 1)" 
            aria-label="Remove file">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
              stroke="currentColor" class="w-5 h-5">
              <path stroke-linecap="round" stroke-linejoin="round"
                d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
            </svg>
          </button>
        </div>
      </div>
      <!-- Upload Progress -->
      <div v-if="fileUploadProgress > 0 && fileUploadProgress < 100" class="mt-5">
        <div class="text-sm text-gray-600">
          Téléchargement du fichier en cours... {{ fileUploadProgress }}%
        </div>
        <div class="w-full bg-gray-200 rounded-full h-2 mt-1">
          <div class="bg-blue-600 h-2 rounded-full  transition-all duration-300 ease-in-out"
            :style="{ width: fileUploadProgress + '%' }"></div>
        </div>
      </div>
      <!-- Drop Zone -->
      <div
        class="mt-5 cursor-pointer p-12 flex justify-center bg-gray-50 border border-dashed border-gray-300 rounded-xl hover:border-blue-400 transition-colors"
        @click="triggerFileSelect">
        <input type="file" ref="fileInput" class="hidden" accept=".pdf,.doc,.docx,.txt" @change="handleFileUpload" />
        <div class="text-center">
          <!-- SVG omitted for brevity -->
            <div class="mt-4 flex flex-wrap justify-center text-sm text-gray-600">
            <span class="pe-1 font-medium text-gray-800" v-if="FilesUploaded.length === 0">
              Déposez votre fichier ici ou
            </span>
            <span class="pe-1 font-medium text-gray-800" v-else>
              Déposez un autre fichier ici ou
            </span>
            <span
              class="bg-white font-semibold text-blue-600 hover:text-blue-700 rounded-lg decoration-2 hover:underline cursor-pointer">
              Parcourir
            </span>
            </div>
          <p class="mt-1 text-xs text-gray-400">
            Choisissez un fichier de 2 Mo maximum.
          </p>
        </div>
      </div>
      <!-- Save & Cancel Buttons -->
      <div v-if="FilesUploaded.length > 0" class="flex justify-end gap-2 mt-8">
        <button class="px-4 py-2 rounded bg-gray-200 text-gray-700 hover:bg-gray-300" @click="handleCancel">
          Annuler
        </button>
        <button class="px-4 py-2 rounded bg-blue-600 text-white hover:bg-blue-700"
          :disabled="FilesUploaded.length === 0 || isSaving" @click="handleSave">
          <span v-if="isSaving">Enregistrement...</span>
          <span v-else>Enregistrer</span>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps } from 'vue';
import BaseApiService from '../services/ApiService';
const emit = defineEmits();
const props = defineProps(['documentId']);
const FilesUploaded = ref([]);
const file = ref(null);
const fileUploadProgress = ref(0);
const fileInput = ref(null);
const isSaving = ref(false);

function triggerFileSelect() {
  fileInput.value.click();
}

function handleFileUpload(event) {
  const selectedFile = event.target.files[0];
  fileUploadProgress.value = 0;

  if (!selectedFile) return;

  // Validate size (max 2MB)
  if (selectedFile.size > 2 * 1024 * 1024) {
    alert("File size must be under 2MB");
    return;
  }

  // Validate type
  const allowedTypes = [
    "application/pdf",
    "application/msword",
    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    "text/plain"
  ];
  if (!allowedTypes.includes(selectedFile.type)) {
    alert("Invalid file type");
    return;
  }

  file.value = selectedFile;

  // Simulate upload progress
  const interval = setInterval(() => {
    if (fileUploadProgress.value < 100) {
      fileUploadProgress.value += 10;
    } else {
      clearInterval(interval);
      
      // Add file to uploaded files array
      FilesUploaded.value.push(file.value);
      
      // Reset progress after a short delay
      setTimeout(() => {
        fileUploadProgress.value = 0;
      }, 2000);
    }
  }, 100);
}

function handleCancel() {
  FilesUploaded.value = [];
  file.value = null;
  fileUploadProgress.value = 0;
  // Optionally emit an event to close the modal from parent
  emit('close');
}
async function handleSave() {
  if (!FilesUploaded.value?.length) {
    alert('Please select at least one file');
    return;
  }

  isSaving.value = true;

  try {
    const formData = new FormData();

    // Append document ID
    formData.append('DocumentId', props.documentId);

    // Append all files to the Files array parameter
    FilesUploaded.value.forEach(file => {
      formData.append('Files', file);
    });

    const response = await BaseApiService('document/upload').create(formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      },
      onUploadProgress: progressEvent => {
        fileUploadProgress.value = Math.round(
          (progressEvent.loaded * 100) / progressEvent.total
        );
      }
    });

    if (response.data.success) {
      emit('added');
      emit('close');
    } else {
      alert(response.message || 'Erreur lors du téléchargement des fichiers');
      console.log(response.data);
    }
  } catch (error) {
    console.error('Upload error:', error);
    alert(error.response?.data?.message || 'Le téléchargement a échoué. Veuillez réessayer.');
  } finally {
    isSaving.value = false;
  }
}
</script>
<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.4);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}
</style>