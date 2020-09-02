import router from './router'
import store from './store'
import { Message, Alert } from 'element-ui'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import { getToken } from '@/utils/auth' // get token from cookie
import getPageTitle from '@/utils/get-page-title'
import { String } from 'core-js'
NProgress.configure({ showSpinner: false }) // NProgress Configuration

const whiteList = ['/login'] // no redirect whitelist

router.beforeEach(async(to, from, next) => {
  // start progress bar
  NProgress.start()

  // set page title
  document.title = getPageTitle(to.meta.title)

  // determine whether the user has logged in

  const hasToken = getToken()
  if (hasToken) {
    if (to.path === '/login') {
      // if is logged in, redirect to the home page
      next({ path: '/' })
      NProgress.done()
    } else {
      if (store.getters.meuns.length>0) {
      //  global.antRouter = store.getters.meuns
      //  router.addRoutes(store.getters.meuns) // 2.动态添加路由
        //next({ ...to, replace: true })
       next()
      } else {
        try {
        await store.dispatch('sysmenus/getmenulist');
        let meuns=store.getters.meuns;
        if (meuns.length < 1) {
              global.antRouter = []
              next()
       }
        router.addRoutes(meuns) // 2.动态添加路由
        global.antRouter = meuns // 3.将路由数据传递给全局变量，做侧边栏菜单渲染工作
        next({ ...to, replace: true })
        // next()
        } catch (error) {
          // remove token and go to login page to re-login
          await store.dispatch('user/resetToken')
          Message.error(error || 'Has Error')
          next(`/login?redirect=${to.path}`)
          NProgress.done()
        }
      }
    }
  } else {
    /* has no token*/
    if (whiteList.indexOf(to.path) !== -1) {
      // in the free login whitelist, go directly
      next()
    } else {
      // other pages that do not have permission to access are redirected to the login page.
      next(`/login?redirect=${to.path}`)
      NProgress.done()
    }
  }
})

router.afterEach(() => {
  // finish progress bar
  NProgress.done()
})
