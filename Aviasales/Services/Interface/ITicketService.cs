using Aviasales.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviasales.Services.Interface
{
    public interface ITicketService
    {
        Task<Ticket> CreateTicket(string rateName, string routeName);
        IEnumerable<Ticket> Tickets { get; }
    }
}
