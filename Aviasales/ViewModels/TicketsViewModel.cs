using Aviasales.DAL.Entities;
using Aviasales.Interfaces;
using MathCore.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviasales.ViewModels
{
    public class TicketsViewModel:ViewModel
    {
        public TicketsViewModel()
        {

        }
        Ticket _ticket = new Ticket();

        private readonly IRepository<Ticket> _TicketsRepository;
        private readonly IRepository<Rate> _RatesRepository;
        private readonly IRepository<Route> _RoutesRepository;

        public string Company => _ticket.Route.Name;
        public string Number => _ticket.Name;
        public string Cost => _ticket.Cost.ToString();

        public TicketsViewModel(IRepository<Ticket> TicketsRepository, IRepository<Route> RoutesRepository, IRepository<Rate> RatesRepository)
        {
            _TicketsRepository = TicketsRepository;
            _RoutesRepository = RoutesRepository;
            _RatesRepository = RatesRepository;

            var id = FindTicketIdMaxCost(_TicketsRepository);

            var ticket = _TicketsRepository.Items.FirstOrDefault(t=>t.ID == id);

        }

        private int FindTicketIdMaxCost(IRepository<Ticket> TicketRepository)
        {
            var tickets = TicketRepository.Items.ToArray();
            Ticket ticket = tickets[0];

            for (var i = 0; i < tickets.Length; i++)
                ticket = tickets[i].Cost > ticket.Cost ? tickets[i] : ticket ;
            return ticket.ID;
        }
    }
}
