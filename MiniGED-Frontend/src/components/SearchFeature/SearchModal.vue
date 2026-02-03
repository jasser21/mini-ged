<template>
  <div class="relative">
    <!-- Trigger -->
    
    <!-- Overlay -->
    <div  class="fixed inset-0 z-50 bg-black/40 backdrop-blur-sm flex items-center justify-start flex-col p-10 overflow-auto"
         @click.self="CloseModal">
      <!-- <div class="absolute inset-0 z-50" @click="CloseModal"></div> -->

      <div class="bg-white z-10 rounded-2xl shadow-xl w-full max-w-xl p-6 focus:border-blue-500 focus:ring-2 focus:ring-blue-100">
        <!-- Search Input -->
        <input 
          v-model="query" 
          class="w-full border rounded-lg p-3 focus:outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
          placeholder="Search..." 
        />

        <!-- Results -->
        
        <!-- Close button -->
        <button @click="CloseModal" class="absolute top-4 right-6 text-red-500 hover:text-red-700 focus:outline-none focus:ring-2 focus:ring-red-500">✕</button>
      </div>
      
        <SearchResultItem
          v-if="query"
          v-for="item in filteredResults"
          @click="directToDocumentView(item)"
          :key="item.id"
          :result="item"
          :highlight="query"
          class="p-2 m-2 w-200 z-10 hover:bg-gray-100  rounded opacity-100 transition duration-200"
        />
      
    </div>
  </div>
</template>

<script setup>
import { ref, computed, defineEmits, reactive,watch } from 'vue'
import BaseApiService from '../../services/ApiService';
import SearchResultItem from './SearchResultItem.vue';
import { useRouter } from 'vue-router';
const router = useRouter();
const emits = defineEmits(['close']);

const CloseModal = () => {
  emits('close');
}

// const isOpen = ref(false)
const query = ref('')

const data = reactive([]);

const loadData = async () => {
  try {
    // Simulate fetching data
    // Replace this with your actual data fetching logic
    const response = await BaseApiService(`/document/search/${query.value}`).create(query.value);
    if(!response.data.success){
      console.error('Error fetching data:', response.data.message);
    }
    else{
      console.log('Data fetched successfully:', response.data.results.hits);
      data.length = 0; // Clear existing data
      data.push(...response.data.results.hits);
    }
    //const result = await response.json();
    console.log(response);
    //data.push(...result);
  } catch (error) {
    console.error('Error fetching data:', error);
  }
}


// Watch for changes in the query and load data accordingly with delay when user stops writing
watch(query, (newQuery) => {
  if (newQuery) {
    loadData();
  }
});

const filteredResults = computed(() =>
  data.filter(item => item )
)
const directToDocumentView = (result) =>{
  console.log("Navigating to document view for:", result);
  emits('close');
  // Logic to navigate to the document view
  router.push({
    path: `/document/${result.documentIdIndexed}`,
    query: {
      FileId: result.fileIdIndexed,
      page: result.pageNumber,
      highlight: query.value
    }
  });
}
// const filteredResults = computed(() =>
//   data.filter(item => item.name.toLowerCase().includes(query.value.toLowerCase()))
// )
</script>
