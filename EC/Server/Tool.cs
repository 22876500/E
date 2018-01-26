using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Tool
    {

        public static decimal Get开仓数量From已买卖数量(int 买卖方向, decimal 委托数量, decimal 已买股数, decimal 已卖股数)
        {
            decimal 待平数量 = 已买股数 - 已卖股数;
            decimal 开仓数量 = 0;
            if (待平数量 > 0)
            {
                if (买卖方向 == 0)
                {
                    开仓数量 = 委托数量;
                }
                else
                {
                    开仓数量 = 委托数量 - 待平数量;
                }
            }
            else if (待平数量 < 0)
            {
                if (买卖方向 == 0)
                {
                    开仓数量 = 委托数量 + 待平数量;
                }
                else
                {
                    开仓数量 = 委托数量;
                }
            }
            else
            {
                开仓数量 = 委托数量;
            }

            return 开仓数量;
        }




        public static DataTable ChangeDataStringToTable(string Data)
        {
            if (Data == string.Empty)
            {
                return null;
            }


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
            bool Changed = false;

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
                    Changed = true;
                }
                else
                {
                    for (int j = 0; j < DataRowWork.ItemArray.Length; j++)
                    {
                        if (DataTableUI.Columns[j].DataType == typeof(int))
                        {
                            int stringUI = (int)DataRowUI.ItemArray[j];
                            int stringWork = (int)DataRowWork.ItemArray[j];
                            if (stringUI != stringWork)
                            {
                                DataRowUI[j] = stringWork;
                                Changed = true;
                            }
                        }
                        else if (DataTableUI.Columns[j].DataType == typeof(DateTime))
                        {
                            DateTime stringUI = (DateTime)DataRowUI.ItemArray[j];
                            DateTime stringWork = (DateTime)DataRowWork.ItemArray[j];
                            if (stringUI != stringWork)
                            {
                                DataRowUI[j] = stringWork;
                                Changed = true;
                            }
                        }
                        else if (DataTableUI.Columns[j].DataType == typeof(decimal))
                        {
                            decimal stringUI = (decimal)DataRowUI.ItemArray[j];
                            decimal stringWork = (decimal)DataRowWork.ItemArray[j];
                            if (stringUI != stringWork)
                            {
                                DataRowUI[j] = stringWork;
                                Changed = true;
                            }
                        }
                        else if (DataTableUI.Columns[j].DataType == typeof(byte))
                        {
                            byte stringUI = (byte)DataRowUI.ItemArray[j];
                            byte stringWork = (byte)DataRowWork.ItemArray[j];
                            if (stringUI != stringWork)
                            {
                                DataRowUI[j] = stringWork;
                                Changed = true;
                            }
                        }
                        else
                        {
                            string stringUI = DataRowUI.ItemArray[j] as string;
                            string stringWork = DataRowWork.ItemArray[j] as string;
                            if (stringUI != stringWork)
                            {
                                DataRowUI[j] = stringWork;
                                Changed = true;
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
                    Changed = true;
                }
            }

            return Changed;
        }


        public static void SendNotifyToClient()
        {
            //发送废单通知
            Server.JyDataSet.委托Row 委托Row1;
            while (Program.废单通知.TryDequeue(out 委托Row1))
            {
                Tool.Send废单通知ToTrader(委托Row1);
            }

            //发送成交通知
            Server.JyDataSet.成交Row 成交Row1;
            while (Program.成交通知.TryDequeue(out 成交Row1))
            {
                Tool.Send成交通知ToTrader(成交Row1);
            }

            //发送风控操作通知
            风控操作 风控操作1;
            while (Program.风控操作.TryDequeue(out 风控操作1))
            {
                Tool.Send风控操作通知ToTrader(风控操作1);
            }

            RiskTableDeal();

            TradeChangedDeal();

            OrderChangedDeal();

            UsersTableDealed();

            LimitTableDeal();

            OrderTableDeal();

            OrderOperatedTableDeal();
        }

        private static void RiskTableDeal()
        {
            var riskControlKeys = Program.风控分配表Changed.Keys.ToList();
            foreach (string UserName in riskControlKeys)
            {
                if (Program.风控分配表Changed[UserName])
                {
                    Tool.Send风控分配TableChangedNoticeToFK(UserName, Program.db.QueryJyBelongFK(UserName));
                    Program.风控分配表Changed[UserName] = false;
                }
            }
        }

        private static void TradeChangedDeal()
        {
            var keysTrades = Program.成交表Changed.Keys.ToList();
            foreach (string UserName in keysTrades)
            {
                if (Program.成交表Changed[UserName])
                {
                    if (Program.交易员成交DataTable.ContainsKey(UserName))
                    {
                        var dtOrders = Program.交易员成交DataTable[UserName];
                        JyDataSet.成交DataTable dtSuccessOrder = null;
                        lock (dtOrders)
                        {
                            dtSuccessOrder = dtOrders.Copy() as JyDataSet.成交DataTable;
                        }

                        Send成交TableChangedNoticeToTrader(UserName, dtSuccessOrder);
                    }
                    Program.成交表Changed[UserName] = false;
                }
            }
        }

        private static void OrderChangedDeal()
        {
            var keysOrder = Program.委托表Changed.Keys.ToList();
            foreach (string UserName in keysOrder)
            {
                if (Program.委托表Changed[UserName])
                {
                    if (Program.交易员委托DataTable.ContainsKey(UserName))
                    {
                        var 交易员委托table = Program.交易员委托DataTable[UserName];
                        if (AutoOrderService.Instance.DictUserLogin.ContainsKey(UserName))
                        {
                            if (AutoOrderService.Instance.DictUserConsignmentChange.ContainsKey(UserName))
                            {
                                AutoOrderService.Instance.DictUserConsignmentChange[UserName] = true;
                            }
                            else if (!AutoOrderService.Instance.DictUserConsignmentChange.TryAdd(UserName, true))
                            {
                                AutoOrderService.Instance.DictUserConsignmentChange[UserName] = true;
                            }
                        }

                        JyDataSet.委托DataTable dtNew = null;
                        lock (交易员委托table)
                        {
                            dtNew = 交易员委托table.Copy() as JyDataSet.委托DataTable;
                        }
                        Send委托TableChangedNoticeToTrader(UserName, dtNew);
                    }
                    Program.委托表Changed[UserName] = false;
                }
            }
        }

        private static void UsersTableDealed()
        {
            var keysUser = Program.平台用户表Changed.Keys.ToList();
            foreach (string UserName in keysUser)
            {
                if (Program.平台用户表Changed[UserName])
                {
                    Send平台用户TableChangedNoticeToTrader(UserName, Program.db.平台用户.QuerySingleUser(UserName));
                    Program.平台用户表Changed[UserName] = false;
                }
            }
        }

        private static void LimitTableDeal()
        {
            var keysLimit = Program.额度分配表Changed.Keys.ToList();
            foreach (string UserName in keysLimit)
            {
                if (Program.额度分配表Changed[UserName])
                {
                    Send额度分配TableChangedNoticeToTrader(UserName, Program.db.额度分配.QueryTradeLimitBelongJy(UserName));
                    Program.额度分配表Changed[UserName] = false;
                }
            }
        }

        private static void OrderTableDeal()
        {
            var keysOrderChange = Program.订单表Changed.Keys.ToList();
            foreach (string UserName in Program.订单表Changed.Keys)
            {
                if (Program.订单表Changed[UserName])
                {
                    var orderTable = Program.db.订单.Query订单BelongJy(UserName);
                    if (AutoOrderService.Instance.DictUserLogin.ContainsKey(UserName))
                    {
                        if (AutoOrderService.Instance.DictUserOrderChange.ContainsKey(UserName))
                        {
                            AutoOrderService.Instance.DictUserOrderChange[UserName] = true;
                        }
                        else if (!AutoOrderService.Instance.DictUserOrderChange.TryAdd(UserName, true))
                        {
                            AutoOrderService.Instance.DictUserOrderChange[UserName] = true;
                        }
                    }
                    Send订单TableChangedNoticeToTrader(UserName, orderTable);
                    Program.订单表Changed[UserName] = false;
                }
            }
        }

        private static void OrderOperatedTableDeal()
        {
            var keysHansDealed = Program.已平仓订单表Changed.Keys;
            foreach (string UserName in Program.已平仓订单表Changed.Keys)
            {
                if (Program.已平仓订单表Changed[UserName])
                {
                    if (AutoOrderService.Instance.DictUserLogin.ContainsKey(UserName))
                    {
                        if (AutoOrderService.Instance.DictUserOrderChange.ContainsKey(UserName))
                        {
                            AutoOrderService.Instance.DictUserOrderChange[UserName] = true;
                        }
                        else if (!AutoOrderService.Instance.DictUserOrderChange.TryAdd(UserName, true))
                        {
                            AutoOrderService.Instance.DictUserOrderChange[UserName] = true;
                        }
                    }
                    Send已平仓订单TableChangedNoticeToTrader(UserName, Program.db.已平仓订单.Query已平仓订单BelongJy(UserName));
                    Program.已平仓订单表Changed[UserName] = false;
                }
            }
        }











        public static void Send成交TableChangedNoticeToTrader(string TradeName, Server.JyDataSet.成交DataTable DataTable1)
        {
            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                if (Program.ClientUserName[IClient1] == TradeName)
                {
                    IClient1.成交DataTableChanged(TradeName, DataTable1);
                }
            }


            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                //向普通风控员发送
                string ClientUserName = Program.ClientUserName[IClient1];
                if (Program.db.风控分配.Exists(TradeName, ClientUserName))
                {

                    IClient1.成交DataTableChanged(TradeName, DataTable1);
                }

                //向超级风控发送
                if (Program.db.平台用户.ExistsUserRole(ClientUserName, 角色.超级风控员))
                {
                    IClient1.成交DataTableChanged(TradeName, DataTable1);
                }
            }

        }
        public static void Send委托TableChangedNoticeToTrader(string TradeName, Server.JyDataSet.委托DataTable DataTable1)
        {
            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                if (Program.ClientUserName[IClient1] == TradeName)
                {
                    try
                    {
                        IClient1.委托DataTableChanged(TradeName, DataTable1);
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogInfoDetail("委托变更通知员工出错,员工：{0}; 出错信息：{1}", TradeName, ex.Message);
                    }

                }
            }


            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                //向普通风控员发送
                string ClientUserName = Program.ClientUserName[IClient1];
                if (Program.db.风控分配.Exists(TradeName, ClientUserName))
                {
                    try
                    {
                        IClient1.委托DataTableChanged(TradeName, DataTable1);
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogInfoDetail("委托变更通知风控出错,风控：{0}; 出错信息：{1}", ClientUserName, ex.Message);
                    }

                }

                //向超级风控发送
                if (Program.db.平台用户.ExistsUserRole(ClientUserName, 角色.超级风控员))
                {
                    try
                    {
                        IClient1.委托DataTableChanged(TradeName, DataTable1);
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogInfoDetail("委托变更通知超级管理员时出错,超级管理员：{0}; 出错信息：{1}", ClientUserName, ex.Message);
                    }


                }
            }

        }
        public static void Send平台用户TableChangedNoticeToTrader(string TradeName, Server.DbDataSet.平台用户DataTable DataTable1)
        {
            StringBuilder sbErr = new StringBuilder(256);
            try
            {
                sbErr.Append("Tool.Send平台用户TableChangedNoticeToTrader Function Start:");

                sbErr.Append("1.平台用户DataTableChanged To User Equals TraderName.");

                foreach (IClient IClient1 in Program.ClientUserName.Keys)
                {
                    if (Program.ClientUserName[IClient1] == TradeName)
                    {
                        IClient1.平台用户DataTableChanged(TradeName, DataTable1);
                    }
                }

                sbErr.Append("2.平台用户DataTableChanged To User belongs 风控");

                foreach (IClient IClient1 in Program.ClientUserName.Keys)
                {

                    //向普通风控员发送
                    string ClientUserName = Program.ClientUserName[IClient1];
                    if (Program.db.风控分配.Exists(TradeName, ClientUserName))
                    {

                        IClient1.平台用户DataTableChanged(TradeName, DataTable1);
                    }

                    //向超级风控发送
                    if (Program.db.平台用户.ExistsUserRole(ClientUserName, 角色.超级风控员))
                    {
                        IClient1.平台用户DataTableChanged(TradeName, DataTable1);
                    }
                }
            }
            catch (Exception) { }





        }
        public static void Send额度分配TableChangedNoticeToTrader(string TradeName, Server.DbDataSet.额度分配DataTable DataTable1)
        {
            try
            {
                foreach (IClient IClient1 in Program.ClientUserName.Keys)
                {
                    if (Program.ClientUserName[IClient1] == TradeName)
                    {
                        IClient1.额度分配DataTableChanged(TradeName, DataTable1);
                    }
                }

                foreach (IClient IClient1 in Program.ClientUserName.Keys)
                {
                    //向普通风控员发送
                    string ClientUserName = Program.ClientUserName[IClient1];
                    if (Program.db.风控分配.Exists(TradeName, ClientUserName))
                    {

                        IClient1.额度分配DataTableChanged(TradeName, DataTable1);
                    }

                    //向超级风控发送
                    if (Program.db.平台用户.ExistsUserRole(ClientUserName, 角色.超级风控员))
                    {
                        IClient1.额度分配DataTableChanged(TradeName, DataTable1);
                    }
                }
            }
            catch (Exception) { }

        }
        public static void Send订单TableChangedNoticeToTrader(string TradeName, Server.DbDataSet.订单DataTable DataTable1)
        {
            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                if (Program.ClientUserName[IClient1] == TradeName)
                {
                    IClient1.订单DataTableChanged(TradeName, DataTable1);
                }
            }


            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                //向普通风控员发送
                string ClientUserName = Program.ClientUserName[IClient1];
                if (Program.db.风控分配.Exists(TradeName, ClientUserName))
                {

                    IClient1.订单DataTableChanged(TradeName, DataTable1);
                }

                //向超级风控发送
                if (Program.db.平台用户.ExistsUserRole(ClientUserName, 角色.超级风控员))
                {
                    IClient1.订单DataTableChanged(TradeName, DataTable1);
                }
            }

        }
        public static void Send已平仓订单TableChangedNoticeToTrader(string TradeName, Server.DbDataSet.已平仓订单DataTable DataTable1)
        {
            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                if (Program.ClientUserName[IClient1] == TradeName)
                {
                    IClient1.已平仓订单DataTableChanged(TradeName, DataTable1);
                }
            }


            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                //向普通风控员发送
                string ClientUserName = Program.ClientUserName[IClient1];
                if (Program.db.风控分配.Exists(TradeName, ClientUserName))
                {

                    IClient1.已平仓订单DataTableChanged(TradeName, DataTable1);
                }

                //向超级风控发送
                if (Program.db.平台用户.ExistsUserRole(ClientUserName, 角色.超级风控员))
                {
                    IClient1.已平仓订单DataTableChanged(TradeName, DataTable1);
                }
            }

        }
        public static void Send风控分配TableChangedNoticeToFK(string FKUserName, Server.DbDataSet.平台用户DataTable 名下交易员DataTable1)
        {
            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                if (Program.ClientUserName[IClient1] == FKUserName)
                {
                    IClient1.风控分配DataTableChanged(名下交易员DataTable1);
                }
            }

        }



        public static void Send废单通知ToTrader(Server.JyDataSet.委托Row 委托Row1)
        {
            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                if (Program.ClientUserName[IClient1] == 委托Row1.交易员)
                {
                    IClient1.Notify(委托Row1.交易员, 委托Row1.证券代码, 委托Row1.证券名称, 委托Row1.委托编号, 委托Row1.买卖方向, 委托Row1.委托数量, 委托Row1.委托价格, "废单");
                }
            }
        }

        public static void Send成交通知ToTrader(Server.JyDataSet.成交Row 成交Row1)
        {
            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                if (Program.ClientUserName[IClient1] == 成交Row1.交易员)
                {
                    IClient1.Notify(成交Row1.交易员, 成交Row1.证券代码, 成交Row1.证券名称, 成交Row1.委托编号, 成交Row1.买卖方向, 成交Row1.成交数量, 成交Row1.成交价格, "委托成交");
                }
            }
        }

        public static void Send风控操作通知ToTrader(风控操作 风控操作1)
        {
            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                if (Program.ClientUserName[IClient1] == 风控操作1.交易员)
                {
                    IClient1.Notify(风控操作1.风控员, 风控操作1.证券代码, 风控操作1.证券名称, 风控操作1.委托编号, 风控操作1.买卖方向, 风控操作1.委托数量, 风控操作1.委托价格, 风控操作1.操作内容);
                }
            }
        }


        public static bool IsSendOrderTimeFit()
        {
            if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 15)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
