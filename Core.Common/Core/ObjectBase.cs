using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.Practices.Prism.Mvvm;
using System.Reflection;
using System.Collections;

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

        protected List<ObjectBase> GetDirtyObjects()
        {
            List<ObjectBase> dirtyObjects = new List<ObjectBase>();

            List<ObjectBase> visited = new List<ObjectBase>();
            Action<ObjectBase> walk = null;

            walk = (o) =>
            {
                if (o != null && !visited.Contains(o))
                {
                    visited.Add(o);

                    if (o.IsDirty)
                        dirtyObjects.Add(o);

                    bool exitWalk = false;

                    if (!exitWalk)
                    {
                        //!!!! in example: PropertyInfo[] properties = o.GetBrowsableProperties(); 
                        PropertyInfo[] properties = o.GetType().GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            if (property.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                            {
                                ObjectBase obj = (ObjectBase)(property.GetValue(o, null));
                                walk(obj);
                            }
                            else
                            {
                                IList coll = property.GetValue(o, null) as IList;
                                if (coll != null)
                                {

                                    foreach (object item in coll)
                                    {
                                        if (item is ObjectBase)
                                            walk((ObjectBase)item);
                                    }
                                }
                            }
                        }
                    }
                }
            };

            walk(this);

            return dirtyObjects;
        }
    }
}
