<script setup lang="ts">
    import { router } from '../router';
    import BookList from '../components/BookList.vue';
    import BookFilter from '../components/BookFilter.vue';

</script>
<template>
    <div id="main">
        <UButton @click='() => router.push("bookForm")'>Add Book</UButton>
        <div class="flex justify-center items-center">
            <BookFilter />
        </div>
        <BookList />
    </div>
</template>
