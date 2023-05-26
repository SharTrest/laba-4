using Aviasales.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviasales.Data
{
    static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration Configuration) => services
            .AddDbContext<AviasalesDB>(opt =>
            {
                var type = Configuration["Type"];
                switch (type)
                {
                    case "MSSQL":
                        opt.UseSqlServer(Configuration.GetConnectionString(type));
                        break;

                    case null: throw new ArgumentNullException("Тип БД не определен");
                        break;
                    default : throw new InvalidOperationException($"Данный тип:{type} не поддерживается");
                }

            }
            )
            .AddTransient<DbInitial>(); 
    }
}
