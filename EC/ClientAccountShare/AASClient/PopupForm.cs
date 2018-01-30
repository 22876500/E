using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class PopupForm : Form
    {
        public static int PopupFormCount = 0;

        AASClient.AccountDataSet.价格提示Row 价格提示Row;
        int x;
        int y;
        public PopupForm(AASClient.AccountDataSet.价格提示Row 价格提示Row1, int x1, int y1)
        {
            InitializeComponent();

            this.价格提示Row = 价格提示Row1;
            this.x = x1;
            this.y = y1;

            PopupFormCount++;
        }

        private void PopupForm_Load(object sender, EventArgs e)
        {
            this.label内容.Text = this.价格提示Row.Description;

            if (this.价格提示Row.提示等级 == (int)提示等级.黄)
            {
                this.BackColor = Color.Yellow;
            }
            else if (this.价格提示Row.提示等级 == (int)提示等级.红)
            {
                this.BackColor = Color.Red;
            }


            Tool.SetWindowPos(this.Handle, Tool.HWND_TOPMOST, this.x, this.y, this.Width, this.Height, Tool.SWP_NOACTIVATE);//设置弹出位置
            Tool.AnimateWindow(this.Handle, 500, Tool.AW_BLEND);//动态显示本窗体

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Tool.AnimateWindow(this.Handle, 500, Tool.AW_SLIDE + Tool.AW_VER_POSITIVE + Tool.AW_HIDE);//动画隐藏窗体
            this.Close();
        }

        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }

        private void PopupForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PopupFormCount--;
        }
    }
}
