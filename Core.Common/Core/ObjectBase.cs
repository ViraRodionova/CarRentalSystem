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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChaned(string propertyName)
        {
            OnProertyChanged(propertyName, true);
        }

        protected virtual void OnProertyChanged(string propertyName, bool makeDirty)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            
            if (makeDirty)
                _IsDirty = true;
        }

        bool _IsDirty;
        public bool IsDirty
        {
            get { return _IsDirty; }
        }

        //этот комментарий находится здесь для проверки работоспособнрости Git
    }
}
