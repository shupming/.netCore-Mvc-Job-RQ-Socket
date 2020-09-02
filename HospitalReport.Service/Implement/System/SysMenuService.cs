using HospitalReport.Models.DataBase;
using HospitalReport.Models.ViewModel;
using HospitalReport.Service.Common;
using HospitalReport.Service.Interface.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using HospitalReport.Models.Common;

namespace HospitalReport.Service.Implement.System
{
    public class SysMenuService :  BaseService<Sys_Menu>,ISysMenuService
    {
        public SysMenuService(IHospitalReportDbContext dbContext) : base(dbContext)
        {

        }
        public ReturnedDataResult getList()
        {
            return new ReturnedDataResult
            {
                Status = ReturnedStatus.Success,
                Data = GetMenuItemViewList(this.GetQueryable().Select(t => new Cm_Menu_ViewModel
                {
                    Name = t.MenuName,
                    ParentId = t.ParentId,
                    Id = t.Id,
                    Path = t.Path,
                    Icon=t.Icon
                }).ToList())
            };
        }

        /// <summary>
        /// 递归获取模块树形菜单
        /// </summary>
        /// <param name="userView"></param>
        /// <param name="parentId">父级模块ID</param>
        /// <param name="originList">所有模块列表</param>
        /// <returns>返回模块树形菜单</returns>
        public List<Cm_Menu_ViewModel> GetMenuItemViewList(List<Cm_Menu_ViewModel> allList,int ParentId=0)
        {
           var menuItemViewList = new List<Cm_Menu_ViewModel>();
            var menuList = allList.Where(t=>t.ParentId== ParentId).ToList();
            foreach (var item in menuList)
            {
                var menuModel = new Cm_Menu_ViewModel();

                menuModel.Name = item.Name;
                menuModel.Path = item.Path;
                menuModel.ParentId = item.ParentId;
                menuModel.Icon = item.Icon;
                menuModel.Id = item.Id;
                if (allList.Any(t => t.ParentId == menuModel.Id)) {
                    menuModel.Childrens= GetMenuItemViewList(allList, menuModel.Id);
                }
                menuItemViewList.Add(menuModel);
            }
            return menuItemViewList;
        }
    }
}