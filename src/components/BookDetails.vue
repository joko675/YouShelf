<script setup lang="ts">
    import { ref } from 'vue';
    import { useUserDataStore } from '../stores/UserDataStore';
    import { RadioGroupItem } from '@nuxt/ui';
    import { Book } from '../types/Book';
    
    const props = defineProps<{
        bookId: number,
    }>();

    
    const userDataStore = useUserDataStore();   
    const bookDetails: Book = userDataStore.userData.find((book) => book.id === Number(props.bookId))!;

    const filterMenuItems = ref<RadioGroupItem[]>([
        { label: "Czytane", value: "READING" },
        { label: "Przeczytane", value: "READ" },
        { label: "Skończone", value: "FINISHED" },
        { label: "Porzucone", value: "CANCELLED" },
    ]);

    if (!bookDetails?.status) bookDetails.status = "READING";
    
</script>

<template>
    
    <UCard>
        <div class="main flex flex-col gap-5 sm:flex-col lg:flex-row">
            <div class="lg:w-1/2 flex justify-center md:justify-center lg:justify-end">
                <img :src="bookDetails.coverImagePathFixed">
            </div>
            <div class="lg:w-1/2 flex flex-col">
                <div id="title">Tytuł: {{ bookDetails?.title }}</div>
                <div id="author">Autor: {{ bookDetails?.author }}</div>
                <template v-if="bookDetails.rating">
                    <div id="rating">Ocena: {{ bookDetails.rating }} </div>
                </template>
                <template v-if="bookDetails.releaseYear">
                    <div id="rating">Rok wydania: {{ bookDetails.releaseYear }} </div>
                </template>
                <URadioGroup :items="filterMenuItems" orientation="horizontal" variant="table" indicator="hidden" v-model="bookDetails.status" class="pt-10"></URadioGroup>
            </div>
        </div>
    </UCard>
    
</template>
<style lang="css" scoped>
    img {
        max-height: 400px;
        max-width: 100%;
        border-radius: 10px;
    }
</style>