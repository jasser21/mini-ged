<template>
    <main class="container border-gray-200 border-2 rounded-lg p-4">
        <div class="tool-bar">
            <img src="../assets/images/Logo1.png" class="w-8 h-8" alt="">
            <span class="page-info">
                <span id="page-num"></span> <span id="page-count"></span>
            </span>
            <div class="middle-bar">
                <div class="text-blue-950 font-semibold rounded-md px-2 py-1 text-sm">
                    {{ Math.round(scale * 100) }}%
                </div>
                <button @click="scale = scale < 2 ? scale + 0.25 : scale"
                    class="btn btn-link text-primary px-2 mb-0 mx-2">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                        stroke="currentColor" class="size-6">
                        <path stroke-linecap="round" stroke-linejoin="round"
                            d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607ZM10.5 7.5v6m3-3h-6" />
                    </svg>

                </button>
                <button @click="scale = scale > 0.25 ? scale - 0.25 : scale"
                    class="btn btn-link text-primary px-2 mb-0 mx-2">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                        stroke="currentColor" class="size-6">
                        <path stroke-linecap="round" stroke-linejoin="round"
                            d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607ZM13.5 10.5h-6" />
                    </svg>

                </button>
                <button @click="scale = 1.88" class="btn btn-link text-primary px-2 mb-0 mx-2">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                        stroke="currentColor" class="size-6">
                        <path stroke-linecap="round" stroke-linejoin="round"
                            d="M3.75 3.75v4.5m0-4.5h4.5m-4.5 0L9 9M3.75 20.25v-4.5m0 4.5h4.5m-4.5 0L9 15M20.25 3.75h-4.5m4.5 0v4.5m0-4.5L15 9m5.25 11.25h-4.5m4.5 0v-4.5m0 4.5L15 15" />
                    </svg>

                </button>
                <button @click="scale = 1" class="btn btn-link text-primary px-2 mb-0 mx-2">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                        stroke="currentColor" class="size-6">
                        <path stroke-linecap="round" stroke-linejoin="round"
                            d="M9 9V4.5M9 9H4.5M9 9 3.75 3.75M9 15v4.5M9 15H4.5M9 15l-5.25 5.25M15 9h4.5M15 9V4.5M15 9l5.25-5.25M15 15h4.5M15 15v4.5m0-4.5 5.25 5.25" />
                    </svg>

                </button>
            </div>

            <div>
                <button @click="downloadDocument" class="btn flex btn-link text-secondary px-1 mb-0 mx-2">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5"
                        stroke="currentColor" class="size-6">
                        <path stroke-linecap="round" stroke-linejoin="round"
                            d="M9 8.25H7.5a2.25 2.25 0 0 0-2.25 2.25v9a2.25 2.25 0 0 0 2.25 2.25h9a2.25 2.25 0 0 0 2.25-2.25v-9a2.25 2.25 0 0 0-2.25-2.25H15M9 12l3 3m0 0 3-3m-3 3V2.25" />
                    </svg>
                    Download
                </button>
            </div>
        </div>
        <div class="file-container" id="file-container">
            <div v-if="selected == null">
                <h1> No available Pdfs </h1>
            </div>
            <!-- <div v-else v-for="(page,index) in pages" :key="index"> -->
            <div v-for="(page, index) in pages" :id="`page-${index}`" :key="index+1">
                <PageViewComponent :page="page"  :scale="scale" :pdf="pdf" :src="src"
                    :fit-parent="fitParent" @refresh-annotation="refresh"
                    @page-loaded="onPageRendered"
                    :highlight-text="highlightText"
                />
                <div class="text-gray-600 text-sm font-medium mt-2 mb-1 text-center">
                    page {{ index + 1 }} of {{ pages }}
                </div>
            </div>
        </div>

        <!-- share a document modal -->
        <!-- <div class="modal fade dark" id="exampleModalMessage" tabindex="-1" role="dialog"
      aria-labelledby="exampleModalMessageTitle" aria-hidden="true" data-bs-backdrop="false">
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">
              Share This Document
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">×</span>
            </button>
          </div>
          <div class="modal-body">
            <form>
              <div class="form-group">
                <label for="recipient-name" class="col-form-label">Share With</label>
                <input type="text" class="form-control" placeholder="Write a valid Username" id="recipient-name"
                  v-model="username" />
              </div>
              <div class="form-group">
                <label for="message-text" class="col-form-label">Message:</label>
                <textarea class="form-control" id="message-text"></textarea>
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn bg-gradient-secondary" data-bs-dismiss="modal">
              Close
            </button>
            <button @click="sharedocument" type="button" class="btn bg-gradient-primary">
              Share
            </button>
          </div>
        </div>
      </div>
    </div> -->
    </main>
</template>

<script setup>
import PageViewComponent from "./PageViewComponent.vue";

import { ref, computed, onMounted } from "vue";
import { usePDF } from "@tato30/vue-pdf";
const src = ref("http://localhost:5092/api/document/pdf/8016");
// const showModal = ref(false);
const props = defineProps({
    selected: Number,
    pageNumber: Number,
    highlightText: String
});
const { pdf, pages } = usePDF(src);
import { watch } from 'vue';
onMounted(async () =>  {
    console.log(props.selected);

    if (props.selected) {
        src.value = `http://localhost:5092/api/document/pdf/${props.selected}`;
        console.log('inmoutn', src.value);
    }
    else {
        // Handle the case when props.selected is an invalid index
        console.log("Invalid index")
    }
    // if (props.pageNumber) {
    //     console.log(props.pageNumber);
    //     setTimeout(() => scrollToPage(props.pageNumber), 500);
    // }
    //console.log(info);

}
);

watch(() => props.selected, (newValue) => {
    src.value = `http://localhost:5092/api/document/pdf/${newValue}`;
}
);
const refresh = () => {
    // force a component rerender
    src.value = `http://localhost:5092/api/document/pdf/${props.selected}`;
};

const scrollToPage = (pageNumber) => {
  const pageNumber1 = pageNumber - 1;
  const container = document.getElementById("file-container")
  const el = document.getElementById(`page-${pageNumber1}`)
  if (container && el) {
    // Use scrollIntoView for robust scrolling
    el.scrollIntoView({ behavior: "smooth", block: "start" })
  }
}

function onPageRendered(page) {
  // if a target page is defined
  if (props.pageNumber && page === props.pageNumber) {
    console.log("Page rendered -> scroll to:", page)
    scrollToPage(page)
  } else {
    console.log("Page rendered (no scroll):", page)
  }
}


const scale = ref(1);
const fitParent = ref(false);
const pageNumber = ref(props.pageNumber || 1);
// const SignatureToolbar = ref(false);
//   const showShareModal = (index) => {
//     showModal.value = true;

//   };
// const sharedocument = async () => {
//   try {
//     const ShareRequest = {
//       username: username.value,
//       documentId: documentId.value,
//     };

//     const response =
//       await BaseApiService(`Document/Share`).create(ShareRequest);

//     router.push("/shared-documents");
//   } catch (e) {
//     console.error(e);
//   }
// };
// const showSignatureToolbar = () => {
//   SignatureToolbar.value = !SignatureToolbar.value;
// }; -->
const emit = defineEmits(["close-button"])

const closeTab = () => {
    emit('close-button')
}
const downloadDocument = async () => {
    try {
        const response = await BaseApiService("Document/Download").get(
            documentId.value,
            {
                responseType: "blob",
            },
        );
        const url = window.URL.createObjectURL(new Blob([response.data]));
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", "document.pdf");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    } catch (error) {
        console.error("Error downloading document:", error);
    }
};


</script>

<style scoped>
.container {
    width: 100%;
    display: flex;
    flex-direction: column;
    height: 70vh;
    margin: 0;
    padding: 0;
    background-color: #f8f9fa;
    overflow: hidden;
}

.tool-bar {
    background: #ffffff;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.08);
    color: #495057;
    padding: 0.75rem 1.5rem;
    width: 100%;
    height: 64px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 1px solid #e9ecef;
}

.middle-bar {
    display: flex;
    justify-content: center;
    align-items: center;

    border-radius: 8px;
    padding: 0.25rem;
}

.btn {
    transition: all 0.2s ease;

}

.btn:hover {
    background-color: #f1f3f5;
    padding: 0.5rem 1rem;
    opacity: 0.85;
    transform: translateY(-1px);
}

.btn:active {
    transform: translateY(0);
}

.page-info {
    font-weight: 500;
    color: #495057;
    font-size: 0.9rem;
}

.file-container {
    background: #e9ecef;
    width: 100%;
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: center;
    overflow: auto;
    height: calc(100vh - 64px);
    padding: 1.5rem 0;
}

.file-container::-webkit-scrollbar {
    width: 8px;
    background-color: #f8f9fa;
}

.file-container::-webkit-scrollbar-thumb {
    background-color: #adb5bd;
    border-radius: 4px;
}

.file-container::-webkit-scrollbar-thumb:hover {
    background-color: #6c757d;
}

.file-container>div {
    margin-bottom: 1.5rem;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    border-radius: 4px;
    background-color: #fff;


}

.text-primary {
    color: #0d6efd !important;
}

.text-secondary {
    color: #6c757d !important;
}

/* Empty state styling */
.file-container h1 {
    color: #6c757d;
    font-weight: 400;
    font-size: 1.5rem;
    margin-top: 2rem;
}
</style>
