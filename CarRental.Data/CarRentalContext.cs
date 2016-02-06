using System;
using System.Data.Entity;
using CarRental.Business.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Runtime.Serialization;
using Core.Common.Contracts;

namespace CarRental.Data
{
    class CarRentalContext : DbContext
    {
        public CarRentalContext()
            : base("name=CarRental")
        {
            Database.SetInitializer<CarRentalContext>(null);
        }

        public DbSet<Account> AccountSet { get; set; }

        public DbSet<Car> CarSet { get; set; }

        public DbSet<Rental> RentalSet { get; set; }

        public DbSet<Reservation> ReservationSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /* 
             * Таблицы сущностей в БД могут называться лишь идентично самим сущностям!
             * Убираем создание множества названий таблиц (конвенцию)
             * AccountSet != Account != Accounts ...
             */
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            /* 
             * В БД нет колонки для ExtensionDataObject, но она есть в каждой сущности,
             * потому что все сущности унаследованы от EntityBase.
             * Также все сущности реализуют ин-с IIdentifiableEntity, для которого тоже нет колонки в БД.
             * 
             * Ин-с IAccountOwnedEntity реализован ЯВНО => его члены не воспринимаются как свойства класса.
             */
            modelBuilder.Ignore<ExtensionDataObject>();
            modelBuilder.Ignore<IIdentifiableEntity>();

            /*
             * Назначаем ключевые поля сущностей в таблицах БД 
             * 
             * + он говорит сделать так: modelBuilder.Entity<Account>().HasKey<int>(e => e.AccountId).Ignore(e => e.EntityId);
             * так как почему-то на него руганулась IDE, хотя он добавил исключение выше: modelBuilder.Ignore<IIdentifiableEntity>()
             */
            modelBuilder.Entity<Account>().HasKey<int>(e => e.AccountId);
            modelBuilder.Entity<Car>().HasKey<int>(e => e.CarId);
            modelBuilder.Entity<Rental>().HasKey<int>(e => e.RentalId);
            modelBuilder.Entity<Reservation>().HasKey<int>(e => e.ReservationId);

            // Этого свойства нет в БД. Оно используется только внутри программы для подсчетов
            modelBuilder.Entity<Car>().Ignore<bool>(e => e.CurrentlyRented);
        }
    }
}
