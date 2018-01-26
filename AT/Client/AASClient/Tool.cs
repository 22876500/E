using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AASClient
{
    static class Tool
    {

        public const Int32 AW_HOR_POSITIVE = 0x00000001;    //自左向右显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;    //自右向左显示窗口。当使用了 AW_CENTER 标志时该标志被忽略
        public const Int32 AW_VER_POSITIVE = 0x00000004;    //自顶向下显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
        public const Int32 AW_VER_NEGATIVE = 0x00000008;    //自下向上显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略
        public const Int32 AW_CENTER = 0x00000010;    //若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展
        public const Int32 AW_HIDE = 0x00010000;    //隐藏窗口，缺省则显示窗口
        public const Int32 AW_ACTIVATE = 0x00020000;    //激活窗口。在使用了AW_HIDE标志后不要使用这个标志
        public const Int32 AW_SLIDE = 0x00040000;    //使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
        public const Int32 AW_BLEND = 0x00080000;    //使用淡入效果。只有当hWnd为顶层窗口的时候才可以使用此标志


        /// <summary>
        /// API动态显示窗体
        /// </summary>
        /// <param name="hwnd">hwnd：目标窗口句柄。</param>
        /// <param name="dwTime">dwTime：动画的持续时间，数值越大动画效果的时间就越长。</param>
        /// <param name="dwFlags">DwFlags：DwFlags参数是动画效果类型选项，该参数在C#中的详细声明含义请看最下面的备注。</param>
        /// <returns></returns>
        [DllImportAttribute("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);





        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 SWP_NOZORDER = 0x0004;
        public const UInt32 SWP_NOREDRAW = 0x0008;
        public const UInt32 SWP_NOACTIVATE = 0x0010;
        public const UInt32 SWP_FRAMECHANGED = 0x0020;
        public const UInt32 SWP_SHOWWINDOW = 0x0040;
        public const UInt32 SWP_HIDEWINDOW = 0x0080;
        public const UInt32 SWP_NOCOPYBITS = 0x0100;
        public const UInt32 SWP_NOOWNERZORDER = 0x0200;
        public const UInt32 SWP_NOSENDCHANGING = 0x0400;
        public const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint wFlags);



        public static DataTable ChangeDataStringToTable(string Data)
        {
            DataTable DataTable1 = new DataTable("Result");

            string[] Lines = Data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] ColumnNames = Lines[0].Split(new char[] { '\t' });
            for (int i = 0; i < ColumnNames.Length; i++)
            {
                if (DataTable1.Columns.IndexOf(ColumnNames[i]) == -1)
                {
                    DataTable1.Columns.Add(ColumnNames[i]);
                }
                else
                {
                    DataTable1.Columns.Add(ColumnNames[i] + "_" + i.ToString());
                }
            }


            for (int i = 1; i < Lines.Length; i++)
            {
                string[] Cells = Lines[i].Split(new char[] { '\t' });
                DataTable1.Rows.Add(Cells);
            }

            return DataTable1;
        }

        public static void RefreshDataTable(DataTable DataTableUI, DataTable DataTableWork, string IDColName)
        {
            if (DataTableUI.Columns.Count == 0)
            {
                foreach (DataColumn DataColumn1 in DataTableWork.Columns)
                {
                    DataTableUI.Columns.Add(DataColumn1.ColumnName);
                }
            }

            DataTableUI.PrimaryKey = new DataColumn[] { DataTableUI.Columns[IDColName] };
            DataTableWork.PrimaryKey = new DataColumn[] { DataTableWork.Columns[IDColName] };



            foreach (DataRow DataRowWork in DataTableWork.Rows)
            {
                string WorkIDRowValue = DataRowWork[IDColName] as string;
                DataRow DataRowUI = DataTableUI.Rows.Find(WorkIDRowValue);
                if (DataRowUI == null)
                {
                    DataTableUI.ImportRow(DataRowWork);
                }
                else
                {
                    for (int j = 0; j < DataRowWork.ItemArray.Length; j++)
                    {
                        string stringUI = DataRowUI.ItemArray[j] as string;
                        string stringWork = DataRowWork.ItemArray[j] as string;
                        if (stringUI != stringWork)
                        {
                            DataRowUI[j] = stringWork;
                        }
                    }
                }
            }
            for (int i = DataTableUI.Rows.Count - 1; i >= 0; i--)
            {
                string UIIDRowValue = DataTableUI.Rows[i][IDColName] as string;
                DataRow DataRowWork = DataTableWork.Rows.Find(UIIDRowValue);
                if (DataRowWork == null)
                {
                    DataTableUI.Rows.RemoveAt(i);
                }
            }

        }
        public static bool RefreshDrcjDataTable(DataTable DataTableUI, DataTable DataTableWork, string[] IDColNames)
        {
            bool Ret = false;

            if (DataTableUI.Columns.Count == 0)
            {
                foreach (DataColumn DataColumn1 in DataTableWork.Columns)
                {
                    DataTableUI.Columns.Add(DataColumn1.ColumnName);
                    Ret = true;
                }
            }

            DataColumn[] IDColsUI = new DataColumn[IDColNames.Length];
            DataColumn[] IDColsWork = new DataColumn[IDColNames.Length];
            for (int i = 0; i < IDColNames.Length; i++)
            {
                IDColsUI[i] = DataTableUI.Columns[IDColNames[i]];
                IDColsWork[i] = DataTableWork.Columns[IDColNames[i]];
            }

            DataTableUI.PrimaryKey = IDColsUI;
            DataTableWork.PrimaryKey = IDColsWork;


            foreach (DataRow DataRowWork in DataTableWork.Rows)
            {
                object[] WorkIDRowValues = new object[IDColNames.Length];
                for (int i = 0; i < IDColNames.Length; i++)
                {
                    WorkIDRowValues[i] = DataRowWork[IDColNames[i]];
                }
                DataRow DataRowUI = DataTableUI.Rows.Find(WorkIDRowValues);
                if (DataRowUI == null)
                {
                    DataTableUI.ImportRow(DataRowWork);
                    Ret = true;
                }
                else
                {
                    for (int j = 0; j < DataRowWork.ItemArray.Length; j++)
                    {
                        if (DataTableWork.Columns[j].DataType == typeof(int))
                        {
                            int stringUI = (int)DataRowUI.ItemArray[j];
                            int stringWork =(int) DataRowWork.ItemArray[j];
                            if (stringUI != stringWork)
                            {
                                DataRowUI[j] = stringWork;
                                Ret = true;
                            }
                        }
                        else if (DataTableWork.Columns[j].DataType == typeof(DateTime))
                        {
                            DateTime stringUI = (DateTime)DataRowUI.ItemArray[j];
                            DateTime stringWork = (DateTime)DataRowWork.ItemArray[j];
                            if (stringUI != stringWork)
                            {
                                DataRowUI[j] = stringWork;
                                Ret = true;
                            }
                        }
                        else if (DataTableWork.Columns[j].DataType == typeof(decimal))
                        {
                            decimal stringUI = (decimal)DataRowUI.ItemArray[j];
                            decimal stringWork = (decimal)DataRowWork.ItemArray[j];
                            if (stringUI != stringWork)
                            {
                                DataRowUI[j] = stringWork;
                                Ret = true;
                            }
                        }
                        else if (DataTableWork.Columns[j].DataType == typeof(byte))
                        {
                            byte stringUI = (byte)DataRowUI.ItemArray[j];
                            byte stringWork = (byte)DataRowWork.ItemArray[j];
                            if (stringUI != stringWork)
                            {
                                DataRowUI[j] = stringWork;
                                Ret = true;
                            }
                        }
                        else
                        {
                            string stringUI = DataRowUI.ItemArray[j] as string;
                            string stringWork = DataRowWork.ItemArray[j] as string;
                            if (stringUI != stringWork)
                            {
                                DataRowUI[j] = stringWork;
                                Ret = true;
                            }
                        }
                    }
                }
            }
            for (int i = DataTableUI.Rows.Count - 1; i >= 0; i--)
            {
                object[] UIIDRowValues = new object[IDColNames.Length];
                for (int j = 0; j < IDColNames.Length; j++)
                {
                    UIIDRowValues[j] = DataTableUI.Rows[i][IDColNames[j]];
                }

                DataRow DataRowWork = DataTableWork.Rows.Find(UIIDRowValues);
                if (DataRowWork == null)
                {
                    DataTableUI.Rows.RemoveAt(i);
                    Ret = true;
                }
            }

            return Ret;
        }

        public static DataTable GetNewDataTable(DataTable dt, string condition)
        {
            DataTable newdt = new DataTable();
            newdt = dt.Clone();
            DataRow[] dr = dt.Select(condition);
            for (int i = 0; i < dr.Length; i++)
            {
                newdt.ImportRow((DataRow)dr[i]);
            }
            return newdt;//返回的查询结果
        }
    }
}
