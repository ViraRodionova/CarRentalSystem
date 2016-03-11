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
using FluentValidation;
using FluentValidation.Results;
using System.Runtime.Serialization;
using System.ComponentModel.Composition.Hosting;

namespace Core.Common.Core
{
    public abstract class ObjectBase : INotifyPropertyChanged, IDataErrorInfo, IExtensibleDataObject
    {
        public ObjectBase()
        {
            _Validator = GetValidator();
            Validate();
        }

        protected bool _IsDirty;
        protected IValidator _Validator = null;

        protected IEnumerable<ValidationFailure> _ValidationErrors = null;

        public static CompositionContainer Container { get; set; }

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion

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

        #region OnPropertyChanged Notifications

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

            Validate();
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName, makeDirty);
        }

        #endregion

        #region DirtyObjects
        //[NotNavigable]
        public bool IsDirty
        {
            get { return _IsDirty; }
            set { _IsDirty = value; }
        }

        public List<ObjectBase> GetDirtyObjects()
        {
            List<ObjectBase> dirtyObjects = new List<ObjectBase>();

            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                    dirtyObjects.Add(o);
                return false;
            }, coll => {});

            return dirtyObjects;
        }

        public void CleanAll()
        {
            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                    o.IsDirty = false;
                return false;
            }, coll => { });
        }

        public virtual bool IsAnythingDirty()
        {
            bool IsDirty = false;

            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                {
                    IsDirty = true;
                    return true; //short circut
                }
                else 
                    return false;
            }, coll => { });

            return IsDirty;
        }
        #endregion

        #region WalkObjectGraph Method
        protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject,
                                       Action<IList> snippetForCollection,
                                       params string[] exemptProperties)
        {
            List<ObjectBase> visited = new List<ObjectBase>();
            Action<ObjectBase> walk = null;

            List<string> exemptions = new List<string>();
            if (exemptProperties != null)
                exemptions = exemptProperties.ToList();

            //делегат, к которому прикреплена рекурсивная лямбда-функция. Проверка объектов на изменение
            walk = (o) =>
            {
                if (o != null && !visited.Contains(o))
                {
                    visited.Add(o);

                    bool exitWalk = snippetForObject.Invoke(o);

                    if (!exitWalk)
                    {
                        //!!!! in example: PropertyInfo[] properties = o.GetBrowsableProperties(); 
                        PropertyInfo[] properties = o.GetType().GetProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            if (!exemptions.Contains(property.Name))
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
                                        snippetForCollection.Invoke(coll);

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
                }
            };

            walk(this);
        }
        #endregion

        #region Validation

        protected virtual IValidator GetValidator()
        {
            return null;
        }

        //[NotNavigable]
        public IEnumerable<ValidationFailure> ValidationErrors
        {
            get { return _ValidationErrors; }
            set { }
        }

        public void Validate()
        {
            if (_Validator != null)
            {
                ValidationResult results = _Validator.Validate(this);
                _ValidationErrors = results.Errors;
            }
        }

        //[NotNavigable]
        public bool IsValid
        {
            get
            {
                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                    return false;
                else return true;
            }
        }

        #endregion

        #region IDataErrorInfo Members
        string IDataErrorInfo.Error
        {
            get { return string.Empty; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                StringBuilder errors = new StringBuilder();

                if (_ValidationErrors != null && _ValidationErrors.Count() > 0)
                {
                    foreach (ValidationFailure validationError in _ValidationErrors)
                    {
                        if (validationError.PropertyName == columnName)
                            errors.AppendLine(validationError.ErrorMessage);
                    }
                }

                return errors.ToString();
            }
        }
        #endregion

    }
}
