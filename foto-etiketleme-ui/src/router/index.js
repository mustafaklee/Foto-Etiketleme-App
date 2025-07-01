import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import TagView from '../views/TagView.vue'
import LoginView from '@/views/Login/LoginView.vue'
const routes = [
  {path: '/', name: 'Login', component: LoginView }, // giriş sayfası
  { path: '/Home', name: 'Home', component: HomeView }, //kök sayfa ilk hangi sayfa yüklenecek
  { path: '/etiketle', name: 'Tag', component: TagView }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
