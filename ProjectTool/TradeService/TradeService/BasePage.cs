using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeService
{
    public class BasePage : System.Web.UI.Page
    {
        //BasePage()
        //{
            
        //}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                if (Session.Keys.Count == 0 || string.IsNullOrEmpty(Session["username"].ToString()))
                {
                    Response.Redirect("Login.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}