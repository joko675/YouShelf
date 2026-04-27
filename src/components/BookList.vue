<script setup lang="ts">
    import { router } from '../router';
    import { useBookFilterStore } from '../stores/BookFilterStore';

    const bookFilterStore = useBookFilterStore();
    
</script>
<template>
    <div id="bookList">
        <div class="grid grid-cols-1 sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-2 xl:grid-cols-3 gap-4">
            <UCard v-for="book in bookFilterStore.filteredBooks">
                <div class="book">
                    <div class="coverImage"><img :src="book.coverImagePathFixed"></div>
                    <div class="title">{{ book.title }}</div>
                    <UButton label="Edit" color="neutral" @Click='() => router.push("/bookForm/"+book.id)'></UButton>
                </div>
            </UCard>
        </div>
    </div>

</template>
<style lang="css" scoped>
    .book {
        display: flex;
        flex-direction: column;
        align-items: center;
    }
    .coverImage img {
        border-radius: 10px;
        max-height: 250px;
    }
    .title {
        padding-top: 10px;
    }
    #bookList {
        padding-top: 10px;
    }
</style>