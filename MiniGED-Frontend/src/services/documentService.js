import apiClient from "./api";

export async function getUserDocuments() {
  try {
    const response = await apiClient.get("/document");
    return response.data;
  } catch (error) {
    console.error("Erreur lors du chargement des documents :", error);
    throw error;
  }
}

export async function deleteDocument(documentId) {
  try {
    const response = await apiClient.delete(`/document/${documentId}`);
    return response.data;
  } catch (error) {
    console.error("Erreur lors de la suppression du document :", error);
    throw error;
  }
}

export async function getDocumentById(documentId) {
  try {
    const response = await apiClient.get(`/document/${documentId}`);
    return response.data;
  } catch (error) {
    console.error("Erreur lors du chargement du document :", error);
    throw error;
  }
}
