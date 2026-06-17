import { defineStore } from "pinia";
import { Book, BookFormDto } from "../types/Book";
import { ref, watch } from "vue";
import {  addNewBookService, deleteBookService, editBookService, readUserData, writeUserData } from "../services/UserDataService";

export const useUserDataStore = defineStore("userData", () => {
    const userData = ref<Book[]>([]);
    const loading = ref<boolean>(true);

    watch(userData, async () => {
        console.log("writing json");
        await writeUserData();
    }, {deep: true});

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

    async function deleteBook(bookId: number) {
      deleteBookService(bookId);
      await updateUserData();
    }

    return { userData, updateUserData, addBook, editBook, loading, deleteBook }

})