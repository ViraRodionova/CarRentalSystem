using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CarRental.Business.Entities;
using CarRental.Data.Contracts;
using CarRental.Data.Contracts.RepositoryInterfaces;

namespace CarRental.Data.DataRepositories
{
    [Export(typeof(IReservationRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class ReservationRepository : DataRepositoryBase<Reservation>, IReservationRepository
    {
        protected override Reservation AddEntity(CarRentalContext entityContext, Reservation entity)
        {
            return entityContext.ReservationSet.Add(entity);
        }

        protected override Reservation UpdateEntity(CarRentalContext entityContext, Reservation entity)
        {
            return (from e in entityContext.ReservationSet
                    where e.ReservationId == entity.ReservationId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Reservation> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.ReservationSet
                   select e;
        }

        protected override Reservation GetEntity(CarRentalContext entityContext, int id)
        {
            return (from e in entityContext.ReservationSet
                    where e.ReservationId == id
                    select e).FirstOrDefault();
        }

        public IEnumerable<Reservation> GetReservationByPickupDate(DateTime pickupDate)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = from r in entityContext.ReservationSet
                            where r.RentalDate < pickupDate
                            select r;

                //return query.ToFullyLoaded();
                return query.ToArray();
            }
        }
    }
}
