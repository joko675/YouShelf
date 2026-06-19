<script setup lang="ts">
    import { ref } from 'vue';
    import { BookFormDto } from '../types/Book';
    import { open } from '@tauri-apps/plugin-dialog';
    import { SelectItem } from '@nuxt/ui';
    import { convertFileSrc } from '@tauri-apps/api/core';
    import { useUserDataStore } from '../stores/UserDataStore';
    import { router } from '../router';

    const props = defineProps(['bookId']);
    
    const userDataStore = useUserDataStore();

    const previewImgSrc = ref("/blankCover.png");

    const formData = ref<BookFormDto>({
        id: undefined,
        title: "",
        author: "",
        rating: "",
        coverImgPath: "",
        status: "READING",
        releaseYear: null
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
            formData.value.releaseYear = bookInfo.releaseYear;

            previewImgSrc.value = bookInfo.coverImagePathFixed;
        }
    }


    type FormErrors = { [K in keyof BookFormDto]: boolean };    
    const errors = ref<FormErrors>({
        title: false,
        author: false,
        rating: false,
        coverImgPath: false,
        status: false,
        releaseYear: false,
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
            errors.value.coverImgPath = false;
        } else {
            errors.value.coverImgPath = true;
        }
    }
    function deleteImage() {
      previewImgSrc.value = "/blankCover.png";
      formData.value.coverImgPath = "";
      errors.value.coverImgPath = false;
    }

    //Returns true if errors present
    function validateData(): boolean {
        let error = false;
        for (const field in errors.value) {
            const formDataField = formData.value[field as keyof BookFormDto];
            const date = new Date();

          
            if (field === "releaseYear") {
              if (formDataField === null) {}
              if (Number(formDataField) < 0 || Number(formDataField) > date.getFullYear()) {
                error = true;
                errors.value[field as keyof FormErrors] = true;
              } else {
                  errors.value[field as keyof FormErrors] = false;
              }
            }
            else if (field === "rating") {}
            else if(field === "coverImgPath" && formDataField === "" || formDataField === null) formData.value.coverImgPath = "/blankCover.png";
            else if (formDataField === "" || formDataField === null){
                error = true;
                errors.value[field as keyof FormErrors] = true;
            } else {
                errors.value[field as keyof FormErrors] = false;
            }
        }
        return error;
    }

    function inputChange(fieldName: string) {
        console.log("change");
        const formDataField = formData.value[fieldName as keyof BookFormDto]

        if (formDataField !== "") {
            errors.value[fieldName as keyof FormErrors] = false;
        }
    }

    async function onSubmit() {
        if (validateData()) {
            console.log("One or more errors");
            console.log(errors.value['releaseYear']);
            return 0;
        }
        
        if (props.bookId) {
            formData.value.id = props.bookId;
            await userDataStore.editBook(formData.value);
        } else {
            await userDataStore.addBook(formData.value);
        }
        
        router.push("/");
    }

    const handleDeleteBook = async () => {
      if (!props.bookId) return;
    
      await userDataStore.deleteBook(props.bookId);
      router.push('/');
    };

</script>
<template>
    <div id="main">
    <form @submit.prevent="onSubmit" class="grid grid-cols-1 gap-4" id="form">
        
        <div class="flex flex-col gap-2">
            <label>Tytuł książki:*</label>
            <UInput v-model="formData.title" placeholder="Tytuł książki" :color='!errors.title ? "neutral" : "error"' :highlight=true @change='inputChange("title")' />
        </div>
        <div class="flex flex-col gap-2">
            <label>Autor książki:*</label>
            <UInput v-model="formData.author" placeholder="Autor książki" :color='!errors.author ? "neutral" : "error"' :highlight=true @change='inputChange("author")' />
        </div>
        <div class="flex flex-col gap-2">
            <label>Ocena książki:</label>
            <UInput v-model="formData.rating" placeholder="Ocena książki" color="neutral" :highlight=true />
        </div>
        <div class="grid grid-cols-2 gap-2">
            <div class="flex flex-col gap-1">
                <label>Dodaj okładkę:*</label>
                <UButton @click='uploadImage()' label="Wybierz okładkę" color="secondary" />
                <UButton label="Usuń okładkę" color="error" @click='deleteImage()'></UButton>
            </div>
            <img :src="previewImgSrc" id="coverImg" class="col-span-1">
        </div>
        <div class="flex flex-col gap-2">
            <label>Status książki:</label>
            <USelect v-model="formData.status" :items="statusItems" />
        </div>
        <div class="flex flex-col gap-2">
            <label>Data wydania:</label>
            <UInput v-model="formData.releaseYear" type="number" placeholder="Rok wydania" :color='!errors.releaseYear ? "neutral" : "error"' :highlight=true @change='inputChange("releaseYear")' />
        </div>

        <div id="buttons">
            <UButton type="button" v-if="props.bookId" color="error" @click="handleDeleteBook">Usuń książkę</UButton>
            <UButton type="submit">{{ props.bookId ? "Zapisz zmiany" : "Dodaj książkę" }}</UButton>
        </div>
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
    #buttons {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 10px;
    }
</style>