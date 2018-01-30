using System;
using System.ComponentModel;
using System.Reflection;

namespace AASTrader.Model
{
    [Serializable]
    public class AbstractModel : INotifyPropertyChanged
    {
        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        protected void OnPropertyChangedAll()
        {
            PropertyInfo[] props = this.GetType().GetProperties();
            if (props != null && props.Length > 0)
            {
                foreach (var p in props)
                {
                    OnPropertyChanged(p.Name);
                }
            }
        }
    }
}
