using OKCoilClientWF.OKCoin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OKCoilClientWF
{
    public partial class fmMain : Form
    {
        public fmMain()
        {
            InitializeComponent();
            this.Load += fmMain_Load;
            this.FormClosed += fmMain_FormClosed;
        }

        void fmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            OKCoin.OKCoilAdapter.Instance.Stop();
        }

        void fmMain_Load(object sender, EventArgs e)
        {
            OKCoin.OKCoilAdapter.Instance.Start();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            OKCoinPrices obj = null;
            if (OKCoin.OKCoilAdapter.Instance.QueuePrices.TryDequeue( out obj))
            {
                this.labelQuarterBuy.Text = String.Format("{0:0.00}", obj.QuarterBuy);
                this.labelQuarterSell.Text = String.Format("{0:0.00}", obj.QuaterSale);
                this.labelThisWeekBuy.Text = String.Format("{0:0.00}", obj.ThisWeekBuy);
                this.labelThisWeekSell.Text = String.Format("{0:0.00}", obj.ThisWeekSale);
                this.labelNextWeekBuy.Text = String.Format("{0:0.00}", obj.NextWeekBuy);
                this.labelNextWeekSale.Text = String.Format("{0:0.00}", obj.NextWeekSale);

                this.labelOpenDiff.Text = Math.Round(obj.NextWeekBuy - obj.ThisWeekSale) + "";
                this.labelCloseDiff.Text = Math.Round(obj.NextWeekSale - obj.ThisWeekBuy) + "";
                //this.labelNextWeekDiffOpen.Text = obj.NextWeekPriceOpen.ToString();
                //this.labelNextWeekDiffClose.Text = obj.NextWeekPriceClose.ToString();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            //{
            //    OKCoilAdapter.Instance.ChangeChannel(WebSocketApi.WSChannelChangeType.Remove, WebSocketApi.WSChannel.NextWeekChannel);
            //    OKCoilAdapter.Instance.ChangeChannel(WebSocketApi.WSChannelChangeType.Add, WebSocketApi.WSChannel.ThisWeekChannel);
            //}
            //else
            //{
            //    OKCoilAdapter.Instance.ChangeChannel(WebSocketApi.WSChannelChangeType.Remove, WebSocketApi.WSChannel.ThisWeekChannel);
            //    OKCoilAdapter.Instance.ChangeChannel(WebSocketApi.WSChannelChangeType.Add, WebSocketApi.WSChannel.NextWeekChannel);
            //}
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
