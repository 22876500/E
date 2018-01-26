using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TradeService
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnLogon_Click(object sender, EventArgs e)
        {
            bool isLogon = CommonUtils.GetConfig("user") == Cryptor.MD5Encrypt(txtUserName.Value) && CommonUtils.GetConfig("psw") == Cryptor.MD5Encrypt(txtPsw.Value);
            if (isLogon)
            {
                Session["username"] = txtUserName.Value;
                Response.Redirect("Manage.aspx");
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('用户名或密码错误！');</script>");
            }
        }
    }
}