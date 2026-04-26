import { createRouter, createWebHashHistory } from 'vue-router';
import MainView from './views/MainView.vue';
import BookFormView from './views/BookFormView.vue';

const routes = [
  { path: '/', component: MainView },
  { path: '/bookForm', component: BookFormView },
  { path: '/bookForm/:id', component: BookFormView }
]

export const router = createRouter({
  history: createWebHashHistory(),
  routes,
  scrollBehavior() {
    return { top: 0 }
  }
})