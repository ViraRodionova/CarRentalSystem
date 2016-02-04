using System;
using Core.Common.Core;
using System.Collections.Generic;

namespace Core.Common.Tests.TetsClasses
{
    class TestChild : ObjectBase
    {
        string _ChildName = string.Empty;

        public string ChildName
        {
            get { return _ChildName; }
            set
            {
                if (_ChildName == value)
                    return;

                _ChildName = value;
                OnPropertyChanged(() => ChildName);
            }
        }
    }
}
