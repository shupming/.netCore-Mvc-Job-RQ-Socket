import { CommonRouteList,constantRouteList } from '@/router'
import { getmenulist } from '@/api/user'
import Layout from '@/layout'
import { Message } from "element-ui"
/**
 * 后台查询的菜单数据拼装成路由格式的数据
 * 这里是demo使用的是以前项目的后台数据，这里强制修改了一下
 * @param routes
 */

export function generaMenu(routes, data) {
  data.forEach(item => {
    let menu
    if (item.Childrens) {
      menu = {
        path: '',
        component: Layout,
        children: [],
        redirect: '',
        meta: { title: item.Name, icon: item.Icon }
      }
      generaMenu(menu.children, item.Childrens);
    } else {
      menu = {
        path: item.Path,
        component: loadView(item.Path),
        name: item.Name,
        meta: { title: item.Name , icon: item.Icon }
      }
    }
    routes.push(menu)
   
  })
}
export const loadView = (path) => { // 路由懒加载
  return (resolve) => require([`@/views/${path}`], resolve)
  // return () => import(`@/views${parent}/${path}/index`)
}
const state = {
  routes: [],
  addRoutes: []
}
const mutations = {
  SET_ROUTES: (state, routes) => {
    state.addRoutes = routes
   // state.routes =constantRouteList.concat(routes)
     state.routes =routes.concat(CommonRouteList)
  }
}
const actions = {
  generateRoutes({ commit }) {

    return new Promise(resolve => {
      const loadMenuData = []
      // 先查询后台并返回左侧菜单数据并把数据添加到路由
      getmenulist().then(response => {
        if (response.Status === 'Success') {
          let routes=[]
          generaMenu(routes, response.Data)
          commit('SET_ROUTES', routes)
          resolve(routes)
        }
      }).catch(error => {
        console.log(error)
      })
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}



