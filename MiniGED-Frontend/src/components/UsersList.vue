<template>
    <div class="p-6">
        <h2 class="text-2xl font-bold mb-6 text-gray-800">Users Management</h2>
        
        <!-- Search and add new user controls -->
        <div class="flex justify-between mb-6">
            <div class="relative w-64">
                <input 
                    type="text" 
                    v-model="searchQuery" 
                    placeholder="Search users..." 
                    @input="filterUsers"
                    class="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                >
                <button class="absolute right-3 top-2 text-gray-500">
                    <i class="fas fa-search"></i>
                </button>
            </div>
            <!-- <button 
                @click="openAddUserModal"
                class="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition duration-200"
            >
                Add New User
            </button> -->
        </div>

        <!-- Users table -->
        <div class="overflow-x-auto bg-white rounded-lg shadow">
            <table v-if="filteredUsers.length > 0" class="min-w-full divide-y divide-gray-200">
                <thead class="bg-gray-50">
                    <tr>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Username</th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Full Name</th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Email</th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Role</th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Phone Number</th>
                        <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Actions</th>
                    </tr>
                </thead>
                <tbody class="bg-white divide-y divide-gray-200">
                    <tr v-for="user in filteredUsers" :key="user.id" class="hover:bg-gray-50">
                        <td class="px-6 py-4 whitespace-nowrap">{{ user.username }}</td>
                        <td class="px-6 py-4 whitespace-nowrap">{{ user.fullName }}</td>
                        <td class="px-6 py-4 whitespace-nowrap">{{ user.email }}</td>
                        <td class="px-6 py-4 whitespace-nowrap">{{ user.role }}</td>
                        <td class="px-6 py-4 whitespace-nowrap">{{ user.phoneNumber }}</td>
                        <td class="px-6 py-4 whitespace-nowrap space-x-2">
                            <button @click="OpenUserDetails(user)" class="text-blue-600 hover:text-blue-900">
                                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-5">
  <path stroke-linecap="round" stroke-linejoin="round" d="M2.036 12.322a1.012 1.012 0 0 1 0-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178Z" />
  <path stroke-linecap="round" stroke-linejoin="round" d="M15 12a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z" />
</svg>
                            </button>
                            <button @click="editUser(user)" class="text-amber-600 hover:text-amber-900">
                            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-5">
  <path stroke-linecap="round" stroke-linejoin="round" d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10" />
</svg>
                            </button>
                            <button @click="confirmDelete(user)" class="text-red-600 hover:text-red-900">
                               <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-5">
  <path stroke-linecap="round" stroke-linejoin="round" d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
</svg>
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div v-else class="py-10 text-center text-gray-500">No users found</div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import BaseApiService from '../services/ApiService';
import { useRoute,useRouter } from 'vue-router';
const route = useRoute();
const router = useRouter();
// Sample data - replace with your actual data source
const users = ref([
    { id: 1, username: 'johndoe', fullName: 'John Doe', email: 'john@example.com', role: 'Admin', phoneNumber: '555-1234' },
]);
const searchQuery = ref('');
const filteredUsers = ref([]);

onMounted(() => {
    // Initialize filtered users with all users
    loadUsers();
    filteredUsers.value = [...users.value];
});

const loadUsers = () =>{
    BaseApiService('/user').list().then(response => {
        users.value = response.data;
        filteredUsers.value = [...users.value];
    }).catch(error => {
        console.error('Error fetching users:', error);
    });
}

const filterUsers = () => {
    // Filter users based on search query
    const query = searchQuery.value.toLowerCase();
    filteredUsers.value = users.value.filter(user => 
        user.username.toLowerCase().includes(query) ||
        user.fullName.toLowerCase().includes(query) ||
        user.email.toLowerCase().includes(query) ||
        user.role.toLowerCase().includes(query)
    );
};

const openAddUserModal = () => {
    // Implement modal for adding a new user
    console.log('Open add user modal');
};

const OpenUserDetails = (user) => {
    // Implement user details viewing functionality
    console.log('Open user details:', user);
    router.push(`user/${user.id}`);
};

const editUser = (user) => {
    // Implement user editing functionality
    console.log('Edit user:', user);
};

const confirmDelete = (user) => {
    // Implement user deletion confirmation
    console.log('Confirm delete for user:', user);
};
</script>
