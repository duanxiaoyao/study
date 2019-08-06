import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      component: resolve => require(['../components/Login/Login.vue'], resolve),
    },
    {
      path: '/Register',
      component: resolve => require(['../components/Login/Register.vue'], resolve),
    },
    {
      path: '/Home',
      component: resolve => require(['../components/Home/Home.vue'], resolve),
    }
  ]
})
