<script setup lang="ts">
    import { onMounted } from 'vue';
    import { router } from '../router';
    import { useUserDataStore } from '../stores/UserDataStore';
    import BookList from '../components/BookList.vue';

    const userDataStore = useUserDataStore();

    
    onMounted(async() => {
        console.log(userDataStore.userData);
    })
    

</script>
<template>
    <div id="main">
        <UButton @click='() => router.push("bookForm")'>Add Book</UButton>
        <BookList />
    </div>
</template>
