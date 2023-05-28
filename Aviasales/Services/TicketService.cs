using Aviasales.DAL.Entities;
using Aviasales.Interfaces;
using Aviasales.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviasales.Services
{
    class TicketService : ITicketService
    {
        private readonly IRepository<Rate> _Rates;
        private readonly IRepository<Route> _Routes;
        private readonly IRepository<Ticket> _Tickets;

        public IEnumerable<Ticket> Tickets => _Tickets.Items;

        public TicketService(
            IRepository<Rate> Rates, 
            IRepository<Route> Routes,
            IRepository<Ticket> Tickets)
        {
            _Rates = Rates;
            _Routes = Routes;
            _Tickets = Tickets;
        }

        public async Task<Ticket> CreateTicket(string rateName, string routeName)
        {
            var route = await _Routes.Items.FirstOrDefaultAsync(r => r.Name == routeName).ConfigureAwait(false);
            if (route is null) return null;

            var rate = await _Rates.Items.FirstOrDefaultAsync(r => r.Name == rateName).ConfigureAwait(false);
            if (rate is null) return null;

            var ticket = new Ticket
            {
                Name = routeName,
                Rate = rate,
                Route = route,
                Cost = route.Distance * rate.Multiplier * 10
            };

            return await _Tickets.AddAsync(ticket);
            
        }

    }
}
