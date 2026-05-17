<script setup lang="ts">
import { pl } from "@nuxt/ui/runtime/locale/index.js";
import { useUserDataStore } from "./stores/UserDataStore";
import { onMounted } from "vue";
const userDataStore = useUserDataStore();

onMounted(async () => {
    await userDataStore.updateUserData();
});
</script>

<template>
    <div v-if="userDataStore.loading">Loading...</div>
    <div v-else>
        <UApp :locale="pl">
            <RouterView></RouterView>
        </UApp>
    </div>
</template>

<style scoped></style>
