﻿using CarRental.Business.Entities;
using CarRental.Data.Contracts.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Data.DataRepositories
{
    [Export(typeof(ICarRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class CarRepository : DataRepositoryBase<Car>, ICarRepository
    {
        protected override Car AddEntity(CarRentalContext entityContext, Car entity)
        {
            return entityContext.CarSet.Add(entity);
        }

        protected override Car UpdateEntity(CarRentalContext entityContext, Car entity)
        {
            return (from e in entityContext.CarSet
                    where e.CarId == entity.CarId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Car> GetEntities(CarRentalContext entityContext)
        {
            return from e in entityContext.CarSet
                   select e;
        }

        protected override Car GetEntity(CarRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.CarSet
                         where e.CarId == id
                         select e);
            var results = query.FirstOrDefault();
            
            return results;
        }
    }
}
