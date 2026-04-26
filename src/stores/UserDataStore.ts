import { defineStore } from "pinia";
import { Book, BookFormDto } from "../types/Book";
import { ref } from "vue";
import {  addNewBookService, editBookService, readUserData } from "../services/UserDataService";

export const useUserDataStore = defineStore("userData", () => {
    const userData = ref<Book[]>([]);
    const loading = ref<boolean>(true);

    async function updateUserData() {
        userData.value = await readUserData();
        userData.value.sort((a, b) => b.id - a.id);
        loading.value = false;
    }


    async function addBook(dto: BookFormDto) {
        await addNewBookService(dto);
        await updateUserData();
    }

    async function editBook(dto: BookFormDto) {
        await editBookService(dto);
        await updateUserData();
    }

    return { userData, updateUserData, addBook, editBook, loading }

})