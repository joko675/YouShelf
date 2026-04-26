<script setup lang="ts">
    import { RadioGroupItem } from '@nuxt/ui';
    import { ref } from 'vue';
    import { useBookFilterStore } from '../stores/BookFilterStore';

    const bookFilterStore = useBookFilterStore();
    const filterMenuItems = ref<RadioGroupItem[]>([
        { label: "Czytane", value: "READING" },
        { label: "Przeczytane", value: "READ" },
        { label: "Skończone", value: "FINISHED" },
        { label: "Porzucone", value: "CANCELLED" },
    ]);

</script>
<template>
    <URadioGroup :items="filterMenuItems" orientation="horizontal" variant="table" indicator="hidden" v-model="bookFilterStore.selectedStatus"></URadioGroup>
</template>