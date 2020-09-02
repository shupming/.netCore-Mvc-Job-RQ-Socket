using HospitalReport.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace HospitalReport.Common
{
   public class EntityReflection
    { 
        public static List<PropertyNameModel> GetPropertyNameModels(Type type)
        {
            if (type == null)
            {
                return new List<PropertyNameModel>();
            }
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (!properties.Any<PropertyInfo>())
            {
                return null;
            }
            List<PropertyNameModel> propertyNameModels = new List<PropertyNameModel>();
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                string displayName = string.Empty;
                object[] customAttributes = item.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                if (customAttributes.Any<object>())
                {
                    DisplayNameAttribute displayNameAttribute = customAttributes.First<object>() as DisplayNameAttribute;
                    displayName =  displayNameAttribute?.DisplayName??string.Empty;
                }
                if (item.PropertyType.IsValueType || item.PropertyType.Name=="String")
                {
                    propertyNameModels.Add(new PropertyNameModel
                    {
                        Name = name,
                        DisplayName = displayName
                    });
                }
            }
            return propertyNameModels;
        }

        public static List<PropertyModel> GetPropertyModels<T>(T t)
        {
            if (t == null)
            {
                return new List<PropertyModel>();
            }
            var properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (!properties.Any())
            {
                return new List<PropertyModel>();
            }
            List<PropertyModel> propertyModels = new List<PropertyModel>();
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t);
                string displayName = string.Empty;
                object[] customAttributes = item.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                if (customAttributes.Any())
                {
                    var  displayNameAttribute = customAttributes.First() as DisplayNameAttribute;
                    displayName = displayNameAttribute.DisplayName??string.Empty;
                }
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    propertyModels.Add(new PropertyModel
                    {
                        Name = name,
                        Value = value,
                        DisplayName = displayName,
                        TypeCode = Type.GetTypeCode(item.PropertyType)
                    });
                }
            }
            return propertyModels;
        }
    }
}
