import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Login from '../views/Login.vue'
import About from '../views/About.vue'
import CreatePlayer from "@/components/CreatePlayer.vue";
import PlayersManagement from '../views/PlayersManagement.vue';
import CheckListTemplates from '../views/CheckListTemplates.vue';
import CheckListTemplate_Questions from "@/components/CheckListTemplate_Questions.vue";
import OnBoardingTasks from '../views/OnBoardingTasks.vue';

Vue.use(VueRouter)

const routes = [
  {
    path: '/home',
    name: 'home',
    component: Home
  },
  {
    path: '/aboutus',
    name: 'about',
    component: About
  },
  {
    path: '',
    name: 'Login',
    component: Login
  },
  {
    path: '/PlayersManagement',
    name: 'PlayersManagement',
    component: PlayersManagement,
    meta: {
      requiresAuth: true
    }
  },
  {
    path: '/addEditPlayer',
    name: 'addEditPlayer',
    component: CreatePlayer
  },
  {
    path: '/CheckListTemplates',
    name: 'CheckListTemplates',
    component: CheckListTemplates
  },
  {
    path: '/CheckListTemplate_Questions',
    name: 'CheckListTemplate_Questions',
    component: CheckListTemplate_Questions
  },
  {
    path: '/OnBoardingTasks',
    name: 'OnBoardingTasks',
    component: OnBoardingTasks
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
