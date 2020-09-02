using HospitalReport.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HospitalReport.Common
{
    public static class ModelFunc
    {
        /// <summary>
        /// 单个实体转集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> OToList<T>(this object obj)
        {
            var result = new List<T>();
            if (obj == null)
                return result;

            var type = typeof(T);
            if (obj.GetType() != type)
                return result;

            result.Add((T)obj);
            return result;
        }
        /// <summary>
        /// 对比两个实体类属性值的差异
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="oldMod">原实体类</param>
        /// <param name="newMod">新实体类</param>
        /// <returns>差异记录</returns>
        public static string GetChangeData<T>(T oldMod, T newMod)
        {
            var namelist = new List<string>() {
                 "CreatorId",
                 "Creator",
                 "CreationTime",
                 "LastModifierId",
                 "LastModifier",
                 "LastModificationTime"
            };
            Type typeDescription = typeof(DescriptionAttribute);
            if (oldMod == null || newMod == null) { return ""; }
            string updateData = "";
            PropertyInfo[] mPi = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            for (int i = 0; i < mPi.Length; i++)
            {
                PropertyInfo pi = mPi[i];
                object[] arr = pi.GetCustomAttributes(typeDescription, true);
                if (!namelist.Contains(pi.Name))
                {
                    string atrr = arr.Length > 0 ? ((DescriptionAttribute)arr[0]).Description : pi.Name;

                    object oldObj = pi.GetValue(oldMod, null);
                    object newObj = pi.GetValue(newMod, null);
                    string oldValue = oldObj == null ? "" : oldObj.ToString();
                    string newValue = newObj == null ? "" : newObj.ToString();
                    if (oldValue != newValue)
                    {
                        updateData += atrr + "：由" + oldValue + " 改成 " + newValue + "<br/>";
                    }
                }
            }
            return updateData;
        }

        public static T ModelCopy<T>(this T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;
            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, ModelCopy(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;

        }
        /// <summary>
        /// 导入的excel 必须和 实体字段顺序对上 (实体多余字段往后放)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> TableToList<T>(this DataTable table) where T : new()
        {
            var list = new List<T>();
            var rows = table.AsEnumerable().Where(t => t.ItemArray.Any(s => !string.IsNullOrWhiteSpace(s.ToString())));
            foreach (var row in rows)
            {
                var model = new T();
                for (var i = 0; i < row.ItemArray.Count(); i++)
                {
                    var propertyInfo = typeof(T).GetProperties()[i];
                    try
                    {
                        if (propertyInfo != null && row[i] != DBNull.Value)
                        {
                            if (propertyInfo.PropertyType == typeof(string))
                                propertyInfo.SetValue(model, (row[i] ?? string.Empty).ToString().Trim(), null);
                            if (propertyInfo.PropertyType == typeof(int))
                                propertyInfo.SetValue(model, Convert.ToInt32(row[i] ?? 0), null);
                            if (propertyInfo.PropertyType == typeof(DateTime))
                                propertyInfo.SetValue(model, Convert.ToDateTime(row[i] ?? DateTime.Now), null);
                        }
                    }
                    catch
                    {
                        propertyInfo.SetValue(model, null, null);
                    }
                }
                list.Add(model);
            }
            return list;
        }


        public static void SetUserToModelCreate<T>(this T inputT, Cm_User_ViewModel userView=null)
        {
            if (userView == null || inputT == null)
                userView = new Cm_User_ViewModel
                {
                    Id = 0,
                    FullName = "system"
                };

            var type = inputT.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == "CreationUserName")
                {
                    property.SetValue(inputT, userView.UserName ?? userView.FullName);
                }
                else if (property.Name == "CreationFullName")
                {
                    property.SetValue(inputT, userView.FullName ?? userView.UserName);
                }
                else if (property.Name == "CreationTime")
                {
                    property.SetValue(inputT, DateTime.UtcNow);
                }
                else if (property.Name == "LastModificationTime")
                {
                    property.SetValue(inputT, DateTime.UtcNow);
                }
                else if (property.Name == "LastModificationUserName")
                {
                    property.SetValue(inputT, userView.UserName ?? userView.FullName);
                }
                else if (property.Name == "LastModificationFullName")
                {
                    property.SetValue(inputT, userView.FullName ?? userView.UserName);
                }
            }
        }

        public static void SetUserToModelEdit<T>(this T inputT, Cm_User_ViewModel userView = null)
        {
            if (userView == null || inputT == null)
                userView = new Cm_User_ViewModel
                {
                    Id = 0,
                    FullName = "system"
                };
            var type = inputT.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == "LastModifierId")
                {
                    property.SetValue(inputT, userView.Id);
                }
                else if (property.Name == "LastModifier")
                {
                    property.SetValue(inputT, userView.FullName ?? userView.UserName);
                }
                else if (property.Name == "LastModificationTime")
                {
                    property.SetValue(inputT, DateTime.UtcNow);
                }
            }
        }
    }
}
