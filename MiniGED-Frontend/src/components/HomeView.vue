<template>
    <div class="w-5/6 mx-auto px-8 py-8 bg-white m-5 rounder-4xl shadow-xl ">
        <!-- <header class="text-center mb-12">
            <h1 class="text-3xl font-bold text-gray-800 mb-2">Bienvenue sur MiniGED</h1>
            <p class="text-gray-600">Votre système de gestion électronique de documents simple</p>
        </header> -->

        <section class="mb-12">
            <h2 class="text-2xl font-semibold mb-6">Actions Rapides</h2>
            <div class="flex flex-wrap gap-4">
                <button @click="createDocument" 
                    class="flex flex-col items-center p-6 bg-gray-50 border border-gray-200 rounded-lg min-w-[150px] transition-all duration-200 hover:bg-gray-100 hover:-translate-y-1 hover:shadow-md">
                    <i class="fas fa-file-alt text-4xl mb-2 text-blue-600"></i>
                    <span>Créer un Document</span>
                </button>
                <button @click="uploadFile" 
                    class="flex flex-col items-center p-6 bg-gray-50 border border-gray-200 rounded-lg min-w-[150px] transition-all duration-200 hover:bg-gray-100 hover:-translate-y-1 hover:shadow-md">
                    <i class="fas fa-upload text-4xl mb-2 text-blue-600"></i>
                    <span>Téléverser un Fichier</span>
                </button>
                <button @click="searchDocuments" 
                    class="flex flex-col items-center p-6 bg-gray-50 border border-gray-200 rounded-lg min-w-[150px] transition-all duration-200 hover:bg-gray-100 hover:-translate-y-1 hover:shadow-md">
                    <i class="fas fa-search text-4xl mb-2 text-blue-600"></i>
                    <span>Rechercher des Documents</span>
                </button>
            </div>
        </section>

        <section>
            <h2 class="text-2xl font-semibold mb-6">Liste de Démarrage</h2>
            <div class="flex flex-col gap-4 mb-8">
                <div 
                    v-for="(item, index) in checklistItems" 
                    :key="index" 
                    class="border rounded-lg overflow-hidden transition-all duration-300"
                    :class="{ 'border-green-400 bg-green-50/20': item.completed, 'border-gray-200': !item.completed }"
                >
                    <div class="flex items-center p-4 bg-gray-50 cursor-pointer">
                        <div class="flex items-center mr-4">
                            <i :class="[item.completed ? 'fas fa-check-circle text-green-500' : 'far fa-circle', 'text-2xl mr-2']"></i>
                            <span class="flex items-center justify-center w-7 h-7 rounded-full text-sm font-bold"
                                :class="{ 'bg-green-500 text-white': item.completed, 'bg-gray-200': !item.completed }">
                                {{ index + 1 }}
                            </span>
                        </div>
                        <h3 class="flex-grow m-0">{{ item.title }}</h3>
                        <button @click="toggleGuide(index)" class="text-blue-600 bg-transparent border-none cursor-pointer">
                            {{ item.showGuide ? 'Masquer le Guide' : 'Afficher le Guide' }}
                        </button>
                    </div>
                    
                    <div v-if="item.showGuide" class="p-4 bg-white border-t border-gray-200">
                        <p>{{ item.description }}</p>
                        <div class="my-4">
                            <div v-for="(step, stepIndex) in item.steps" :key="stepIndex" class="flex mb-2">
                                <span class="flex items-center justify-center w-6 h-6 rounded-full bg-blue-600 text-white text-xs mr-3 flex-shrink-0">
                                    {{ stepIndex + 1 }}
                                </span>
                                <p>{{ step }}</p>
                            </div>
                        </div>
                        <button 
                            v-if="!item.completed" 
                            @click="markCompleted(index)" 
                            class="bg-green-500 text-white border-none py-2 px-4 rounded cursor-pointer mt-4"
                        >
                            Marquer comme Terminé
                        </button>
                    </div>
                </div>
            </div>

            <div class="mt-8">
                <div class="h-2.5 bg-gray-100 rounded-full overflow-hidden mb-2">
                    <div class="h-full bg-blue-600 transition-all duration-300" :style="{ width: progressPercentage + '%' }"></div>
                </div>
                <span class="text-sm text-gray-500">{{ completedCount }} sur {{ checklistItems.length }} tâches terminées ({{ progressPercentage }}%)</span>
            </div>
        </section>
    </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();

const checklistItems = ref([
    {
        title: 'Créer votre premier document',
        completed: false,
        showGuide: false,
        description: 'Commencez à organiser votre contenu en créant votre premier document.',
        steps: [
            'Cliquez sur le bouton "Créer un Document" dans les actions rapides.',
            'Saisissez un titre et une description pour votre document.',
            'Choisissez une catégorie (facultatif).',
            'Cliquez sur "Créer" pour terminer.'
        ]
    },
    {
        title: 'Téléverser des fichiers vers un document',
        completed: false,
        showGuide: false,
        description: 'Ajoutez des fichiers à vos documents pour tout garder organisé.',
        steps: [
            'Ouvrez un document depuis votre liste de documents.',
            'Cliquez sur le bouton "Téléverser un Fichier".',
            'Sélectionnez des fichiers depuis votre ordinateur.',
            'Ajoutez des descriptions à vos fichiers (facultatif).'
        ]
    },
    {
        title: 'Installer l\'assistant de bureau',
        completed: false,
        showGuide: false,
        description: 'Tirez le meilleur parti de MiniGED avec notre application de bureau.',
        steps: [
            'Allez dans Paramètres > Application de Bureau.',
            'Cliquez sur "Télécharger" pour votre système d\'exploitation.',
            'Exécutez l\'installateur et suivez les instructions à l\'écran.',
            'Connectez-vous avec votre compte MiniGED.'
        ]
    },
    {
        title: 'Essayer la fonctionnalité de recherche',
        completed: false,
        showGuide: false,
        description: 'Trouvez rapidement vos documents et fichiers avec notre puissante recherche.',
        steps: [
            'Cliquez sur l\'icône de recherche dans la navigation supérieure.',
            'Tapez des mots-clés liés à votre document ou fichier.',
            'Utilisez des filtres pour affiner les résultats.',
            'Cliquez sur un résultat pour l\'ouvrir.'
        ]
    },
    {
        title: 'Utiliser les instantanés de documents',
        completed: false,
        showGuide: false,
        description: 'Créez des instantanés pour sauvegarder des versions de documents pour référence ultérieure.',
        steps: [
            'Ouvrez un document dont vous souhaitez faire un instantané.',
            'Cliquez sur l\'onglet "Instantanés".',
            'Cliquez sur "Créer un Instantané" et ajoutez une description.',
            'Accédez aux instantanés précédents depuis le même onglet.'
        ]
    }
]);

const completedCount = computed(() => {
    return checklistItems.value.filter(item => item.completed).length;
});

const progressPercentage = computed(() => {
    return Math.round((completedCount.value / checklistItems.value.length) * 100);
});

function toggleGuide(index) {
    checklistItems.value[index].showGuide = !checklistItems.value[index].showGuide;
}

function markCompleted(index) {
    checklistItems.value[index].completed = true;
}

function createDocument() {
    router.push('/documents/new');
}

function uploadFile() {
    router.push('/upload');
}

function searchDocuments() {
    router.push('/search');
}
</script>
