using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class AppConfig: IDisposable
    {
        Configuration ExeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

        public bool Exists(string Name)
        {
            using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
            {
                return this.ExeConfiguration.AppSettings.Settings.AllKeys.Contains(Name);
            }
        }
        public void Delete(string Name)
        {
            using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
            {
                if (this.ExeConfiguration.AppSettings.Settings.AllKeys.Contains(Name))
                {
                    this.ExeConfiguration.AppSettings.Settings.Remove(Name);
                    this.ExeConfiguration.Save(ConfigurationSaveMode.Modified);
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                }
            }
        }
        public void SetValue(string Name, string Value)
        {
            using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
            {
                if (this.ExeConfiguration.AppSettings.Settings.AllKeys.Contains(Name))
                {
                    this.ExeConfiguration.AppSettings.Settings[Name].Value = Value;
                }
                else
                {
                    this.ExeConfiguration.AppSettings.Settings.Add(Name, Value);
                }

                this.ExeConfiguration.Save(ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            }
        }

        public string GetValue(string Name, string DefaultValue)
        {
            using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
            {
                if (this.ExeConfiguration.AppSettings.Settings.AllKeys.Contains(Name))
                {
                    return this.ExeConfiguration.AppSettings.Settings[Name].Value;
                }
                else
                {
                    return DefaultValue;
                }
            }
        }

        public void Dispose()
        {
            if (readerWriterLockSlim != null)
            {
                readerWriterLockSlim.Dispose();
            }
        }
    }
}
