using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GroupService
{
    public class Adapter
    {
        public Adapter()
        {

        }

        public static List<GroupLogonInfo> GroupLogonList { get; set; }

        public void Start()
        {
            GroupLogonList = new List<GroupLogonInfo>();
            foreach (var item in GroupsDict.Values)
            {
                var o = new GroupLogonInfo() { Name = item.名称 };
                GroupLogonList.Add(o);

                if (item.Safe启用)
                {
                    item.ClientID = -1;
                    item.Start();
                }
                else
                {
                    if (item.Status.Length > 0)
                    {
                        o.Times = item.Status;
                    }
                    o.CanUse = false;
                }
            }
        }

        private static Dictionary<string, 券商> _dictGroups;
        public static Dictionary<string, 券商> GroupsDict
        {
            get
            {
                if (_dictGroups == null)
                {
                    StringBuilder builder = new StringBuilder(32);
                    _dictGroups = new Dictionary<string, 券商>();
                    foreach (string item in CommonUtils.ConfigurationInstance.AppSettings.Settings.AllKeys)
                    {

                        if (Regex.IsMatch(item, "^[A-Z][0-9]{2}$"))
                        {
                            try
                            {
                                var configData = Cryptor.MD5Decrypt(CommonUtils.ConfigurationInstance.AppSettings.Settings[item].Value);
                                if (configData.Contains("Safe启用"))
                                {
                                    var entity = configData.FromJson<券商>();

                                    _dictGroups.Add(item, entity);

                                    var group = new GroupInfo(entity);
                                    CommonUtils.SetConfig(item, Cryptor.MD5Encrypt(group.ToJson()));
                                }
                                else
                                {
                                    var group = configData.FromJson<GroupInfo>();
                                    var entity = new 券商(group);
                                    _dictGroups.Add(item, entity);
                                }
                            }
                            catch (Exception ex)
                            {
                                //builder.AppendFormat("组合号{0}解析失败！错误详情见log文件\n", item);
                                CommonUtils.Log("从配置文件解析组合号失败, 组合号：" + item, ex);
                                var o = new 券商() { 名称 = item, 启用 = false, };
                                o.SetStatus("组合号解析失败，请尝试删除后重新添加！");
                                _dictGroups.Add(item, o);
                            }
                        }
                    }

                }
                return _dictGroups;
            }
        }

        public static bool UpdateGroup(券商 o)
        {
            try
            {
                var groupJson = (new GroupInfo(o)).ToJson();
                CommonUtils.SetConfig(o.名称, Cryptor.MD5Encrypt(groupJson));

                if (o.ClientID == -1 && o.启用)
                {
                    o.Start();
                }
            }
            catch (Exception ex)
            {
                CommonUtils.Log("新增或编辑组合号出错", ex);
                return false;
            }
            return true;
        }

        public void Stop()
        {
            foreach (var item in GroupsDict.Values)
            {
                item.Stop();
            };
        }
    }
}