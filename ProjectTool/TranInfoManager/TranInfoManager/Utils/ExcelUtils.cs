using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TranInfoManager.Utils
{
    class ExcelUtils
    {
        #region Read Excel
        public static DataSet ReadExcelFileDataset(string fileFullPath)
        {
            DataSet ds = new DataSet();
            using (var stream = File.OpenRead(fileFullPath))
            {
                IWorkbook bk = fileFullPath.ToLower().EndsWith(".xls") ? new HSSFWorkbook(stream) as IWorkbook : new XSSFWorkbook(stream);
                for (int i = 0; i < bk.NumberOfSheets; i++)
                {
                    ds.Tables.Add();
                    ISheet sheet = bk.GetSheetAt(i);
                    SheetToDataTable(ds.Tables[i], sheet);
                }
            }
            return ds;
        }

        public static DataTable ReadExcelFileDataTable(string fileFullPath, int headerRowIndex = 0, int sheetIndex = 0,Action<int, int> onRowReadAction = null)
        {
            DataTable dt = new DataTable();
            using (var stream = File.OpenRead(fileFullPath))
            {
                IWorkbook bk = fileFullPath.ToLower().EndsWith(".xls") ? new HSSFWorkbook(stream) as IWorkbook : new XSSFWorkbook(stream);
                ISheet sheet = bk.GetSheetAt(sheetIndex);
                SheetToDataTable(dt, sheet, headerRowIndex, onRowReadAction);
            }
            return dt;
        }

        public static void ReadExcelFile(string fileFullPath, List<string[]> lst, Action<int, int> onRowReadAction = null)
        {
            using (var stream = File.OpenRead(fileFullPath))
            {
                IWorkbook bk = fileFullPath.ToLower().EndsWith(".xls") ? new HSSFWorkbook(stream) as IWorkbook : new XSSFWorkbook(stream);
                ISheet sheet = bk.GetSheetAt(0);
                lst.AddRange(SheetToList(sheet));
            }
        }
        #endregion

        #region Render To Excel excel
        public static IWorkbook RenderToExcel(DataTable table, bool isXLS = true)
        {
            IWorkbook workbook = isXLS ? new HSSFWorkbook() as IWorkbook : new XSSFWorkbook() as IWorkbook;
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in table.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName.ToString());

            //handling value.
            int rowIndex = 1;

            foreach (DataRow row in table.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in table.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString().Replace(" 0:00:00", ""));
                }
                rowIndex++;
            }
            return workbook;
        }

        public static IWorkbook RenderToExcel<T>(List<T> list, string[] Members, string fileName)
        {
            IWorkbook workbook = fileName.EndsWith(".xls") ? new HSSFWorkbook() as IWorkbook : new XSSFWorkbook() as IWorkbook;

            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);
            List<PropertyInfo> PIL = new List<PropertyInfo>(Members.Length);

            // handling header.
            for (int i = 0; i < Members.Length; i++)
            {
                headerRow.CreateCell(i).SetCellValue(Members[i]);
                PIL.Add(typeof(T).GetProperty(Members[i]));
            }

            for (int i = 0; i < list.Count; i++)
            {
                IRow dataRow = sheet.CreateRow(i);
                for (int j = 0; j < Members.Length; j++)
                    dataRow.CreateCell(j).SetCellValue((PIL[j].GetValue(list[i]) + "").Replace(" 0:00:00", ""));
            }

            return workbook;
        }

        public static void Export<T>(DataGrid dg)
        {
            try
            {
                SaveFileDialog s = new SaveFileDialog() { Filter = "Excel 97~2003|*.xls|Excel 2007/2010|*.xlsx" };
                if (s.ShowDialog() == true && !string.IsNullOrEmpty(s.FileName))
                {
                    using (var stream = File.OpenWrite(s.FileName))
                    {
                        var list = (dg.ItemsSource as IEnumerable<T>).ToList();
                        var Members = dg.Columns.Select(_ => _.SortMemberPath).ToArray();
                        var Headers = dg.Columns.Select(_ => _.Header.ToString());

                        var wb = Utils.ExcelUtils.RenderToExcel<T>(list, Members, s.FileName);
                        wb.Write(stream);
                    }

                }
            }
            catch (Exception ex)
            {
                CommonUtils.Log("导出错误！", ex);
            }
        }
        #endregion

        #region Render To String
        public static string RenderToString(DataTable dt, int perCellLength = 64)
        {
            var s = new StringBuilder(dt.Rows.Count * dt.Columns.Count * perCellLength);

            foreach (DataColumn dtc in dt.Columns)
                s.Append(dtc.ColumnName).Append('\t');
            s.Remove(s.Length - 1, 1);


            foreach (DataRow dtr in dt.Rows)
            {
                foreach (DataColumn dtc in dt.Columns)
                    s.Append(dtr[dtc].ToString().Replace(" 0:00:00", "")).Append('\t');
                if (s.Length > 0 && s[s.Length - 1] == '\t')
                    s.Remove(s.Length - 1, 1).Append(Environment.NewLine);
            }

            return s.ToString();
        }

        public static string RenderToString<T>(DataGrid dg)
        {
            var headers = dg.Columns.Select(_ => _.Header.ToString()).ToArray();
            var properties = dg.Columns.Select(_ => _.SortMemberPath).ToArray();
            var source = dg.ItemsSource as List<T>;
            return RenderToString(source, properties, headers);
        }

        public static string RenderToString<T>(IEnumerable<T> iEnumerable, string[] Members, string[] Headers = null, int perCellLength = 64, Action<int, int> onRowReadAction = null)
        {
            StringBuilder s = new StringBuilder(iEnumerable.Count() * Members.Count() * perCellLength);

            s.Append(string.Join("\t", Headers ?? Members));

            List<PropertyInfo> plist = new List<PropertyInfo>(Members.Length);
            for (int i = 0; i < Members.Length; i++)
                plist.Add(typeof(T).GetProperty(Members[i]));

            foreach (T entity in iEnumerable)
            {
                for (int i = 0; i < Members.Length; i++)
                    s.Append((plist[i].GetValue(entity) + "").Replace(" 0:00:00", "")).Append('\t');

                if (s.Length > 0 && s[s.Length - 1] == '\t')
                    s.Remove(s.Length - 1, 1).Append(Environment.NewLine);
            }
            return s.ToString();
        }
        #endregion

        #region Private Func
        private static void SheetToDataTable(DataTable dt, ISheet sheet, int headerRowIndex = 0, Action<int, int> onRowReadAction = null)
        {
            headerRowIndex = HeaderIndexFix(dt, sheet, headerRowIndex);

            for (int j = headerRowIndex + 1; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
            {
                if (onRowReadAction != null)
                    onRowReadAction.Invoke(j, sheet.LastRowNum);
                
                IRow row = sheet.GetRow(j);
                if (row != null)
                {
                    for (int i = dt.Columns.Count; i < row.LastCellNum; i++)
                    {
                        dt.Columns.Add();
                    }

                    var dtRow = dt.NewRow();
                    dt.Rows.Add(dtRow);
                    for (int k = 0; k <= row.LastCellNum; k++)
                    {
                        ICell cell = row.GetCell(k);  //当前表格
                        if (cell != null )
                        {
                            var value = cell + "";
                            if (cell.CellType == CellType.Numeric && value.IsDate())
                                value = cell.DateCellValue.ToString();

                            dtRow[k] = value;
                        }
                    }
                }
            }
        }

        private static int HeaderIndexFix(DataTable dt, ISheet sheet, int headerRowIndex)
        {
            if (headerRowIndex >= 0)
            {
                var headerRow = sheet.GetRow(headerRowIndex);
                for (int i = 0; i < headerRow.LastCellNum; i++)
                {
                    dt.Columns.Add(headerRow.GetCell(i).ToString());
                }
            }
            else
            {
                headerRowIndex = -1;
            }
            return headerRowIndex;
        }

        private static List<string[]> SheetToList(ISheet sheet)
        {
            List<string[]> list = new List<string[]>();
            for (int j = 0; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
            {
                IRow row = sheet.GetRow(j);
                if (row != null)
                {
                    var arr = new string[row.LastCellNum];
                    list.Add(arr);
                    for (int k = 0; k <= row.LastCellNum; k++)
                    {
                        ICell cell = row.GetCell(k);  //当前表格
                        if (cell != null)
                        {
                            arr[k] = cell.ToString();
                        }
                    }
                }
            }
            return list;
        }

        #endregion
    }
}
