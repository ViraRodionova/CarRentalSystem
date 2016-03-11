using CarRental.Business.Entities;
using CarRental.Data.Contracts.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.DataRepositories
{
    [Export(typeof(IRentalRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class RentalRepository : DataRepositoryBase<Rental>, IRentalRepository
    {
        protected override Rental AddEntity(CarRentalContext entityContext, Rental entity)
        {
            return entityContext.RentalSet.Add(entity);
        }

        protected override Rental UpdateEntity(CarRentalContext entityContext, Rental entity)
        {
            return (from e in entityContext.RentalSet
                    where e.RentalId == entity.RentalId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Rental> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.RentalSet
                   select e;
        }

        protected override Rental GetEntity(CarRentalContext entityContext, int id)
        {
            return (from e in entityContext.RentalSet
                    where e.RentalId == id
                    select e).FirstOrDefault();
        }

        public IEnumerable<Rental> GetRentalHistoryByCar(int carId)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = from e in entityContext.RentalSet
                            where e.CarId == carId
                            select e;
                //return query.ToFullyLoaded();
                return query.ToArray();
            }
        }

        public Rental GetCurrentRentalByCar(int carId)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = from e in entityContext.RentalSet
                            where e.CarId == carId && e.DataReturned == null
                            select e;
                return query.FirstOrDefault();
            }
        }

        public IEnumerable<Rental> GetCurrentlyRentadCars()
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = from e in entityContext.RentalSet
                            where e.DataReturned == null
                            select e;
                //return query.ToFullyLoaded();
                return query.ToArray();
            }
        }

        public IEnumerable<Rental> GetRentalHistoryByAccount(int accountId)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = from e in entityContext.RentalSet
                            where e.AccountId == accountId
                            select e;
                //return query.ToFullyLoaded();
                return query.ToArray();
            }
        }
    }
}
