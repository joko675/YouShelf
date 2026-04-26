<script setup lang="ts">
    import { ref } from 'vue';
    import { BookFormDto } from '../types/Book';
    import { open } from '@tauri-apps/plugin-dialog';
    import { SelectItem } from '@nuxt/ui';
    import { convertFileSrc } from '@tauri-apps/api/core';
    import { useUserDataStore } from '../stores/UserDataStore';
    import { router } from '../router';
    import { CalendarDate } from '@internationalized/date';

    const props = defineProps(['bookId']);
    
    const userDataStore = useUserDataStore();

    const previewImgSrc = ref();
    const releaseDateUnformatted = ref();
    const formData = ref<BookFormDto>({
        id: undefined,
        title: "",
        author: "",
        rating: "",
        coverImgPath: "",
        status: "READING",
        releaseDate: ""
    });

    //If we are editing already existing book read its info and populate form with it
    if (props.bookId) {
        const bookInfo = userDataStore.userData.find((book) => book.id === Number(props.bookId));

        if (bookInfo){
            formData.value.id = bookInfo.id;
            formData.value.title = bookInfo.title;
            formData.value.author = bookInfo.author;
            formData.value.coverImgPath = bookInfo.coverImagePathFixed;
            formData.value.rating = bookInfo.rating;
            formData.value.status = bookInfo.status;
            formData.value.releaseDate = bookInfo.releaseDate;

            previewImgSrc.value = bookInfo.coverImagePathFixed;

            const [year, month, day] = bookInfo.releaseDate.split("-").map(Number);
            
            releaseDateUnformatted.value = new CalendarDate(year, month, day);
        }
    }
        
    const errors = ref<BookFormDto>({
        title: "",
        author: "",
        coverImgPath: "",
        status: "",
        releaseDate: ""
    });
    
    const statusItems = ref<SelectItem[]>([
        { label: "Czytana", value: "READING" },
        { label: "Przeczytana", value: "READ" },
        { label: "Planowana", value: "PLANNING" },
        { label: "Porzucona", value: "CANCELLED" }
    ]);

    async function uploadImage() {
        const file = await open({
            multiple: false,
            directory: false,
            filters: [{
                name: "Image",
                extensions: ["jpg", "png", "jpeg", "webp"]
            }]
        })
        if (file) {
            previewImgSrc.value = convertFileSrc(file);
            formData.value.coverImgPath = file;
        }
    }

    //Returns true if errors present
    function validateData(): boolean {
        let error = false;
        for (const field in errors) {
            if (field === "" || field === null) error = true;
        }
        return error;
    }
    
    function convertDateToString() {
        const stringDate = releaseDateUnformatted.value.toString();
        formData.value.releaseDate = stringDate;
    }

    async function onSubmit() {
        if (validateData()) {
            console.log("One or more errors");
            return 0;
        }
        convertDateToString();

        if (props.bookId) {
            formData.value.id = props.bookId;
            await userDataStore.editBook(formData.value);
        } else {
            await userDataStore.addBook(formData.value);
        }
        
        router.push("/");
    }

</script>
<template>
    <div id="main">
    <div class="goBack"><UButton @click='() => router.push("/")' label="Wróć" color="error"></UButton></div>
    <form v-on:submit.prevent="onSubmit" class="grid grid-cols-1 gap-4" id="form">
        
        <div class="flex flex-col gap-2">
            <label>Tytuł książki:*</label>
            <UInput v-model="formData.title" placeholder="Tytuł książki" :color='!errors.title ? "neutral" : "error"' :highlight=true />
        </div>
        <div class="flex flex-col gap-2">
            <label>Autor książki:*</label>
            <UInput v-model="formData.author" placeholder="Autor książki" :color='!errors.author ? "neutral" : "error"' :highlight=true />
        </div>
        <div class="flex flex-col gap-2">
            <label>Ocena książki:</label>
            <UInput v-model="formData.rating" placeholder="Ocena książki" color="neutral" :highlight=true />
        </div>
        <div class="grid grid-cols-2 gap-2">
            <div class="flex flex-col gap-1">
                <label>Dodaj okładkę:*</label>
                <UButton @click="uploadImage" label="Wybierz okładkę" :color='!errors.coverImgPath ? "secondary" : "error"' />
            </div>
            <img :src="previewImgSrc" id="coverImg" class="col-span-1">
        </div>
        <div class="flex flex-col gap-2">
            <label>Status książki:</label>
            <USelect v-model="formData.status" :items="statusItems" />
        </div>
        <div class="flex flex-col gap-2">
            <label>Data wydania:*</label>
            <UInputDate v-model="releaseDateUnformatted" />
        </div>
            
        <UButton type="submit">{{ props.bookId ? "Zapisz zmiany" : "Dodaj książkę" }}</UButton>
    </form>
    </div>
</template>

<style scoped>
    #coverImg {
        border-radius: 10px;
        max-height: 300px;
        margin-left: auto;
        margin-right: auto;
    }
    .goBack {
        margin-bottom: 10px;
    }
</style>