using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Core.Common.Core
{
    public abstract class ObjectBase : INotifyPropertyChanged 
    {
        private event PropertyChangedEventHandler _PropertyChanged;
        List<PropertyChangedEventHandler> PropertyChandedEventSubscribers
            = new List<PropertyChangedEventHandler>();

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if(!PropertyChandedEventSubscribers.Contains(value))
                {
                    _PropertyChanged += value;
                    PropertyChandedEventSubscribers.Add(value);
                }
            }
            remove
            {
                _PropertyChanged -= value;
                PropertyChandedEventSubscribers.Remove(value);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnProertyChanged(propertyName, true);
        }

        protected virtual void OnProertyChanged(string propertyName, bool makeDirty)
        {
            if (_PropertyChanged != null)
                _PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            
            if (makeDirty)
                _IsDirty = true;
        }

        bool _IsDirty;
        public bool IsDirty
        {
            get { return _IsDirty; }
        }
    }
}
