import { getmenulist } from '@/api/user'
import Layout from '@/layout'


const state = {
  menus: []
}

const mutations = {
  SET_MENUS: (state, meuns) => {
    state.menus = meuns
  }
}
export function generaMenu(routes, data) {
  data.forEach(item => {
    let menu
    if (item.Childrens) {
      menu = {
        path: item.Path,
        component: Layout,
        children: [],
        //redirect:item.Redirect,
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
}
const actions = {
  getmenulist({ commit }) {
    return new Promise(resolve => {
      const loadMenuData = []
      // 先查询后台并返回左侧菜单数据并把数据添加到路由
      getmenulist().then(response => {
        if (response.Status === 'Success') {
          let routes=[]
          generaMenu(routes, response.Data)
          commit('SET_MENUS', routes)
          resolve(routes)
        }
      }).catch(error => {
        console.log(error)
      })
    })
  },
  resetMenu({ commit }){
    commit('SET_MENUS', []);
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
