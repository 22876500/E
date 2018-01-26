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

namespace QuotaShareClient
{
    /// <summary>
    /// Excel的辅助类
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 读取excel到datatable中
        /// </summary>
        /// <param name="excelPath">excel地址</param>
        /// <param name="sheetIndex">sheet索引</param>
        /// <returns>成功返回datatable，失败返回null</returns>
        public static DataTable ImportExcel(string excelPath, int sheetIndex)
        {
            List<string> lstRows = new List<string>();
            IWorkbook workbook = null;//全局workbook
            ISheet sheet;//sheet
            DataTable table = null;
            bool isFirst = true;
            try
            {
                FileInfo fileInfo = new FileInfo(excelPath);//判断文件是否存在
                if (fileInfo.Exists)
                {
                    FileStream fileStream = fileInfo.OpenRead();//打开文件，得到文件流
                    switch (fileInfo.Extension)
                    {
                        //xls是03，用HSSFWorkbook打开，.xlsx是07或者10用XSSFWorkbook打开
                        case ".xls": workbook = new HSSFWorkbook(fileStream); break;
                        case ".xlsx": workbook = new XSSFWorkbook(fileStream); break;
                        default: break;
                    }
                    fileStream.Close();//关闭文件流
                }
                if (workbook != null)
                {
                    sheet = workbook.GetSheetAt(sheetIndex);//读取到指定的sheet
                    table = new DataTable();//初始化一个table

                    IRow headerRow = sheet.GetRow(0);//获取第一行，一般为表头
                    int cellCount = headerRow.Cells.Count;//得到列数

                    if (cellCount != CommonUtils.heads.Length)
                    {
                        //Program.logger.LogInfo(string.Format("导入的已分配额度表格式不一致,应为“{0}”{1}列", string.Join(",", CommonUtils.heads), CommonUtils.heads.Length));
                        return table;
                    }
                    else
                    {
                        for (int i = 0; i < CommonUtils.heads.Length; i++)
                        {
                            DataColumn dc = new DataColumn(CommonUtils.heads[i]);
                            table.Columns.Add(dc);
                        }
                    }
                    try
                    {
                        int code = int.Parse(headerRow.GetCell(1).ToString());
                    }
                    catch (Exception)
                    {
                        isFirst = false;
                    }
                    int startIdx = sheet.FirstRowNum;
                    if (isFirst == false)
                    {
                        startIdx = sheet.FirstRowNum + 1;
                    }
                    //遍历读取cell
                    for (int i = startIdx; i <= sheet.LastRowNum; i++)
                    {
                        StringBuilder sb = new StringBuilder();
                        NPOI.SS.UserModel.IRow row = sheet.GetRow(i);//得到一行
                        DataRow dataRow = table.NewRow();//新建一个行
                        bool flag = true;

                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            ICell cell = row.GetCell(j);//得到cell
                            if (cell == null)//如果cell为null，则赋值为空
                            {
                                dataRow[j] = "";
                            }
                            else
                            {
                                if (j == 1)//股票列表
                                {
                                    if (string.IsNullOrEmpty(row.GetCell(j).ToString().Trim())) { flag = false; break; }
                                    dataRow[j] = row.GetCell(j).ToString().Trim().PadLeft(6, '0');

                                }
                                else
                                {

                                    dataRow[j] = row.GetCell(j).ToString().Trim();//否则赋值
                                }
                            }
                            sb.Append(dataRow[j]);
                        }
                        if (flag)
                        {
                            if (lstRows.Contains(sb.ToString())) { continue; }
                            lstRows.Add(sb.ToString());

                            table.Rows.Add(dataRow);//把行 加入到table中
                        }

                    }
                }
                return table;

            }
            catch (Exception e)
            {
                //Program.logger.LogInfo("读取excel文件异常,{0}", e.Message);
                return table;
            }
            finally
            {
                //释放资源
                if (table != null) { table.Dispose(); }
                workbook = null;
                sheet = null;
            }
        }

    }
}
