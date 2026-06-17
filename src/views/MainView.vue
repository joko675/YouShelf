<script setup lang="ts">
    import { router } from '../router';
    import BookList from '../components/BookList.vue';
    import BookFilter from '../components/BookFilter.vue';
    import { useColorMode } from '@nuxt/ui/runtime/vue/stubs/base.js';

    const colorMode = useColorMode();

    function toogle() {
      colorMode.preference = colorMode.value === "dark" ? "light" : "dark"
    }
</script>
<template>
    <div id="main">
        <div id="buttons">
            <UButton @click='() => router.push("bookForm")' class="m-1">Dodaj książkę</UButton>
            <UButton @click='toogle'>Zmień motyw</UButton>
        </div>
        <div class="flex justify-center items-center">
            <BookFilter />
        </div>
        <BookList />
    </div>
</template>
<style scoped>
    #buttons {
        padding-bottom: 10px;
    }
</style>