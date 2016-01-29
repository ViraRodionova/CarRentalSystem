using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Core;
using Core.Common.Contracts;

namespace CarRental.Business.Entities
{
    [DataContract]
    public class Resrvation : EntityBase, IIdentifiableEntity, IAccountOwnedEntity
    {
        [DataMember]
        public int ReservationId { get; set; }
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public int CarId { get; set; }
        [DataMember]
        public DateTime RentalDate { get; set; }
        [DataMember]
        public DateTime ReturnDate { get; set; }

        #region IIdentifiableEntity Members
        
        public int EntityId
        {
            get { return ReservationId; }
            set { ReservationId = value; }
        }

        #endregion
                
        #region IAccountOwnedEntity Members
        
        public int OwnerAccountId
        {
            get { return AccountId; }
        }

        #endregion
    }
}
