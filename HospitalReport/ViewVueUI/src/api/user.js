import request from '@/utils/request'
//import { Alert } from 'element-ui'
export const  getmenulist= data => request.post("/sysmenu/getmenulist",data)

export function login(data) {
  return request({
    url: '/vue-admin-template/user/login',
    method: 'post',
    data
  })
}
export function logintest(data) {
  return request({
    url: '/vue-admin-template/user/login',
    method: 'post',
    data
  })
}
export function logindata(data) {
  return request({
    url: '/home/logindata',
    method: 'post',
    data
  })
}
export function logout() {
  return request({
    url: '/home/LoginOut',
    method: 'post'
  })
}
