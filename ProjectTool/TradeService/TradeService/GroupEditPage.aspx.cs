using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradeService.Models;

namespace TradeService
{
    public partial class GroupEditPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Request.Params.AllKeys.Contains("name"))
                {
                    var group = this.Request.Params["name"];
                    InitPage(group);
                }
            }
        }

        private void InitPage(string groupName)
        {
            var config = CommonUtils.GetConfig(groupName);
            var o = Cryptor.MD5Decrypt(config).FromJson<券商>();
            this.cbIsUse.Checked = o.启用;
            this.txtDepartment.Text = o.营业部代码.ToString();
            this.txtIP.Text = o.IP;
            this.txtLogAccount.Text = o.登录帐号;
            this.txtName.Text = groupName;
            this.txtPort.Text = o.Port.ToString();
            this.txtServerInfo.Text = o.交易服务器;
            this.txtTradeAccount.Text = o.交易帐号;
            this.txtVersion.Text = o.版本号;
            
            this.txtCommunicatePsw.Attributes.Add("value", o.CommunicatePsw);
            this.txtTradePsw.Attributes.Add("value", o.TradePsw);
            //this.txtTradePsw.Text = o.TradePsw;
            //this.txtCommunicatePsw.Text = o.CommunicatePsw;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            券商 Group = null;
            if (string.IsNullOrEmpty(this.Request.Params["name"]) && WebConfigurationManager.AppSettings.AllKeys.Contains(txtName.Text.Trim()))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('已存在该组合号！');</script>");
                return;
            }
            else
            {
                Group = new 券商();
            }

            UpdateGroup(Group);

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script>alert('修改完毕！'); window.location.url='Manage.aspx'</script>");
            //Response.Redirect("Manage.aspx");
            Server.Transfer("~/Manage.aspx");
        }

        private void UpdateGroup(券商 Group)
        {
            try
            {
                var matchItem = Regex.Match(txtServerInfo.Text, "([0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}):([0-9]+)");
                if (matchItem.Success && string.IsNullOrEmpty(txtIP.Text) || string.IsNullOrEmpty(txtPort.Text))
                {
                    txtIP.Text = matchItem.Groups[1].Value;
                    txtPort.Text = matchItem.Groups[2].Value;
                }
                Group.IP = txtIP.Text.Trim();
                Group.Port = short.Parse(txtPort.Text.Trim());
                Group.版本号 = txtVersion.Text.Trim();
                Group.登录帐号 = txtLogAccount.Text.Trim();
                Group.交易服务器 = txtServerInfo.Text;
                Group.交易密码 = Cryptor.MD5Encrypt(txtTradePsw.Text);
                Group.交易帐号 = txtTradeAccount.Text.Trim();
                Group.名称 = txtName.Text.Trim();
                Group.启用 = cbIsUse.Checked;
                Group.通讯密码 = Cryptor.MD5Encrypt(txtCommunicatePsw.Text);
                Group.营业部代码 = short.Parse(txtDepartment.Text.Trim());
                CommonUtils.SetConfig(Group.名称, Cryptor.MD5Encrypt(Group.ToJson()));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", string.Format("<script>alert('修改异常,{0}！');</script>", ex.Message));
            }
            
        }
    }
}