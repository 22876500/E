using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASClient
{
    public class ExcelUtils
    {
        public static bool IsExcel(string fileFullPath)
        {
            bool ret = false;

            FileStream fs = File.OpenRead(fileFullPath);
            BinaryReader r = new BinaryReader(fs);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {
                return false;
            }
            r.Close();
            fs.Close();


            String[] fileType = { "208207", "8075" };
            ret = fileType.Contains(fileclass);
            return ret;

            /*文件扩展名说明
             *4946/104116 txt
             *7173        gif 
             *255216      jpg
             *13780       png
             *6677        bmp
             *239187      txt,aspx,asp,sql
             *208207      xls.doc.ppt
             *6063        xml
             *6033        htm,html
             *4742        js
             *8075        xlsx,zip,pptx,mmap,zip
             *8297        rar   
             *01          accdb,mdb
             *7790        exe,dll           
             *5666        psd 
             *255254      rdp 
             *10056       bt种子 
             *64101       bat 
             *4059        sgf
             *239187      csv
             */
        }

        public static DataTable ReadExcel(string fileFullPath, int headerRowIndex = 0, int sheetIndex = 0)
        {
            DataTable dt = new DataTable();
            using (var stream = File.OpenRead(fileFullPath))
            {
                IWorkbook bk = fileFullPath.ToLower().EndsWith(".xls") ? new HSSFWorkbook(stream) as IWorkbook : new XSSFWorkbook(stream);
                ISheet sheet = bk.GetSheetAt(sheetIndex);
                SheetToDataTable(dt, sheet, headerRowIndex);
            }
            return dt;
        }

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

        #region Private Func
        private static void SheetToDataTable(DataTable dt, ISheet sheet, int headerRowIndex = 0)
        {
            headerRowIndex = HeaderIndexFix(dt, sheet, headerRowIndex);

            for (int j = headerRowIndex + 1; j <= sheet.LastRowNum; j++)  //LastRowNum 是当前表的总行数
            {
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
                        if (cell != null)
                        {
                            dtRow[k] = cell.ToString();
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
