import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export default new Router({
  routes: [{
      path: '/',
      redirect: '/dashboard'
    },
    {
      path: '/',
      component: resolve => require(['../components/Layout/Home.vue'], resolve),
      meta: {
        title: '自述文件'
      },
      children: [{
          path: '/dashboard',
          component: resolve => require(['../components/Dashboard/Dashboard.vue'], resolve),
          meta: {
            title: '系统首页'
          }
        }
      ]
    },
    {
      path: '/Login',
      component: resolve => require(['../components/Login/Login.vue'], resolve),
    },
    {
      path: '/Register',
      component: resolve => require(['../components/Login/Register.vue'], resolve),
    },
  ]
})
