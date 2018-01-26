using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DataGridViewAutoFilter
{
   
    public partial class DataGridViewAutoFilterColumnHeaderCell : DataGridViewColumnHeaderCell
    {
        static int CurrentColumnIndex = -1;
        public static Dictionary<int, List<string>> ValueListForFilter = new Dictionary<int, List<string>>();
        static CheckedListBox checkedListBox = new CheckedListBox();
        static ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

        static DataGridViewAutoFilterColumnHeaderCell()
        {
            ToolStripItem ToolStripItem1 = contextMenuStrip.Items.Add("清空");
            ToolStripItem1.Click += ToolStripItem1_Click;
            checkedListBox.ContextMenuStrip = contextMenuStrip;
            checkedListBox.CheckOnClick = true;
        }

        static void ToolStripItem1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, false);
            }
        }
        public DataGridViewAutoFilterColumnHeaderCell()
            : base()
        {
          
        }

      

     

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);


            Rectangle Rectangle1 = this.DataGridView.GetCellDisplayRectangle(this.ColumnIndex, -1, false);

            int y = 3;
            Rectangle Rectangle2 = new Rectangle(Rectangle1.Right - 40, y, Rectangle1.Height - 2 * y, Rectangle1.Height - 2 * y);
            //ButtonRenderer.DrawButton(graphics, Rectangle2,"滤",new Font("微软雅黑",8),false, PushButtonState.Default);

            ComboBoxRenderer.DrawDropDownButton(graphics, Rectangle2, ComboBoxState.Normal   );


         
           
        }


        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            Rectangle Rectangle1 = this.DataGridView.GetCellDisplayRectangle(this.ColumnIndex, -1, false);

            int x = Rectangle1.Right - 40;
            int y = 3;
            int width=Rectangle1.Height - 2 * y;

            Rectangle Rectangle2 = new Rectangle(x, y, width, width);



            if (Rectangle2.Contains(e.X + Rectangle1.Left, e.Y + Rectangle1.Top))
            {
                if (!this.DataGridView.Controls.Contains(checkedListBox))
                {
                    BindingSource BindingSource1 = this.DataGridView.DataSource as BindingSource;
                    DataTable DataTable1 = BindingSource1.DataSource as DataTable;



                    List<string> ItemList = new List<string>();

                    for (int i = 0; i < DataTable1.Rows.Count; i++)
                    {
                        string string1 = DataTable1.Rows[i][this.ColumnIndex] + "";

                        if (!ItemList.Contains(string1))
                        {
                            ItemList.Add(string1);
                        }
                    }
                    ItemList.Sort();


                    if (!ValueListForFilter.ContainsKey(this.ColumnIndex))
                    {
                        ValueListForFilter[this.ColumnIndex] = new List<string>();
                    }

                    CurrentColumnIndex = this.ColumnIndex;

                    int ListBoxHeight = 0;
                   
                    checkedListBox.Items.Clear();
                  
                    foreach (string string1 in ItemList)
                    {
                        int int1 = checkedListBox.Items.Add(string1);
                        if (ValueListForFilter[this.ColumnIndex].Contains(string1))
                        {
                            checkedListBox.SetItemChecked(int1, true);
                        }
                        else
                        {
                            checkedListBox.SetItemChecked(int1, false);
                        }

                        if (ListBoxHeight < 200)
                        {
                            ListBoxHeight += 25;
                        }
                    }


                    checkedListBox.Bounds = new Rectangle(Rectangle1.X, Rectangle1.Bottom, Rectangle1.Width, ListBoxHeight);


                    checkedListBox.ItemCheck += checkedListBox_ItemCheck;
                    checkedListBox.LostFocus += checkedListBox_LostFocus;

                    this.DataGridView.Controls.Add(checkedListBox);

                    checkedListBox.Focus();

                }


                //this.DataGridView.InvalidateCell(this);

                //this.过滤ButtonClicked(this.ColumnIndex);
            }
            else
            {
                ListSortDirection direction = ListSortDirection.Ascending;
                if (this.DataGridView.SortedColumn == OwningColumn && this.DataGridView.SortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                this.DataGridView.Sort(OwningColumn, direction);
            }

            base.OnMouseDown(e);
        }

      

       

        void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                ValueListForFilter[CurrentColumnIndex].Add(checkedListBox.Items[e.Index] as string);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                ValueListForFilter[CurrentColumnIndex].Remove(checkedListBox.Items[e.Index] as string);
            }

            Dictionary<int, string> FilterString = new Dictionary<int, string>();
            foreach(int Key in ValueListForFilter.Keys)
            {
                if (ValueListForFilter[Key].Count > 0)
                {
                    List<string> Cell = ValueListForFilter[Key];
                    string string1 = string.Format("{0} in (", this.DataGridView.Columns[Key].HeaderText);

                    for (int i = 0; i < Cell.Count; i++)
                    {
                        string1 += string.Format("'{0}'", Cell[i]);

                        if (i < Cell.Count - 1)
                        {
                            string1 += ",";
                        }
                    }

                    string1 += ")";

                    FilterString[Key] = string1;
                }
            }

            string[] FilterStringArray = FilterString.Values.ToArray();

            string FilterString1=string.Empty;
            for(int i=0;i<FilterStringArray.Length;i++)
            {
                FilterString1 += FilterStringArray[i];

                if (i<FilterStringArray.Length-1)
                {
                    FilterString1 += " and ";
                }
            }


            BindingSource BindingSource1 = this.DataGridView.DataSource as BindingSource;

            if (FilterString1 != string.Empty)
            {
                BindingSource1.Filter = FilterString1;
            }
            else
            {
                BindingSource1.Filter = null;
            }
        }


        void checkedListBox_LostFocus(object sender, EventArgs e)
        {
            if (this.DataGridView.Controls.Contains(checkedListBox))
            {

                checkedListBox.ItemCheck -= checkedListBox_ItemCheck;
                checkedListBox.LostFocus -= checkedListBox_LostFocus;

                this.DataGridView.Controls.Remove(checkedListBox);
            }
        }
    }
}
