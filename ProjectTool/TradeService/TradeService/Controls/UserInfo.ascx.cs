using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TradeService.Controls
{
    public partial class UserInfo : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtPsw.Value == txtPswAgain.Value)
            {
                CommonUtils.SetConfig("user", Cryptor.MD5Encrypt(txtUserName.Value));
                CommonUtils.SetConfig("psw", Cryptor.MD5Encrypt(txtPsw.Value));
                this.Page.ClientScript.RegisterClientScriptBlock(GetType(), "Message", "<script>alert('修改完毕！');</script>");
            }
            //else
            //{
            //    this.Page.ClientScript.RegisterClientScriptBlock(GetType(), "Message", "<script>alert('修改完毕！');</script>");
            //}
        }
    }
}