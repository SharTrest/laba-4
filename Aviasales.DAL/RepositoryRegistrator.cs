using Aviasales.DAL.Entities;
using Aviasales.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviasales.DAL
{
    public static class RepositoryRegistrator
    {
        public static IServiceCollection AddRepositoriesInDB(this IServiceCollection services) => services
            .AddTransient<IRepository<Ticket>,TicketsRepository>()
            .AddTransient<IRepository<Rate>, DbRepository<Rate>>()
            .AddTransient<IRepository<Route>, DbRepository<Route>>()
            ;
    }
}
