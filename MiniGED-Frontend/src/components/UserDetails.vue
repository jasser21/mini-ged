<template>
    <div class="w-3/4 m-10 p-6 bg-white rounded-lg shadow-lg">
        <div class="flex justify-between items-center mb-6">
            <button 
            @click="backToUsersList" 
            class="flex items-center gap-2 px-4 py-2 bg-gray-200 text-gray-700 rounded-md hover:bg-gray-300 transition duration-200"
            >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6">
              <path stroke-linecap="round" stroke-linejoin="round" d="M9 15 3 9m0 0 6-6M3 9h12a6 6 0 0 1 0 12h-3" />
            </svg>
            <span>Retour à la liste des utilisateurs</span>
            </button>
        </div>

        <div v-if="loading" class="flex justify-center py-8">
            <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-purple-500"></div>
        </div>

        <div v-else-if="user" class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <!-- User Profile Column -->
            <div class="col-span-1 bg-gray-50 p-4 rounded-lg">
                <div class="flex flex-col items-center mb-4">
                    <div v-if="user.profilePicture" class="w-32 h-32 rounded-full overflow-hidden mb-3">
                        <img :src="user.profilePicture" alt="Profile picture" class="w-full h-full object-cover">
                    </div>
                    <div v-else class="w-32 h-32 rounded-full flex items-center justify-center mb-3 text-white text-6xl font-bold"
                         style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);">
                        {{ getUserInitials(user.fullName) }}
                    </div>
                    <h3 class="text-xl font-semibold">{{ user.fullName }}</h3>
                    <p class="text-gray-600">{{ user.role || 'Utilisateur' }}</p>
                    <p class="text-sm text-indigo-600 mt-2">{{ user.email }}</p>
                </div>
                
                <div class="border-t pt-4 mt-2">
                    <div class="flex items-center mb-2">
                        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-indigo-500 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z" />
                        </svg>
                        <span>{{ user.phoneNumber }}</span>
                    </div>
                    <div class="flex items-center mb-2">
                        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-indigo-500 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                        </svg>
                        <span>Inscrit le {{ formatDate(user.createdDate) }}</span>
                    </div>
                    <div class="flex items-center">
                        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-indigo-500 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 21h10a2 2 0 002-2V9.414a1 1 0 00-.293-.707l-5.414-5.414A1 1 0 0012.586 3H7a2 2 0 00-2 2v14a2 2 0 002 2z" />
                        </svg>
                        <span>{{ user.fileUploadCount }} fichiers téléversés</span>
                    </div>
                </div>
            </div>

            <!-- User Documents -->
            <div class="col-span-2">
                <div class="mb-6">
                    <h3 class="text-lg font-semibold mb-3 text-gray-800">Documents de l'utilisateur</h3>
                    <div class="bg-gray-50 p-4 rounded-lg">
                        <div v-if="user.userdocuments && user.userdocuments.length > 0">
                            <div v-for="(document, index) in user.userdocuments" :key="index" class="mb-3 pb-3 border-b border-gray-200 last:border-0">
                                <div class="flex items-start">
                                    <div class="flex-shrink-0 mt-1">
                                        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-indigo-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                                        </svg>
                                    </div>
                                    <div class="ml-3">
                                        <p class="text-sm font-medium">{{ document.title }}</p>
                                        <p class="text-xs text-gray-500">Téléversé le {{ formatDate(document.uploadedAt) }}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <p v-else class="text-gray-500">Aucun document téléversé</p>
                    </div>
                </div>
            </div>
        </div>

        <div v-else class="py-10 text-center text-gray-500">
            Utilisateur introuvable ou erreur lors du chargement des données
        </div>
    </div>
</template>


<script setup>
import { ref, onMounted } from 'vue';
import BaseApiService from '../services/ApiService';
import { useRoute,useRouter } from 'vue-router';
const router = useRouter();
const route = useRoute();
const props = defineProps({
    userId: {
        type: [Number, String],
        required: true
    }
});

const emit = defineEmits(['close']);

const user = ref(null);
const loading = ref(true);

onMounted(() => {
    loadUserDetails();
});

const loadUserDetails = async () => {
    loading.value = true;
    try {
        const response = await BaseApiService('/user').get(route.params.id);
        user.value = response.data;
    } catch (error) {
        console.error('Erreur lors de la récupération des détails utilisateur:', error);
    } finally {
        loading.value = false;
    }
};

const getUserInitials = (fullName) => {
    if (!fullName) return '';
    return fullName
        .split(' ')
        .map(name => name[0]?.toUpperCase() || '')
        .join('')
        .substring(0, 2);
};

const formatDate = (dateString) => {
    if (!dateString) return 'N/A';
    
    const date = new Date(dateString);
    
    // Check if date is valid
    if (isNaN(date.getTime())) {
        return 'Date invalide';
    }
    
    return new Intl.DateTimeFormat('fr-FR', { 
        year: 'numeric', 
        month: 'short', 
        day: 'numeric' 
    }).format(date);
};
const backToUsersList = () => {
    router.back();
}
</script>

