using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Entities
{
    public class Rental : ObjectBase
    {
        int _ReservationId;
        int _AccountId;
        int _CarId;
        DateTime _DateRented;
        DateTime? _DateReturned;
        DateTime _DateDue;

        public int ReservationId
        {
            get { return _ReservationId; }
            set
            {
                if (_ReservationId != value)
                {
                    _ReservationId = value;
                    OnPropertyChanged(() => ReservationId);
                }
            }
        }

        public int AccountId
        {
            get { return _AccountId; }
            set
            {
                if (_AccountId != value)
                {
                    _AccountId = value;
                    OnPropertyChanged(() => AccountId);
                }
            }
        }

        public int CarId
        {
            get { return _CarId; }
            set
            {
                if (_CarId != value)
                {
                    _CarId = value;
                    OnPropertyChanged(() => CarId);
                }
            }
        }

        public DateTime DateRented
        {
            get { return _DateRented; }
            set
            {
                if (_DateRented != value)
                {
                    _DateRented = value;
                    OnPropertyChanged(() => DateRented);
                }
            }
        }

        public DateTime? DateReturned
        {
            get { return _DateReturned; }
            set
            {
                if (_DateReturned != value)
                {
                    _DateReturned = value;
                    OnPropertyChanged(() => DateReturned);
                }
            }
        }

        public DateTime DateDue
        {
            get { return _DateDue; }
            set
            {
                if (_DateDue != value)
                {
                    _DateDue = value;
                    OnPropertyChanged(() => DateDue);
                }
            }
        }
    }
}
