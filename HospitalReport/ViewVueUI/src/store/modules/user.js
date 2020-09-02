import { login, logout,logintest,logindata,getmenulist } from '@/api/user'
import { getToken, setToken, removeToken } from '@/utils/auth'
import { resetRouter } from '@/router'
import request from '@/utils/request'
const getDefaultState = () => {
  return {
    token: getToken(),
    name: '',
    avatar: ''
  }
}
const state = getDefaultState()
const mutations = {
  RESET_STATE: (state) => {
    Object.assign(state, getDefaultState())
  },
  SET_TOKEN: (state, token) => {
    state.token = token
  },
  SET_NAME: (state, userInfo) => {
    state.userInfo = userInfo
  },
  SET_AVATAR: (state, avatar) => {
    state.avatar = avatar
  }
}

const actions = {
  // user login
  login({ commit }, userInfo) {
    const { username, password } = userInfo
    return new Promise((resolve, reject) => {
      login({ username: username.trim(), password: password }).then(response => {

        const { data } = response
        commit('SET_TOKEN', data.token)
        setToken(data.token)
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },
  logindata({ commit }, userInfo) {
    const { username, password } = userInfo
     return new Promise((resolve, reject) => {
      logindata({ UserName: username.trim(), Password: password }).then(response => {
        commit('SET_TOKEN', response.Data)
        commit('SET_AVATAR', "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif")
        setToken(response.Data)
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },
  logintest({ commit }, userInfo) {
    const { username, password } = userInfo
    return new Promise((resolve, reject) => {
      logintest({ username: username.trim(), password: password }).then(response => {
        console.log(response);
        const { data } = response
        commit('SET_TOKEN', data.token)
        setToken(data.token)
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },
  // get user info
  getmenulist() {
    return new Promise((resolve, reject) => {
      getmenulist().then(response => {
      }).catch(error => {
        reject(error)
      })
    })
  },

  // user logout
  logout({ commit, state }) {
    return new Promise((resolve, reject) => {
      logout(state.token).then(() => {
        removeToken() // must remove  token  first
        resetRouter()
        commit('RESET_STATE')
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // remove token
  resetToken({ commit }) {
    return new Promise(resolve => {

      removeToken() // must remove  token  first
      commit('RESET_STATE')
      resolve()
    })
  },

}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}

