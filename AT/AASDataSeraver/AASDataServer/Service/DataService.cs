using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using AASDataServer.Model;
using AASDataServer.DataAdapter;
using System.Timers;
using DataServerIce;
using AASDataServer.Helper;
using DataModel;
using AASDataServer.Manager;

namespace AASDataServer.Service
{
    /// <summary>
    /// 基本数据服务
    /// </summary>
    public class DataService : IDataService
    {
        private IDataAdapter _dataAdapter;
        private Dictionary<string, SubCode> _codes;
        private Dictionary<string, UserClient> _clients;
        private Timer _timer;

        /// <summary>
        /// 订阅股票列表
        /// </summary>
        public Dictionary<string, SubCode> Codes
        {
            get { return _codes; }
        }

        /// <summary>
        /// 客户端列表
        /// </summary>
        public Dictionary<string, UserClient> Clients
        {
            get { return _clients; }
        }

        public IDataAdapter DataSource
        {
            get { return _dataAdapter; }
            set
            {
                _dataAdapter = value;
            }
        }

        public DataService()
        {
            _codes = new Dictionary<string, SubCode>();
            _clients = new Dictionary<string, UserClient>();

            _timer = new Timer(5 * 60 * 1000);   //5分钟
            _timer.Elapsed += _timer_Elapsed;
            _timer.Enabled = true;
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_dataAdapter != null && _dataAdapter.IsRunning == true && !_dataAdapter.IsAllCodes)
            {
                List<string> overdues = new List<string>();
                foreach (KeyValuePair<string, SubCode> kv in _codes)
                {
                    TimeSpan ts = DateTime.Now - kv.Value.FlushTime;
                    if (ts.TotalMinutes > 5)
                    {
                        overdues.Add(kv.Key);
                    }
                }
                if (_dataAdapter.IsRegistVip && _dataAdapter.IsVIPRegisted)
                {
                    overdues = overdues.Except(StockCodeManager.GetInstance.VIPCodes).ToList();
                }
                if (overdues.Count > 0)
                {
                    UnsubscribeCodes("", overdues.ToArray());
                    App.Logger.Info("DataService:定时取消订阅"+overdues.ToString());
                }
            }
        }

        public int SubscribeCodes(string username, string[] codelist)
        {
            if (_dataAdapter != null)
            {
                List<string> subcodes = new List<string>();
                for (int i=0; i < codelist.Length; i++)
                {
                    string code = codelist[i];
                    if (_codes.ContainsKey(code))
                    {
                        //更新代码时间
                        SubCode sc = _codes[code];
                        sc.Flush(username);
                    }
                    else 
                    {
                        //添加代码
                        if (StockCodeManager.GetInstance.Codes.ContainsKey(code))
                        {
                            //有效代码
                            SubCode sc = new SubCode();
                            sc.Code = code;
                            sc.Name = StockCodeManager.GetInstance.GetStockName(code);
                            sc.SubTime = DateTime.Now;
                            sc.FlushTime = DateTime.Now;
                            sc.Users.Add(username);

                            _codes.Add(code, sc);
                            subcodes.Add(code);
                        }
                    }
                }
                if (_clients.ContainsKey(username))
                {
                    //为用户添加订阅代码
                    _clients[username].AddSubCodes(codelist.ToList<string>());
                }
                else 
                {
                    //添加用户信息
                    UserClient uc = new UserClient();
                    uc.Username = username;
                    uc.AddSubCodes(codelist.ToList<string>());
                    _clients.Add(username, uc);
                }

                if (subcodes.Count > 0)
                {
                    //添加数据源的订阅代码
                    _dataAdapter.RegisterCodes(subcodes);
                    App.Logger.Info(string.Format("DataService:用户{0}添加订阅,订阅列表：{1}", username, string.Join(",", subcodes)));
                }
                
                return subcodes.Count;
            }

            return 0;
        }

        public int UnsubscribeCodes(string username, string[] codelist)
        {
            if (_dataAdapter != null)
            {
                List<string> codes = new List<string>();
                for (int i = 0; i < codelist.Length; i++)
                {
                    string code = codelist[i];
                    if (_codes.ContainsKey(code))
                    {
                        SubCode sc = _codes[code];
                        if (sc.Users.Contains(username))
                        {
                            sc.Users.Remove(username);
                            if (sc.Users.Count == 0)
                            {
                                //取消订阅代码
                                _codes.Remove(code);
                                codes.Add(code);
                            }
                        }

                        if (username == "")
                        {
                            //取消订阅代码
                            _codes.Remove(code);
                            codes.Add(code);
                        }
                    }
                }
                if (_clients.ContainsKey(username))
                {
                    //客户端取消订阅
                    _clients[username].RemoveSubCodes(codelist.ToList<string>());
                    if (_clients[username].SubCount == 0)
                    {
                        _clients.Remove(username);
                    }
                }
                
                if (username == "")
                { 
                   //定时器取消长时间不使用的代码时清除用户引用
                    List<string> deluc = new List<string>();
                    foreach (UserClient uc in _clients.Values)
                    {
                        uc.RemoveSubCodes(codelist.ToList<string>());
                        if (uc.SubCount == 0)
                        {
                            deluc.Add(uc.Username);
                        }
                    }

                    foreach (string du in deluc)
                    {
                        if (_clients.ContainsKey(du))
                        {
                            _clients.Remove(du);
                        }
                    }
                }

                if (codes.Count > 0)
                {
                    _dataAdapter.DeRegisterCodes(codes);
                    App.Logger.Info(string.Format("DataService:{0}取消订阅,{1}", username, codes.ToString()));
                }
                return codes.Count;
            }

            return 0;
        }

        public void FlushCodes(string username, string[] codelist)
        {
            if (_dataAdapter != null)
            {
                //需考虑服务器重连后的客户端刷新
                SubscribeCodes(username, codelist);
            }
        }

        public int GetStockCodes(out List<StockCode> codes)
        {
            codes = StockCodeManager.GetInstance.CodeList;
            return codes.Count;
        }

        public string GetStockCodesInfo()
        {
            var codes = StockCodeManager.GetInstance.CodeList;
            return codes.ToJSON();
        }

        public string GetVipCodes()
        {
            var vipcodes = StockCodeManager.GetInstance.VIPCodes;
            return vipcodes.ToJSON();
        }

        public bool SetSubType(string username, int type)
        {
            try
            {
                string configName = "sub-type";
                if (Helper.AppSettingsHelper.getString(configName) != type.ToString())
                {
                    var codesConfigAdd = StockCodeManager.GetInstance.GetAddList(type);
                    codesConfigAdd = codesConfigAdd.Except(StockCodeManager.GetInstance.VIPCodes).ToList();//排除重复项

                    var codesUnsub = Codes.Keys.Where(_ => !StockCodeManager.GetInstance.VIPCodes.Contains(_) && !codesConfigAdd.Contains(_)).ToList();

                    _dataAdapter.DeRegisterCodes(codesUnsub);
                    _dataAdapter.RegisterCodes(codesConfigAdd);

                    Helper.AppSettingsHelper.SetConfig("sub-type", type.ToString());
                }
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetSubType()
        {
            return (int)StockCodeManager.GetInstance.SubCodesType;
        }

        public void ClearAll()
        {
            _codes.Clear();
            _clients.Clear();
        }

    }
}
