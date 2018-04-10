using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServerIce;
using AASDataServer.Service;
using Microsoft.Practices.Unity;
using AASDataServer.Model;

namespace AASDataServer.Server.IceService
{
    class IceDataService : DataServantDisp_
    {
        DataService _dataService;

        private DataService GetDataService()
        {
            if (!(_dataService is DataService))
            {
                _dataService = UnityContainerHost.Container.Resolve<IDataService>() as DataService;
            }
            return _dataService;
        }

        public override int SubscribeCodes(string username, string[] codelist, Ice.Current current__)
        {
            DataService ds = GetDataService();
            if (ds is DataService)
            {
                App.Logger.Info(string.Format("ICE订阅服务：添加订阅 用户:{0}，订阅列表：{1}", username, string.Join(",",codelist)));
                return ds.SubscribeCodes(username, codelist);
            }

            App.Logger.Error("DataService对象不存在！");
            return 0;
        }

        public override int UnsubscribeCodes(string username, string[] codelist, Ice.Current current__)
        {
            DataService ds = GetDataService();
            if (ds is DataService)
            {
                App.Logger.Info(string.Format("ICE订阅服务：取消订阅 {0}={1}", username, codelist));
                return ds.UnsubscribeCodes(username, codelist);
            }

            App.Logger.Error("DataService对象不存在！");
            return 0;
        }

        public override void FlushCodes(string username, string[] codelist, Ice.Current current__)
        {
            DataService ds = GetDataService();
            if (ds is DataService)
            {
                App.Logger.Info(string.Format("ICE订阅服务：刷新订阅 username:{0}，订阅列表：{1}", username, string.Join(",",codelist)));
                ds.FlushCodes(username, codelist);
                return;
            }

            App.Logger.Error("DataService对象不存在！");
        }

        public override int GetStockCodes(out DSIceStockCode[] codes, Ice.Current current__)
        {
            DataService ds = GetDataService();
            if (ds is DataService)
            {
                List<StockCode> mcd;
                int count = ds.GetStockCodes(out mcd);
                App.Logger.Info(string.Format("ICE订阅服务：获取股票定义{0}", count));
                if (count > 0 && mcd != null)
                {
                    codes = new DSIceStockCode[count];
                    for (int i = 0; i < count; i++ )
                    {
                        codes[i] = mcd[i].GetDSIceStockCode();
                    }
                }
                else
                {
                    codes = null;
                }

                return count;
            }

            App.Logger.Error("DataService对象不存在！");
            codes = null;
            return 0;
        }

        public override string GetVipCodes(Ice.Current current__)
        {
            DataService ds = GetDataService();
            if (ds is DataService)
            {
                var codes = ds.GetVipCodes();
                App.Logger.Info("ICE订阅服务：获取可订阅股票列表");
                return codes;
            }

            App.Logger.Error("DataService对象不存在！");
            return null;
        }

        public override string GetStockCodesInfo(Ice.Current current__)
        {
            DataService ds = GetDataService();
            if (ds is DataService)
            {
                DSIceStockCode[] codes = null;
                List<StockCode> mcd;
                int count = ds.GetStockCodes(out mcd);
                App.Logger.Info(string.Format("ICE订阅服务：获取股票定义{0}", count));
                if (count > 0 && mcd != null)
                {
                    codes = new DSIceStockCode[count];
                    for (int i = 0; i < count; i++)
                    {
                        codes[i] = mcd[i].GetDSIceStockCode();
                    }
                }
                else
                {
                    codes = null;
                }
                //string codesInfo = ds.GetStockCodesInfo();
                App.Logger.Info("ICE订阅服务：获取股票定义");
                return codes == null ? null : Helper.JsonHelper.ToJSON(codes);
                
                
                //return codesInfo;
            }

            App.Logger.Error("DataService对象不存在！");
            
            return null;
        }

        public override bool SetSubType(string username,int subType, Ice.Current current__)
        {
            DataService ds = GetDataService();
            if (ds is DataService)
            {
                 ds.SetSubType(username, subType);
                 return true; 
            }

            App.Logger.Error("DataService对象不存在！");
            return false;
        }

        public override int GetSubType(Ice.Current current__)
        {
            DataService ds = GetDataService();
            if (ds is DataService)
            {
                return ds.GetSubType(); 
            }
            App.Logger.Error("DataService对象不存在！");
            return 0;
        }
    }
}
