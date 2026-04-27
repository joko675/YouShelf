<script setup lang="ts">
import BookForm from '../components/BookForm.vue';
import { router } from '../router';

</script>
<template>
    <div id="main">
        <div class="goBack"><UButton @click='() => router.push("/")' label="Wróć" color="error"></UButton></div>
        <div v-if="$route.params.id">
            <BookForm :book-id=$route.params.id />
            {{ console.log($route.params.id) }}
        </div>
        <div v-else>
            <BookForm />
        </div>
    </div>
   
    
</template>