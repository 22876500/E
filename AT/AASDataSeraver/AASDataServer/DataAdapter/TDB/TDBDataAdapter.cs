using AASDataServer.Helper;
using AASDataServer.Model.Setting;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AASDataServer.DataAdapter.TDB
{
    class TDBDataAdapter : IDataAdapter
    {
        static object sync = new object();
        private bool _isRunning;
        private HHDataAdapterSetting _config;
        TDBDataSource tdbSource;
        List<MarketCode> _marketCodes;

        #region Properties
        Boolean? _isAllCodes = null;
        public bool IsAllCodes
        {
            get
            {
                if (_isAllCodes == null)
                {
                    lock (sync)
                    {
                        if (_isAllCodes == null)
                        {

                            _isAllCodes = string.Equals(AppSettingsHelper.getString("IsAllCodes"), "true", StringComparison.CurrentCultureIgnoreCase);
                        }
                    }
                }
                return _isAllCodes == true;
            }
        }

        Boolean? _isRegistVip = null;
        public bool IsRegistVip
        {
            get
            {
                if (_isRegistVip == null)
                {
                    lock (sync)
                    {
                        if (_isRegistVip == null)
                        {
                            _isRegistVip = string.Equals(AppSettingsHelper.getString("IsRegistVIP"), "true", StringComparison.CurrentCultureIgnoreCase);
                        }
                    }
                }
                return _isRegistVip == true;
            }
        }

        public bool IsVIPRegisted
        {
            get;
            set;
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
        }

        public long RecvDataCount
        {
            get { return 0; }
        }

        public long CacheCount
        {
            get { return 0; }
        }

        List<string> _codes;
        public List<string> Codes
        {
            get { return _codes; }
        }

        public Model.Setting.HHDataAdapterSetting Setting
        {
            get;
            set;
        } 
        #endregion

        public event Action<string> NewSysEvent;

        public event Action<DataModel.MarketData[]> NewMarketData;

        public event Action<DataModel.MarketTransaction[]> NewMarketTransction;

        public event Action<DataModel.MarketOrder[]> NewMarketOrder;

        public event Action<DataModel.MarketOrderQueue[]> NewMarketOrderQueue;

        public TDBDataAdapter()
        {
            _config = new HHDataAdapterSetting();
            
        }

        public int RegisterCodes(List<string> codes)
        {
            //throw new NotImplementedException();
            return 0;
        }

        public int DeRegisterCodes(List<string> codes)
        {
            //throw new NotImplementedException();
            return 0;
        }

        public void DeRegisterAll()
        {
            //throw new NotImplementedException();
        }

        public void ClearCache()
        {
            //throw new NotImplementedException();
        }

        public int GetCodeTable(out List<MarketCode> codes)
        {
            codes = new List<MarketCode>();
            if (_isRunning == true && tdbSource != null)
            {
                int retry = 0;
                while (this._marketCodes == null || this._marketCodes.Count == 0 || retry < 10)
                {
                    Thread.Sleep(1000);
                    retry++;
                }
                codes.AddRange(this._marketCodes);
            }
            return 0;
        }

        public int Start()
        {
            if (_isRunning == true)
            {
                App.Logger.Info("TDB数据服务：已启动！");
                return 1;
            }

            try
            {
                tdbSource = new TDBDataSource(_config.Ip, _config.Port.ToString(), _config.Username, _config.Password, 30, 1, 1);
                TDBLoginResult loginRes;
                TDBErrNo nErr = tdbSource.Connect(out loginRes);
                if (nErr == TDBErrNo.TDB_OPEN_FAILED)
                {
                    App.Logger.Error(string.Format("TDB数据服务：连接失败，错误代码{0}", nErr));
                }
                else
                {
                    App.Logger.Info("TDB数据服务：登陆成功！");
                    TDBCode[] codeArr1;
                    //输出全部市场的代码表
                    TDBErrNo nRetInner = tdbSource.GetCodeTable("", out codeArr1);
                    if (nRetInner == TDBErrNo.TDB_SUCCESS)
                    {
                        var mcList = new List<MarketCode>();
                        foreach (var code in codeArr1)
                        {
                            MarketCode mc = new MarketCode()
                            {
                                WindCode = code.m_strWindCode,
                                Market = code.m_strMarket,
                                Code = code.m_strCode,
                                EnName = code.m_strENName,
                                CnName = code.m_strCNName,
                                Type = code.m_nType
                            };
                            mcList.Add(mc);
                        }
                        _marketCodes.AddRange(mcList);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                App.Logger.Error(ex);
                App.Logger.Error("TDB数据服务：初始化错误！" + ex.Message);
                return 100;
            }

            return 0;
        }

        public int Stop()
        {
            if (tdbSource != null)
            {
                tdbSource.DisConnect();
                //tdbSource.Close();
                //tdbSource.ClearCache();
                _marketCodes.Clear();
                App.Logger.Warn("TDB数据服务：断开连接！");
                _isRunning = false;
                
            }
            return 0;
        }
    }
}
