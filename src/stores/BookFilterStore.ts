import { defineStore } from "pinia";
import { ref } from "vue";

export const useBookFilterStore = defineStore("BookFilter", () => {
    const selectedStatus = ref("READING");

    return { selectedStatus };
});