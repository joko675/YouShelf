<script setup lang="ts">
import BookForm from '../components/BookForm.vue';

</script>
<template>
    <div v-if="$route.params.id">
        <BookForm :book-id=$route.params.id />
        {{ console.log($route.params.id) }}
    </div>
    <div v-else>
        <BookForm />
    </div>
   
    
</template>