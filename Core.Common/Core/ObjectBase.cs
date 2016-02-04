using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.Practices.Prism.Mvvm;

namespace Core.Common.Core
{
    public abstract class ObjectBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

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

        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName, true);
        }

        protected virtual void OnPropertyChanged(string propertyName, bool makeDirty)
        {
            if (_PropertyChanged != null)
                _PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            
            if (makeDirty)
                _IsDirty = true;
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName);
        }

        bool _IsDirty;
        public bool IsDirty
        {
            get { return _IsDirty; }
        }
    }
}
