using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    enum ReadWriteMode { Read, Write }
    class ReadWriteLock:IDisposable
    {
        ReadWriteMode readWriteMode;
        ReaderWriterLockSlim readerWriterLockSlim;
        public ReadWriteLock(ReaderWriterLockSlim ReaderWriterLockSlim1, ReadWriteMode ReadWriteMode1)
        {
            this.readerWriterLockSlim = ReaderWriterLockSlim1;
            this.readWriteMode = ReadWriteMode1;



            if (this.readWriteMode == ReadWriteMode.Read)
            {
                this.readerWriterLockSlim.EnterReadLock();
            }
            else
            {
                this.readerWriterLockSlim.EnterWriteLock();
            }
        }


        public void Dispose()
        {
            if (this.readWriteMode == ReadWriteMode.Read)
            {
                this.readerWriterLockSlim.ExitReadLock();
            }
            else
            {
                this.readerWriterLockSlim.ExitWriteLock();
            }
        }
    }
}
