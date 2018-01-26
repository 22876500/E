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
    public partial class Manage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitPage();
            }
        }

        private void InitPage()
        {
            List<券商> lstGroups = new List<券商>();
            foreach (string item in WebConfigurationManager.AppSettings.AllKeys)
            {
                if (Regex.IsMatch(item, "^[A-Z][0-9]{2}$"))
                {
                    try
                    {
                        var o = Cryptor.MD5Decrypt(WebConfigurationManager.AppSettings[item]).FromJson<券商>();
                        if (o != null)
                        {
                            lstGroups.Add(o);
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonUtils.Log(item + " decrypt error :" + ex.Message);
                    }
                }
            }
            rptMain.DataSource = lstGroups;
            rptMain.DataBind();
        }

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            //var s =this.form1.Controls
            var btn = sender as Button;
            string group = btn.ToolTip;
            var url = "GroupEditPage.aspx?name=" + group;
            Response.Redirect(url);
        }

        protected void BtnDel_Click(object sender, EventArgs e)
        { 
        
        }
    }
}
