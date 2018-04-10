using AASServer.YJServiceReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AASServer
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
            if (Program.废单通知.Count > 0)
            {
                //发送废单通知
                AASServer.JyDataSet.委托Row 委托Row1;
                while (Program.废单通知.TryDequeue(out 委托Row1))
                {
                    Tool.Send废单通知ToTrader(委托Row1);
                }    
            }

            if (Program.成交通知.Count > 0)
            {
                //发送成交通知
                AASServer.JyDataSet.成交Row 成交Row1;
                while (Program.成交通知.TryDequeue(out 成交Row1))
                {
                    Tool.Send成交通知ToTrader(成交Row1);
                }
            }

            if (Program.风控操作.Count > 0)
            {
                //发送风控操作通知
                风控操作 风控操作1;
                while (Program.风控操作.TryDequeue(out 风控操作1))
                {
                    Tool.Send风控操作通知ToTrader(风控操作1);
                }
            }
            

            RiskTableDeal();

            TradeChangedDeal();

            OrderChangedDeal();

            OrderTableDeal();

            UsersTableDealed();

            LimitTableDeal();

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
            Task.Run(() => {
                try
                {
                    foreach (string UserName in Program.成交表Changed.Keys)
                    {
                        if (Program.成交表Changed[UserName])
                        {
                            if (Program.交易员成交DataTable.ContainsKey(UserName))
                            {
                                Send成交TableChangedNoticeToTrader(UserName, Program.交易员成交DataTable[UserName]);
                            }
                            Program.成交表Changed[UserName] = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfoDetail("Tool.OrderChangedDeal(交易员成交) Exception, {0}", ex.Message);
                }
            });
            
        }

        private static void OrderChangedDeal()
        {
            Task.Run(() => {
                try
                {
                    foreach (string UserName in Program.委托表Changed.Keys)
                    {
                        if (Program.委托表Changed[UserName])
                        {
                            if (Program.交易员委托DataTable.ContainsKey(UserName))
                            {
                                Send委托TableChangedNoticeToTrader(UserName, Program.交易员委托DataTable[UserName]);
                            }
                            Program.委托表Changed[UserName] = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfoDetail("Tool.OrderChangedDeal(交易员委托) Exception, {0}", ex.Message);
                }
            });
           
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
            Task.Run(() => {
                try
                {
                    foreach (string UserName in Program.订单表Changed.Keys)
                    {
                        if (Program.订单表Changed[UserName])
                        {
                            Send订单TableChangedNoticeToTrader(UserName, Program.db.订单.Query订单BelongJy(UserName));
                            Program.订单表Changed[UserName] = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfoDetail("Tool.OrderTableDeal Exception, {0}", ex.Message);
                }
            });

            
        }

        private static void OrderOperatedTableDeal()
        {
            var keysHansDealed = Program.已平仓订单表Changed.Keys;
            foreach (string UserName in Program.已平仓订单表Changed.Keys)
            {
                if (Program.已平仓订单表Changed[UserName])
                {
                    Send已平仓订单TableChangedNoticeToTrader(UserName, Program.db.已平仓订单.Query已平仓订单BelongJy(UserName));
                    Program.已平仓订单表Changed[UserName] = false;
                }
            }
        }











        public static void Send成交TableChangedNoticeToTrader(string TradeName, AASServer.JyDataSet.成交DataTable DataTable1)
        {
            //foreach (IClient IClient1 in Program.ClientUserName.Keys)
            //{
            //    try
            //    {
            //        if (Program.ClientUserName[IClient1] == TradeName)
            //        {
            //            IClient1.成交DataTableChanged(TradeName, DataTable1);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Program.logger.LogInfoDetail("成交变更通知交易员出错,员工：{0}; 出错信息：{1}", TradeName, ex.Message);
            //    }
            //}

            //foreach (IClient IClient1 in Program.ClientUserName.Keys)
            //{
            //    try
            //    {
            //        //向普通风控员发送
            //        string ClientUserName = Program.ClientUserName[IClient1];
            //        if (Program.db.风控分配.Exists(TradeName, ClientUserName))
            //        {
            //            IClient1.成交DataTableChanged(TradeName, DataTable1);
            //        }
            //        //向超级风控发送
            //        if (Program.db.平台用户.ExistsUserRole(ClientUserName, 角色.超级风控员))
            //        {
            //            IClient1.成交DataTableChanged(TradeName, DataTable1);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Program.logger.LogInfoDetail("成交变更通知风控出错,员工：{0}; 出错信息：{1}", TradeName, ex.Message);
            //    }
            //}

            foreach (var item in Program.ClientUserName)
            {
                try
                {
                    if (Program.db.风控分配.Exists(TradeName, item.Value))
                    {
                        item.Key.成交DataTableChanged(TradeName, DataTable1);
                    }
                    else if (item.Value == TradeName)
                    {
                        item.Key.成交DataTableChanged(TradeName, DataTable1);
                    }
                    else if (Program.db.平台用户.ExistsUserRole(item.Value, 角色.超级风控员))
                    {
                        item.Key.成交DataTableChanged(TradeName, DataTable1);
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfoDetail("成交变更通知风控出错,交易员：{0}, 通知对象{1}; 出错信息：{2}", TradeName, item.Value, ex.Message);
                }
            }

        }
        public static void Send委托TableChangedNoticeToTrader(string TradeName, AASServer.JyDataSet.委托DataTable DataTable1)
        {
            //foreach (IClient IClient1 in Program.ClientUserName.Keys)
            //{
            //    try
            //    {
            //        if (Program.ClientUserName[IClient1] == TradeName)
            //        {
            //            IClient1.委托DataTableChanged(TradeName, DataTable1);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Program.logger.LogInfoDetail("委托变更通知员工出错,员工：{0}; 出错信息：{1}", TradeName, ex.Message);
            //    }
            //}


            //foreach (IClient IClient1 in Program.ClientUserName.Keys)
            //{
            //    try
            //    {
            //        //向普通风控员发送
            //        if (Program.db.风控分配.Exists(TradeName, Program.ClientUserName[IClient1]))
            //        {
            //            IClient1.委托DataTableChanged(TradeName, DataTable1);
            //        }
            //        //向超级风控发送
            //        if (Program.db.平台用户.ExistsUserRole(Program.ClientUserName[IClient1], 角色.超级风控员))
            //        {
            //            IClient1.委托DataTableChanged(TradeName, DataTable1);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Program.logger.LogInfoDetail("委托变更通知风控及管理员出错, 出错信息：{0}",  ex.Message);
            //    }
            //}
            foreach (var item in Program.ClientUserName)
            {
                try
                {
                    if (item.Value == TradeName)
                    {
                        item.Key.委托DataTableChanged(TradeName, DataTable1);
                    }
                    else if (Program.db.风控分配.Exists(TradeName, item.Value))
                    {
                        item.Key.委托DataTableChanged(TradeName, DataTable1);
                    }
                    else if (Program.db.平台用户.ExistsUserRole(item.Value, 角色.超级风控员))
                    {
                        item.Key.委托DataTableChanged(TradeName, DataTable1);
                    }
                }
                catch (Exception) { }
            }
        }
        public static void Send平台用户TableChangedNoticeToTrader(string TradeName, AASServer.DbDataSet.平台用户DataTable DataTable1)
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
        public static void Send额度分配TableChangedNoticeToTrader(string TradeName, AASServer.DbDataSet.额度分配DataTable DataTable1)
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
        public static void Send订单TableChangedNoticeToTrader(string TradeName, AASServer.DbDataSet.订单DataTable DataTable1)
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
        public static void Send已平仓订单TableChangedNoticeToTrader(string TradeName, AASServer.DbDataSet.已平仓订单DataTable DataTable1)
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
        public static void Send风控分配TableChangedNoticeToFK(string FKUserName, AASServer.DbDataSet.平台用户DataTable 名下交易员DataTable1)
        {
            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                if (Program.ClientUserName[IClient1] == FKUserName)
                {
                    IClient1.风控分配DataTableChanged(名下交易员DataTable1);
                }
            }

        }



        public static void Send废单通知ToTrader(AASServer.JyDataSet.委托Row 委托Row1)
        {
            foreach (IClient IClient1 in Program.ClientUserName.Keys)
            {
                if (Program.ClientUserName[IClient1] == 委托Row1.交易员)
                {
                    IClient1.Notify(委托Row1.交易员, 委托Row1.证券代码, 委托Row1.证券名称, 委托Row1.委托编号, 委托Row1.买卖方向, 委托Row1.委托数量, 委托Row1.委托价格, "废单");
                }
            }
        }

        public static void Send成交通知ToTrader(AASServer.JyDataSet.成交Row 成交Row1)
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



        public static void SendYJDataToServer()
        {
            Program.logger.LogInfo("业绩数据发送开始...");
            YJServiceReference.JyDataSet.业绩统计DataTable 业绩统计DataTable1 = new YJServiceReference.JyDataSet.业绩统计DataTable();
            StringBuilder sb = new StringBuilder();

            AASServer.DbDataSet.已发委托DataTable 已发委托DataTable1 = new DbDataSet.已发委托DataTable();
            AASServer.DbDataSet.已平仓订单DataTable 已平仓订单DataTable1 = new DbDataSet.已平仓订单DataTable();
            Dictionary<string, AASServer.DbDataSet.平台用户Row> dictUser = new Dictionary<string, DbDataSet.平台用户Row>();

            已平仓订单DataTable1.Load();
            已发委托DataTable1.LoadToday();

            foreach (AASServer.DbDataSet.已发委托Row 已发委托Row1 in 已发委托DataTable1.Where(r => r.成交数量 != 0))
            {
                decimal 交易费用 = 0;
                decimal 交易额度 = 0;
                try
                {
                    AASServer.DbDataSet.平台用户Row AASUser1;
                    
                    if (dictUser.ContainsKey(已发委托Row1.交易员))
                    {
                        AASUser1 = dictUser[已发委托Row1.交易员];
                    }
                    else
                    {
                        AASUser1 = Program.db.平台用户.Get平台用户(已发委托Row1.交易员);
                        dictUser.Add(已发委托Row1.交易员, AASUser1);
                    }

                    var group = ShareLimitAdapter.Instance.GetLimitGroup(已发委托Row1.交易员);
                    if (group != null)
                    {
                        var stockLimitItem = group.GroupStockList.FirstOrDefault(_ => _.GroupAccount == 已发委托Row1.组合号 && _.StockID == 已发委托Row1.证券代码);
                        if (stockLimitItem != null)
                        {
                            交易费用 += ShareLimitAdapter.Instance.Get交易费用(已发委托Row1, decimal.Parse(stockLimitItem.Commission));
                            交易额度 = decimal.Parse(stockLimitItem.LimitCount);
                        }
                        else
                        {
                            Program.logger.LogInfo("业绩统计逻辑异常，额度共享逻辑未找到组合号{0}下证券代码{1}对应的配置项！");
                        }
                    }
                    else
                    {
                        AASServer.DbDataSet.额度分配Row AASPermition = Program.db.额度分配.Get额度分配(已发委托Row1.交易员, 已发委托Row1.组合号, 已发委托Row1.证券代码);
                        if (AASPermition == null)
                        {
                            AASUser1 = Program.db.平台用户.Get平台用户(已发委托Row1.交易员);
                            交易费用 = 已发委托Row1.Get交易费用(AASUser1.手续费率);
                        }
                        else
                        {
                            交易费用 = 已发委托Row1.Get交易费用(AASPermition.手续费率);
                            交易额度 = AASPermition.交易额度;
                        }
                    }
                    if (!业绩统计DataTable1.Any(r => r.交易员 == 已发委托Row1.交易员 && r.组合号 == 已发委托Row1.组合号 && r.证券代码 == 已发委托Row1.证券代码))
                    {
                        #region 生成业绩统计Row
                        YJServiceReference.JyDataSet.业绩统计Row 业绩统计RowNew = 业绩统计DataTable1.New业绩统计Row();
                        业绩统计RowNew.交易员 = 已发委托Row1.交易员;
                        业绩统计RowNew.组合号 = 已发委托Row1.组合号;
                        业绩统计RowNew.证券代码 = 已发委托Row1.证券代码;
                        业绩统计RowNew.证券名称 = 已发委托Row1.证券名称;

                        业绩统计RowNew.交易额度 = 交易额度;

                        if (已发委托Row1.买卖方向 == 0)
                        {
                            #region 买单
                            业绩统计RowNew.买入数量 = 已发委托Row1.成交数量;
                            业绩统计RowNew.买入金额 = 已发委托Row1.成交价格 * 已发委托Row1.成交数量;
                            业绩统计RowNew.买入均价 = 已发委托Row1.成交价格;


                            业绩统计RowNew.卖出数量 = 0;
                            业绩统计RowNew.卖出金额 = 0;
                            业绩统计RowNew.卖出均价 = 0;

                            #endregion

                        }
                        else
                        {
                            #region 卖单
                            业绩统计RowNew.买入数量 = 0;
                            业绩统计RowNew.买入金额 = 0;
                            业绩统计RowNew.买入均价 = 0;

                            业绩统计RowNew.卖出数量 = 已发委托Row1.成交数量;
                            业绩统计RowNew.卖出金额 = 已发委托Row1.成交价格 * 已发委托Row1.成交数量;
                            业绩统计RowNew.卖出均价 = 已发委托Row1.成交价格;


                            #endregion
                        }


                        业绩统计RowNew.毛利 = 0;
                        业绩统计RowNew.交易费用 = 交易费用;
                        业绩统计RowNew.净利润 = 0;
                        业绩统计RowNew.分组 = Enum.GetName(typeof(分组), AASUser1.分组);
                        业绩统计RowNew.日期 = Convert.ToInt32(已发委托Row1.日期.ToString("yyyyMMdd"));
                        业绩统计DataTable1.Add业绩统计Row(业绩统计RowNew);

                        #endregion
                    }
                    else
                    {
                        #region 修改业绩统计Row
                        YJServiceReference.JyDataSet.业绩统计Row 业绩统计Row1 = 业绩统计DataTable1.First(r => r.交易员 == 已发委托Row1.交易员 && r.组合号 == 已发委托Row1.组合号 && r.证券代码 == 已发委托Row1.证券代码);

                        if (已发委托Row1.买卖方向 == 0)
                        {
                            #region 买单
                            业绩统计Row1.买入数量 += 已发委托Row1.成交数量;
                            业绩统计Row1.买入金额 += 已发委托Row1.成交价格 * 已发委托Row1.成交数量;
                            业绩统计Row1.买入均价 = Math.Round(业绩统计Row1.买入金额 / 业绩统计Row1.买入数量, 3, MidpointRounding.AwayFromZero);
                            #endregion
                        }
                        else
                        {
                            #region 卖单
                            业绩统计Row1.卖出数量 += 已发委托Row1.成交数量;
                            业绩统计Row1.卖出金额 += 已发委托Row1.成交价格 * 已发委托Row1.成交数量;
                            业绩统计Row1.卖出均价 = Math.Round(业绩统计Row1.卖出金额 / 业绩统计Row1.卖出数量, 3, MidpointRounding.AwayFromZero);
                            #endregion
                        }



                        业绩统计Row1.毛利 = 0;
                        业绩统计Row1.交易费用 += 交易费用;
                        业绩统计Row1.净利润 = 0;
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo("已发委托.交易员=" + 已发委托Row1.交易员 + ",委托编号=" + 已发委托Row1.委托编号 + System.Environment.NewLine + ex.Message);
                }
            }
            foreach (YJServiceReference.JyDataSet.业绩统计Row 业绩统计Row1 in 业绩统计DataTable1)
            {
                if (已平仓订单DataTable1.Any(r => r.交易员 == 业绩统计Row1.交易员 && r.组合号 == 业绩统计Row1.组合号 && r.证券代码 == 业绩统计Row1.证券代码))
                {
                    业绩统计Row1.毛利 = 已平仓订单DataTable1.Where(r => r.交易员 == 业绩统计Row1.交易员 && r.组合号 == 业绩统计Row1.组合号 && r.证券代码 == 业绩统计Row1.证券代码).Sum(r => r.毛利);
                    业绩统计Row1.净利润 = 业绩统计Row1.毛利 - 业绩统计Row1.交易费用;
                }
            }

            if (Program.DataServiceClient != null)
            {
                Program.DataServiceClient.SendYJDataAsync(业绩统计DataTable1, CommonUtils.Mac);
                Program.logger.LogInfo("业绩数据发送结束");
            }
            else
            {
                Program.logger.LogInfo("业绩数据发送失败");
            }

        }

        public static void SendWTDataToServer()
        {
            try
            {
                Program.logger.LogInfo("委托数据发送开始...");
                YJServiceReference.JyDataSet.已发委托DataTable new已发委托DataTable1 = new YJServiceReference.JyDataSet.已发委托DataTable();

                AASServer.DbDataSet.已发委托DataTable 已发委托DataTable1 = new DbDataSet.已发委托DataTable();
                已发委托DataTable1.LoadToday();

                foreach (AASServer.DbDataSet.已发委托Row 已发委托Row1 in 已发委托DataTable1)
                {
                    YJServiceReference.JyDataSet.已发委托Row 已发委托RowNew = new已发委托DataTable1.New已发委托Row();
                    new已发委托DataTable1.Add已发委托Row(已发委托RowNew);

                    已发委托RowNew.日期 = 已发委托Row1.日期;
                    已发委托RowNew.组合号 = 已发委托Row1.组合号;
                    已发委托RowNew.委托编号 = 已发委托Row1.委托编号;
                    已发委托RowNew.交易员 = 已发委托Row1.交易员;
                    已发委托RowNew.状态说明 = 已发委托Row1.状态说明;
                    已发委托RowNew.市场代码 = 已发委托Row1.市场代码;
                    已发委托RowNew.证券代码 = 已发委托Row1.证券代码;
                    已发委托RowNew.证券名称 = 已发委托Row1.证券名称;
                    已发委托RowNew.买卖方向 = 已发委托Row1.买卖方向;
                    已发委托RowNew.成交价格 = 已发委托Row1.成交价格;
                    已发委托RowNew.成交数量 = 已发委托Row1.成交数量;
                    已发委托RowNew.委托价格 = 已发委托Row1.委托价格;
                    已发委托RowNew.委托数量 = 已发委托Row1.委托数量;
                    已发委托RowNew.撤单数量 = 已发委托Row1.撤单数量;
                }

                if (Program.DataServiceClient != null)
                {
                    Program.DataServiceClient.SendWTDataAsync(new已发委托DataTable1, CommonUtils.Mac);
                    Program.logger.LogInfo("委托数据发送结束");
                }
                else
                {
                    Program.logger.LogInfo("委托数据发送失败");
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static void SendQuotaDataToServer()
        {
            try
            {
                Program.logger.LogInfo("额度分配数据发送开始...");
                Dictionary<string, string> dicGroup = new Dictionary<string, string>();
                YJServiceReference.JyDataSet.额度分配DataTable new额度分配DataTable1 = new YJServiceReference.JyDataSet.额度分配DataTable();
                AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                平台用户DataTable1.LoadToday();

                foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
                {
                    if (!dicGroup.ContainsKey(平台用户Row1.用户名))
                    {
                        dicGroup.Add(平台用户Row1.用户名, Enum.GetName(typeof(分组), 平台用户Row1.分组));
                    }
                }

                //var limitValue = CommonUtils.GetConfig("UseZmqInterface");
                //if ("1".Equals(limitValue))
                //{
                //    //策略下单
                //    foreach (ShareLimitGroupItem item in ShareLimitAdapter.Instance.ShareLimitGroups)
                //    {
                //        foreach (LimitTrader trader in item.GroupTraderList)
                //        {
                //            foreach (StockLimitItem subItem in item.GroupStockList)
                //            {
                //                YJServiceReference.JyDataSet.额度分配Row 额度分配RowNew = new额度分配DataTable1.New额度分配Row();
                //                new额度分配DataTable1.Add额度分配Row(额度分配RowNew);

                //                额度分配RowNew.交易员 = trader.TraderAccount;
                //                额度分配RowNew.证券代码 = subItem.StockID;
                //                额度分配RowNew.组合号 = subItem.GroupAccount;
                //                额度分配RowNew.市场 = (byte)(subItem.StockID.StartsWith("6") ? 1 : 0);
                //                额度分配RowNew.证券名称 = subItem.StockName;
                //                额度分配RowNew.拼音缩写 = string.Empty;
                //                额度分配RowNew.买模式 = Convert.ToInt32(subItem.BuyType);
                //                额度分配RowNew.卖模式 = Convert.ToInt32(subItem.SaleType);
                //                额度分配RowNew.交易额度 = Convert.ToDecimal(subItem.LimitCount);
                //                额度分配RowNew.手续费率 = Convert.ToDecimal(subItem.Commission);
                //                if (dicGroup.ContainsKey(额度分配RowNew.交易员))
                //                    额度分配RowNew.分组 = dicGroup[额度分配RowNew.交易员];
                //                else
                //                {
                //                    额度分配RowNew.分组 = string.Empty;
                //                }
                //            }
                //        }
                //    }

                //}
                //else
                {

                    AASServer.DbDataSet.额度分配DataTable 额度分配DataTable1 = new DbDataSet.额度分配DataTable();
                    额度分配DataTable1.LoadToday();

                    foreach (AASServer.DbDataSet.额度分配Row 额度分配Row1 in 额度分配DataTable1)
                    {
                        YJServiceReference.JyDataSet.额度分配Row 额度分配RowNew = new额度分配DataTable1.New额度分配Row();
                        new额度分配DataTable1.Add额度分配Row(额度分配RowNew);

                        额度分配RowNew.交易员 = 额度分配Row1.交易员;
                        额度分配RowNew.证券代码 = 额度分配Row1.证券代码;
                        额度分配RowNew.组合号 = 额度分配Row1.组合号;
                        额度分配RowNew.市场 = 额度分配Row1.市场;
                        额度分配RowNew.证券名称 = 额度分配Row1.证券名称;
                        额度分配RowNew.拼音缩写 = 额度分配Row1.拼音缩写;
                        额度分配RowNew.买模式 = 额度分配Row1.卖模式;
                        额度分配RowNew.卖模式 = 额度分配Row1.卖模式;
                        额度分配RowNew.交易额度 = 额度分配Row1.交易额度;
                        额度分配RowNew.手续费率 = 额度分配Row1.手续费率;
                        if (dicGroup.ContainsKey(额度分配RowNew.交易员))
                            额度分配RowNew.分组 = dicGroup[额度分配RowNew.交易员];
                        else
                        {
                            额度分配RowNew.分组 = string.Empty;
                        }
                    }
                }


                if (Program.DataServiceClient != null)
                {
                    Program.DataServiceClient.SendQuotaDataAsync(new额度分配DataTable1, CommonUtils.Mac);
                    Program.logger.LogInfo("额度分配数据发送结束");
                }
                else
                {
                    Program.logger.LogInfo("额度分配数据发送失败");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public static bool InitService()
        {
            try
            {
                if (Program.DataServiceClient != null) return true;

                //string serverIP = "192.168.1.2";
                string serverIP = CommonUtils.GetConfig("CollectServerIp");
                string serverPort = CommonUtils.GetConfig("CollectServerPort");
                if (string.IsNullOrEmpty(serverPort)) { serverPort = "80"; }
                string endPoint = string.Format("http://{0}:{1}/", serverIP, serverPort);

                var httpBinding = new WSHttpBinding(SecurityMode.None);
                httpBinding.MaxReceivedMessageSize = 2147483647;
                httpBinding.ReceiveTimeout = new TimeSpan(0, 0, 10);
                httpBinding.UseDefaultWebProxy = false;

                EndpointIdentity ei = null;
                ei = EndpointIdentity.CreateDnsIdentity(serverIP);
                var endpointAddress = new EndpointAddress(new Uri(endPoint), ei);
                Program.DataServiceClient = new DataServiceClient(httpBinding, endpointAddress);
                if (Program.DataServiceClient == null) return false;
                return true;
            }
            catch (Exception)
            {
                //Program.logger.LogInfo("InitService异常：" + ex.Message);
                return false;
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
