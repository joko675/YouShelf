<script setup lang="ts">
    import BookDetails from '../components/BookDetails.vue';
    import { router } from '../router';
</script>
<template>
    <div id="main">
        <div class="goBack"><UButton @click='() => router.push("/")' label="Wróć" color="error"></UButton></div>
        <BookDetails :book-id="Number($route.params.id)"></BookDetails>
    </div>
</template>