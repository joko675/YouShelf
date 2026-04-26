import { defineStore } from "pinia";
import { computed, ref } from "vue";
import { useUserDataStore } from "./UserDataStore";

export const useBookFilterStore = defineStore("BookFilter", () => {
    const selectedStatus = ref("READING");
    const userDataStore = useUserDataStore();

    const filteredBooks = computed(() => 
        userDataStore.userData.filter((book) => book.status === selectedStatus.value)
    )

    return { selectedStatus, filteredBooks };
});