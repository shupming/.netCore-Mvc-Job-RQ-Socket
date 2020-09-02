const tokens = {
  admin: {
    token: 'admin-token'
  },
  editor: {
    token: 'editor-token'
  },
  shu: {
    token: 'shu-token'
  }
}

const users = {
  'admin-token': {
    roles: ['admin'],
    introduction: 'I am a super administrator',
    avatar: 'https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif',
    name: 'Super Admin'
  },
  'editor-token': {
    roles: ['editor'],
    introduction: 'I am an editor',
    avatar: 'https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif',
    name: 'Normal Editor'
  }
}

const constantRouteList= [{
  "Path": "",
  "Name": "系统管理",
  "Redirect":"/example/table",
  "Icon": "",
  "Childrens": [{
    "Path": "tree/index",
    "Name": "菜单管理",
    "Redirect":"/example/table",
    "ParentId": "1",
    "Id": "2",
    "Icon": ""
  }, {
    "Path": "table/role",
    "Redirect":"/example/role",
    "Name": "角色管理",
    "ParentId": "1",
    "Id": "3",
    "Icon": ""
  }, {
    "Path": "form/index",
    "Redirect":"/form/index",
    "Name": "权限管理",
    "ParentId": "1",
    "Id": "4",
    "Icon": ""
  }, {
    "Path": "table/index",
    "Redirect":"/example/table",
    "Name": "用户管理",
    "ParentId": "1",
    "Id": "5",
    "Icon": ""
  }]
}]
const logindata ={
  UserName:"administrator",
  FullName:"超级管理员",
  CreationTime:"0001-01-01 12:00:00",
  Id:"1"
}
const shuMenuList= [
  {
    Path: '',
    Name: '系统管理-1',
    Icon:"el-icon-s-help",
    Childrens: [
      {
        Path: 'table',
        Name:"用户管理",
        PathUrl:"table/index",
        Icon:"table"
      },
      {
        Path: 'tree',
        Name:"角色管理",
        PathUrl:"tree/index",
        Icon:"tree"
      }
    ]
  }
]
module.exports = [
  {
    url: '/vue-admin-template/user/login',
    type: 'post',
    response: config => {
  
      const { username } = config.body
      const token = tokens[username]
      // mock error
      if (!token) {
        return {
          Status: "Error",
          message: '登录失败.'
        }
      }
      return {
        Code: "Success",
        Message: '登录成功.',
        Data: ''
      }
    }
  },
  {
    url: '/vue-admin-template/user/logintest',
    type: 'post',
    response: config => {
      const { username } = config.body
      const token = tokens[username]

      // mock error
      if (!token) {
        return {
          code: 60204,
          message: 'Account and password are incorrect.'
        }
      }
      return {
        code: 20000,
        data: token
      }
    }
  },
  {
    url: '/home/logindata',
    type: 'post',
    response: config => {
      return {
        Status: "Success",
        Message: '登录成功.1',
        Data:logindata
      }
    }
  },
  // get user info
  {
    url: '/vue-admin-template/user/info\.*',
    type: 'get',
    response: config => {
      return {
        Status: "Success",
        Message: '登录成功.1',
        Data: users["admin-token"]
      }
    }
  },
  {
    url: '/sysmenu/getmenulist',
    type: 'post',
    response: config => {
      const token = tokens['admin']
      return {
        Status: "Success",
        Data: shuMenuList
      }
    }
  },
  {
    url: '/home/LoginOut',
    type: 'post',
    response: config => {
      return {
        Status: "Success",
        Data: ""
      }
    }
  },
  // user logout
  {
    url: '/vue-admin-template/user/logout',
    type: 'post',
    response: _ => {
      return {
        code: 20000,
        data: 'success'
      }
    }
  }
]
