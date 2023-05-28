using Aviasales.Services.Interface;
using Aviasales.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviasales.Services
{
    static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddTransient<ITicketService ,TicketService>()
            .AddSingleton<MainWindowViewModel>()
            ;


    }
}
