import { createApp } from "vue";
import App from "./App.vue";
import './assets/main.css'
import { createPinia } from 'pinia'
import ui from '@nuxt/ui/vue-plugin'
import { router } from "./router";



const app = createApp(App)
const pinia = createPinia()

app.use(ui);
app.use(pinia)
app.use(router);
app.mount('#app')

