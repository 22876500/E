using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;




namespace AASClient
{
    public partial class AccountDataSet
    {


        public string FileName;

        public void Load()
        {
            this.FileName = Program.Current平台用户.用户名 + "\\account.xml";

            if (File.Exists(this.FileName))
            {
                this.ReadXml(this.FileName);
            }
            else
            {
                this.WriteXml(this.FileName, XmlWriteMode.WriteSchema);
            }

            this.参数.RowChanged += new DataRowChangeEventHandler(DataSetChanged);
            this.参数.RowDeleted += new DataRowChangeEventHandler(DataSetChanged);

            this.快捷键.RowChanged += new DataRowChangeEventHandler(DataSetChanged);
            this.快捷键.RowDeleted += new DataRowChangeEventHandler(DataSetChanged);


            this.价格提示.RowChanged += new DataRowChangeEventHandler(DataSetChanged);
            this.价格提示.RowDeleted += new DataRowChangeEventHandler(DataSetChanged);
        }

        void DataSetChanged(object sender, DataRowChangeEventArgs e)
        {
            this.WriteXml(this.FileName, XmlWriteMode.WriteSchema);
        }

        public partial class 参数DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();


            参数Row FindPara(string ParaName)
            {
                foreach (参数Row 参数Row1 in this)
                {
                    if (参数Row1.项 == ParaName)
                    {
                        return 参数Row1;
                    }
                }

                return null;
            }

            public string GetParaValue(string ParaName, string DefaultValue)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    参数Row 参数Row1 = this.FindPara(ParaName);
                    if (参数Row1 == null)
                    {
                        return DefaultValue;
                    }
                    else
                    {
                        return 参数Row1.值;
                    }
                }
            }

            public void SetParaValue(string ParaName, string ParaValue)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    参数Row 参数Row1 = this.FindPara(ParaName);
                    if (参数Row1 == null)
                    {
                        参数Row1 = this.New参数Row();
                        参数Row1.项 = ParaName;
                        参数Row1.值 = ParaValue;
                        this.Add参数Row(参数Row1);
                    }
                    else
                    {
                        参数Row1.值 = ParaValue;
                    }
                }
            }


            public void DeletePara(string ParaName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    参数Row 参数Row1 = this.FindPara(ParaName);
                    if (参数Row1 != null)
                    {
                        this.Remove参数Row(参数Row1);
                    }
                }
            }
        }


        partial class 价格提示Row
        {
            public string Description
            {
                get
                {
                    return string.Format("{0} {1} {2}{3}元", this.证券代码, this.证券名称, ((提示类型)this.提示类型).ToString(), this.提示价格);
                }
            }
        }
    }
}
