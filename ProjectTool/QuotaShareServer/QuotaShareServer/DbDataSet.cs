using System.Data;
using System.Threading;
namespace QuotaShareServer {
    
    
    public partial class DbDataSet {
        partial class StockLimitDataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();
            public void Load()
            {

                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["Account"], this.Columns["StockID"] };
                    using (QSDBContext db = new QSDBContext())
                    {
                        foreach (var item in db.StockLimit)
                        {
                            var row = this.NewStockLimitRow();
                            row.Account = item.Account;
                            row.BuyType = (int)item.BuyType;
                            row.SaleType = (int)item.SaleType;
                            row.CommissionCharge = item.CommissionCharge;
                            row.Market = item.Market;
                            row.QtyCanUse = item.QtyCanUse;
                            row.StockID = item.StockID;
                            row.StockName = item.StockName;
                            this.AddStockLimitRow(row);
                        }
                    }
                    this.StockLimitRowChanging += StockLimitDataTable_StockLimitRowChanging;
                    this.StockLimitRowDeleting += StockLimitDataTable_StockLimitRowChanging;
                }
            }

            void StockLimitDataTable_StockLimitRowChanging(object sender, StockLimitRowChangeEvent e)
            {
                using (QSDBContext db = new QSDBContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            var stockLimit = new StockLimit();
                            stockLimit.Account = e.Row.Account;
                            stockLimit.BuyType = (BuySMode)e.Row.BuyType;
                            stockLimit.SaleType = (SaleMode)e.Row.SaleType;
                            stockLimit.CommissionCharge = e.Row.CommissionCharge;
                            stockLimit.Market = e.Row.Market;
                            stockLimit.QtyCanUse = e.Row.QtyCanUse;
                            stockLimit.StockID = e.Row.StockID;
                            stockLimit.StockName = e.Row.StockName;
                            db.StockLimit.Add(stockLimit);
                            break;
                        case DataRowAction.Change:
                            stockLimit = db.StockLimit.Find(e.Row.Account, e.Row.StockID);
                            stockLimit.Account = e.Row.Account;
                            stockLimit.BuyType = (BuySMode)e.Row.BuyType;
                            stockLimit.SaleType = (SaleMode)e.Row.SaleType;
                            stockLimit.CommissionCharge = e.Row.CommissionCharge;
                            stockLimit.Market = e.Row.Market;
                            stockLimit.QtyCanUse = e.Row.QtyCanUse;
                            stockLimit.StockID = e.Row.StockID;
                            stockLimit.StockName = e.Row.StockName;
                            db.Entry(stockLimit).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            break;
                        case DataRowAction.Delete:
                            stockLimit = db.StockLimit.Find(e.Row.Account, e.Row.StockID);
                            db.StockLimit.Remove(stockLimit);
                            db.SaveChanges();
                            break;
                        default:
                            break;
                    }
                }
            }

            public bool LockStockQty(string stockID)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    try
                    {
                        var limist = this.Where(_ => _.StockID == stockID && _.QtyCanUse > _.QtyUsed);
                        return true;
                    }
                    catch (System.Exception)
                    {
                        return false;
                    }
                    
                }
                
            }
        }

        partial class StockLimitRow
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();


        }
    }
}
