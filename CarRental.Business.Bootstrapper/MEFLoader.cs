using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition.Hosting;
using CarRental.Data.DataRepositories;
using Core.Common.Contracts;
using Core.Common.Data;


namespace CarRental.Business.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            AggregateCatalog catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(AccountRepository).Assembly));


            CompositionContainer container = new CompositionContainer(catalog);

            return container;
        }
    }
}
