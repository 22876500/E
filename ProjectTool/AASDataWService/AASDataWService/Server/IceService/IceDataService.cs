using AASDataWService.Common;
using AASDataWService.Logger;
using DataServerIce;
using Ice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;


namespace AASDataWService.Server.IceService
{
    class IceDataService: DataServantDisp_
    {
        private DataServantPrx _dataServant;
        private Ice.Communicator _ic;
        //DataService _dataService;

        //private DataService GetDataService()
        //{
        //    if (!(_dataService is DataService))
        //    {
        //        _dataService = UnityContainerHost.Container.Resolve<IDataService>() as DataService;
        //    }
        //    return _dataService;
        //}

        public override int SubscribeCodes(string username, string[] codelist, Ice.Current current__)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null && codelist.Length > 0)
                {
                    int retval = prx.SubscribeCodes(username, codelist);
                    LogHelper.Instance.Info("AASDataWService：" + username + " 已订阅股票：" + string.Join(",", codelist));
                    return retval;
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Instance.Info("AASDataWService：SubscribeCodes调用失败\n" + ex.Message);
            }
            return 0;
        }

        public override int UnsubscribeCodes(string username, string[] codelist, Ice.Current current__)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    int retval = prx.UnsubscribeCodes(username, codelist);
                    return retval;
                }
            }
            catch (Ice.Exception ex)
            {
                LogHelper.Instance.Info("AASDataWService：UnsubscribeCodes调用失败\n" + ex.Message);
            }
            return 0;
        }

        public override void FlushCodes(string username, string[] codelist, Ice.Current current__)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    if (codelist.Length > 0)
                    {
                        prx.FlushCodes(username, codelist);
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Instance.Info("AASDataWService：FlushCodes调用失败\n" + ex.Message);
            }
        }

        public override int GetStockCodes(out DSIceStockCode[] codes, Ice.Current current__)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    DSIceStockCode[] arr;
                    var re = prx.GetStockCodes(out arr);
                    LogHelper.Instance.Info("AASDataWService：获取全市场股票代码成功，股票总数：" + arr.Length);
                    codes = arr;
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Instance.Info("AASDataWService：GetStockCodes调用失败\r\n  Message:" + ex.Message);
            }
            codes = null;
            return 0;
        }

        public override string GetVipCodes(Ice.Current current__)
        {
            try
            {
                LogHelper.Instance.Info("GetVipCodes");
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    var re = prx.GetVipCodes();
                   // var codes = re.ListFromJSON<string>();
                    LogHelper.Instance.Info("AASDataWService：已获取订阅列表，股票总数：" + re.Length);
                    //return codes;
                    return re;
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Instance.Info("AASDataWService：GetVipCodes调用失败\r\n  Message:" + ex.Message);
            }
            return null;
        }

        public override string GetStockCodesInfo(Ice.Current current__)
        {
            //DataService ds = GetDataService();
            //if (ds is DataService)
            //{
            //    DSIceStockCode[] codes = null;
            //    List<StockCode> mcd;
            //    int count = ds.GetStockCodes(out mcd);
            //    App.Logger.Info(string.Format("ICE订阅服务：获取股票定义{0}", count));
            //    if (count > 0 && mcd != null)
            //    {
            //        codes = new DSIceStockCode[count];
            //        for (int i = 0; i < count; i++)
            //        {
            //            codes[i] = mcd[i].GetDSIceStockCode();
            //        }
            //    }
            //    else
            //    {
            //        codes = null;
            //    }
            //    //string codesInfo = ds.GetStockCodesInfo();
            //    App.Logger.Info("ICE订阅服务：获取股票定义");
            //    return codes == null ? null : Helper.JsonHelper.ToJSON(codes);


            //    //return codesInfo;
            //}

            //App.Logger.Error("DataService对象不存在！");

            return null;
        }

        public override bool SetSubType(string username, int subType, Ice.Current current__)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    var re = prx.SetSubType(username, subType);
                    LogHelper.Instance.Info("AASDataWService：设置订阅模式成功");
                    return re;
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Instance.Info("AASDataWService：设置订阅模式失败！异常信息：" + ex.Message);
            }
            return false;
        }

        public override int GetSubType(Ice.Current current__)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    var re = prx.GetSubType();
                    LogHelper.Instance.Info("AASDataWService：获取当前订阅模式成功");
                    return re;
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Instance.Info("AASDataWService：获取当前订阅模式失败！异常信息：" + ex.Message);
            }
            return -1;
        }

        public override int GetPubType(Ice.Current current__)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    var re = prx.GetPubType();
                    LogHelper.Instance.Info("AASDataWService：获取当前pub模式成功");
                    return re;
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Instance.Info("AASDataWService：获取当前pub模式失败！异常信息：" + ex.Message);
            }
            return -1;
        }
        protected DataServantPrx GetDataServant()
        {
            try
            {
                lock (this)
                {
                    if (_dataServant != null)
                    {
                        _dataServant.ice_ping();
                        return _dataServant;
                    }

                    if (_ic == null)
                    {

                        InitializationData icData = new InitializationData();
                        Ice.Properties icProp = Util.createProperties();
                        icProp.setProperty("Ice.ACM.Client", "0");
                        icProp.setProperty("Ice.MessageSizeMax", "2097152");//2gb in kb
                        icData.properties = icProp;
                        Communicator ic = Util.initialize(icData);

                        if (ic != null)
                        {
                            _ic = ic;
                        }
                    }

                    if (_ic != null)
                    {
                        string endpoint = string.Format("AASDataServer/DataServant:tcp -h {0} -p {1}", ConfigMain.GetConfigValue(BaseCommon.CONFIGIP), ConfigMain.GetConfigValue(BaseCommon.CONFIGICEPORT));
                        Ice.ObjectPrx obj = _ic.stringToProxy(endpoint);
                        DataServantPrx client = DataServantPrxHelper.checkedCast(obj);
                        if (client == null)
                        {
                            LogHelper.Instance.Info("DataServerClient：无法获得有效数据服务器接口");
                            return null;
                        }

                        client.ice_ping();
                        _dataServant = client;
                        return _dataServant;
                    }
                }
            }
            catch (Ice.ConnectionRefusedException)
            {
                //计算机积极拒绝，认为未部署鱼头，不进行记录。
                //Program.logger.LogRunning("DataServerClient：数据服务器连接失败\r\n  {0}", ex.InnerException.Message);
            }
            catch (System.Exception ex)
            {
                _dataServant = null;
                LogHelper.Instance.Info("DataServerClient：数据服务器连接失败" + ex.Message);
            }

            return null;
        }
    }
}
