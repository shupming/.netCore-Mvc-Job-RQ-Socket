using HospitalReport.Models.Common;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Linq;
using OfficeOpenXml.Style;
using System.Drawing;

namespace HospitalReport.Common
{
    public class ExcelExtend
    {
        public static string ExcelContentType
        {
            get
            {
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
        }
        public static bool DataTableToExcel(DataTable dt, string sheetName, string fileName)
        {
            bool result = false;
            if (string.IsNullOrEmpty(fileName))
            {
                return result;
            }
            if (dt.Rows.Count == 0)
            {
                return result;
            }
            bool result2;
            using (ExcelPackage package = new ExcelPackage(new FileInfo(fileName)))
            {
                if (string.IsNullOrEmpty(sheetName))
                {
                    sheetName = "sheet1";
                }
                package.Workbook.Worksheets.Add(sheetName);
                ExcelWorksheet sheet = package.Workbook.Worksheets[1];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sheet.Cells[1, i + 1].Value = dt.Columns[i].ColumnName.ToLower();
                }
                int nRow = 1;
                foreach (object obj in dt.Rows)
                {
                    DataRow row = (DataRow)obj;
                    nRow++;
                    try
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            sheet.Cells[nRow, j + 1].Value = row[j];
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                package.Save();
                result = true;
                result2 = result;
            }
            return result2;
        }
        public static bool ListToExcel<T>(List<T> list, string sheetName, string fileName)
        {
            bool result = false;
            if (string.IsNullOrEmpty(fileName))
            {
                return result;
            }
            if (list.Count == 0)
            {
                return result;
            }
            bool result2;
            using (ExcelPackage package = new ExcelPackage(new FileInfo(fileName)))
            {
                if (string.IsNullOrEmpty(sheetName))
                {
                    sheetName = "sheet1";
                }
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add(sheetName);
                var  properties = list[0].GetType().GetProperties(BindingFlags.Instance
                    | BindingFlags.Public);
                if (properties != null)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        string colName = properties[i].Name;
                        sheet.Cells[1, i + 1].Value = colName.ToLower();
                    }
                }
                int cellIndex = 1;
                for (int rowIndex = 0; rowIndex < list.Count; rowIndex++)
                {
                    cellIndex++;
                    properties = list[rowIndex].GetType()
                       .GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    if (properties != null)
                    {
                        for (int j = 0; j < properties.Length; j++)
                        {
                            object value = properties[j].GetValue(list[rowIndex], null);
                            sheet.Cells[cellIndex, j + 1].Value = value;
                        }
                    }
                }
                package.Save();
                result = true;
                result2 = result;
            }
            return result2;
        }
        public static byte[] ListToExcel<T>(List<T> ts)
        {
            byte[] asByteArray;
            var data = new Dictionary<string, List<T>>();
            data.Add("Sheet1", ts);
            using (ExcelPackage excelPackage = ExportExcel(data))
            {
                asByteArray = excelPackage.GetAsByteArray();
            }
            return asByteArray;
        }

        public static byte[] ListToExcel<T>(Dictionary<string, List<T>>  ts)
        {
            byte[] asByteArray;
            using (ExcelPackage excelPackage = ExportExcel<T>(ts))
            {
                asByteArray = excelPackage.GetAsByteArray();
            }
            return asByteArray;
        }
        public static ExcelPackage ExportExcel<T>(Dictionary<string, List<T>> ts)
        {
            if (ts == null)
            {
                throw new ArgumentException("export data is null！");
            }
            var excelPackage = new ExcelPackage();
            foreach (var item in ts)
            {
                var sheet = excelPackage.Workbook.Worksheets.Add(item.Key);
                var propertyNames = EntityReflection.GetPropertyNameModels(typeof(T))
                    .Where(t => !string.IsNullOrEmpty(t.DisplayName)).ToList();
                if (propertyNames.Any())
                {
                    int col = 1;
                    foreach (PropertyNameModel property in propertyNames)
                    {
                        string title = string.IsNullOrEmpty(property.DisplayName) ? property.Name : property.DisplayName;
                        sheet.Cells[1, col].Value = title;
                        GetBorder(sheet.Cells[1, col]);
                        col++;
                    }
                }
                int r = 2;
                item.Value.ForEach(t =>
                {
                    var list = EntityReflection.GetPropertyModels(t)
                    .Where(t1 => !string.IsNullOrEmpty(t1.DisplayName)).ToList();
                    int col2 = 1;
                    foreach (PropertyModel property2 in list)
                    {
                        sheet.Cells[r, col2].Value = property2.Value;
                        if (property2.TypeCode == TypeCode.DateTime)
                        {
                            sheet.Cells[r, col2].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                        }
                        GetBorder(sheet.Cells[r, col2]);
                        col2++;
                      
                    }
                    r++;
                });
            }
            return excelPackage;
        }
        /// <summary>
        /// 给excel 格子换边框
        /// </summary>
        /// <param name="r"></param>
        public static void GetBorder(ExcelRange r)
        {
            r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            r.Style.Border.Top.Color.SetColor(Color.Black);
            r.Style.Border.Bottom.Color.SetColor(Color.Black);
            r.Style.Border.Left.Color.SetColor(Color.Black);
            r.Style.Border.Right.Color.SetColor(Color.Black);
        }
        /// <summary>
        /// 给格子字体颜色，格子背景
        /// </summary>
        /// <param name="r"></param>
        public static void GetFontColor(ExcelRange r, Color colorFont, Color colorBackground)
        {
            r.Style.Font.Color.SetColor(colorFont);
            r.Style.Font.Bold = true;
            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            // r.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#1fb5ad"));
            r.Style.Fill.BackgroundColor.SetColor(colorBackground);
        }
    }
}
