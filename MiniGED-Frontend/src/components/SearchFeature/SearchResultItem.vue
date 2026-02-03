<template>
    <div class="border border-gray-200 rounded-lg cursor-pointer p-4 mb-4 bg-white shadow-sm hover:shadow-md transition-shadow" @click="directTodocumentView">
        <!-- Document Header -->
        <div class="flex justify-between items-start mb-3">
            <div class="flex-1">
                <div class="flex items-center gap-2 mb-1">
                    <!-- <svg class="w-4 h-4 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"/>
                    </svg> -->
                    <svg class="w-4 h-4 text-blue-600" xmlns="http://www.w3.org/2000/svg" fill="none"
                        viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round"
                            d="M3.75 9.776c.112-.017.227-.026.344-.026h15.812c.117 0 .232.009.344.026m-16.5 0a2.25 2.25 0 0 0-1.883 2.542l.857 6a2.25 2.25 0 0 0 2.227 1.932H19.05a2.25 2.25 0 0 0 2.227-1.932l.857-6a2.25 2.25 0 0 0-1.883-2.542m-16.5 0V6A2.25 2.25 0 0 1 6 3.75h3.879a1.5 1.5 0 0 1 1.06.44l2.122 2.12a1.5 1.5 0 0 0 1.06.44H18A2.25 2.25 0 0 1 20.25 9v.776" />
                    </svg>

                    <h3 class="m-0 text-lg text-gray-800 font-medium">{{ result.documentName }}</h3>
                </div>
                <!-- <div v-if="result.description" class="text-sm text-gray-600 italic mb-2">
                    {{ result.description.trim() }}
                </div> -->
            </div>
            <span class="text-xs text-gray-400 bg-gray-50 px-2 py-1 rounded">
                Doc ID: {{ result.documentIdIndexed }}
            </span>
        </div>

        <!-- File Information -->
        <div class="bg-gray-50 rounded-md p-3 mb-3">
            <div class="flex items-center justify-between mb-2">
                <div class="flex items-center gap-2">
                    
                    <svg class="w-4 h-4 text-gray-600" xmlns="http://www.w3.org/2000/svg" fill="none"
                        viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round"
                            d="M19.5 14.25v-2.625a3.375 3.375 0 0 0-3.375-3.375h-1.5A1.125 1.125 0 0 1 13.5 7.125v-1.5a3.375 3.375 0 0 0-3.375-3.375H8.25m0 12.75h7.5m-7.5 3H12M10.5 2.25H5.625c-.621 0-1.125.504-1.125 1.125v17.25c0 .621.504 1.125 1.125 1.125h12.75c.621 0 1.125-.504 1.125-1.125V11.25a9 9 0 0 0-9-9Z" />
                    </svg>

                    <span class="text-sm font-medium text-gray-700">{{ result.fileName }}</span>
                    <span v-if="result.pageNumber" class="text-xs text-white bg-blue-400 px-2 py-1 rounded border">
                        Page {{ result.pageNumber }}
                    </span>
                </div>
                <span class="text-xs text-gray-500">
                    File ID: {{ result.fileIdIndexed }}
                </span>
            </div>

            <!-- <div class="text-xs text-gray-500">
                <span>Uploaded: {{ formatDate(result.uploadedAt) }}</span>
                <span class="mx-2">•</span>
                <span>Created: {{ formatDate(result.createdDate) }}</span>
            </div> -->
        </div>

        <!-- Page Content -->
        <div class="border-l-4 border-blue-200 pl-4">
            <!-- <div class="flex items-center gap-2 mb-2">
                <svg class="w-4 h-4 text-blue-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                        d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                </svg>
                <span class="text-sm font-medium text-blue-700">Page Content</span>
            </div> -->
            <div class="text-sm leading-relaxed text-gray-700 whitespace-pre-line bg-blue-50 p-3 rounded">
                <span v-html="truncateContent(result.pageContent)"></span>
                <span v-if="isContentTruncated(result.pageContent)"
                    class="text-blue-600 cursor-pointer hover:underline ml-1">
                    ...read more
                </span>
            </div>
        </div>

        <!-- Breadcrumb-style hierarchy indicator -->
        <div class="mt-3 pt-2 border-t border-gray-100">
            <div class="flex items-center text-xs text-gray-500 gap-1">
                <span class="font-medium">{{ result.documentName }}</span>
                <svg class="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
                </svg>
                <span>{{ result.fileName }}</span>
                <svg v-if="result.pageNumber" class="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
                </svg>
                <span v-if="result.pageNumber">Page {{ result.pageNumber }}</span>
            </div>
        </div>
    </div>
</template>

<script setup>
import { defineProps } from 'vue'
import { useRouter } from 'vue-router';
const router = useRouter();

const props = defineProps({
    result: {
        type: Object,
        required: true
    },
    highlight: {
        type: String,
        required: true
    }
})

const formatDate = (dateString) => {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString();
}

const truncateContent = (content, maxLength = 300) => {
    if (!content) return '';
    if (content.length <= maxLength) return content;
    return content.substring(0, maxLength) + '...';
}

const isContentTruncated = (content, maxLength = 300) => {
    return content && content.length > maxLength;
}

const directToDocumentView = () => {

    console.log("clicked ");
    // Logic to navigate to the document view
    router.push({
       path: `/document/${props.result.documentIdIndexed}`, // or use path
        query: {
            FileId: props.result.fileIdIndexed,
            page: props.result.pageNumber,
            highlight: props.highlight
        }
    })
}


</script>