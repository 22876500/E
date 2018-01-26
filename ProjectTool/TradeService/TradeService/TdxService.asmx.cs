using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace TradeService
{
    /// <summary>
    /// TdxService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class TdxService : System.Web.Services.WebService
    {
        public TdxService()
        {
            TdxApi.OpenTdx();
        }

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}



        protected override void Dispose(bool disposing)
        {
            TdxApi.CloseTdx();
            base.Dispose(disposing);
        }
    }
}
