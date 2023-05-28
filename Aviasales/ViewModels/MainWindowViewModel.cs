using Aviasales.DAL.Entities;
using Aviasales.Interfaces;
using Aviasales.Services.Interface;
using Aviasales.Views;
using MathCore.WPF.Commands;
using MathCore.WPF.ViewModels;
using System;
using System.Linq;
using System.Printing;
using System.Windows.Input;

namespace Aviasales.ViewModels
{
    public class MainWindowViewModel :ViewModel
    {
        private readonly IRepository<Ticket> _TicketsRepository;
        private readonly IRepository<Rate> _RatesRepository;
        private readonly IRepository<Route> _RoutesRepository;
        private readonly ITicketService _TicketServicce;

        private ViewModel _CurrentViewModel;

        private ICommand _ShowTicketViewCommand;
        private ICommand _ShowRouteViewCommand;



        public ViewModel CurrentViewModel { get => _CurrentViewModel; private set => Set(ref _CurrentViewModel, value); }

        public ICommand ShowTicketViewCommand => _ShowTicketViewCommand 
            ??= new LambdaCommand(OnShowTicketViewCommandExecuted, CanShowTicketViewCommandExecute);

        private void OnShowTicketViewCommandExecuted(object? obj)
        {
            CurrentViewModel = new TicketsViewModel();
        }

        private bool CanShowTicketViewCommandExecute(object parameter) => true;

        public ICommand ShowRouteViewCommand => _ShowRouteViewCommand
            ??= new LambdaCommand(OnShowRouteViewCommandExecuted, CanShowRoutemmandExecute);

        private void OnShowRouteViewCommandExecuted(object? obj)
        {
            CurrentViewModel = new RoutsViewModel(_RoutesRepository);
        }

        private bool CanShowRoutemmandExecute(object? arg) => true;

        //public MainWindowViewModel(IRepository<Route> RoutesRepository, 
        //    IRepository<Rate> RatesRepository, 
        //    IRepository<Ticket> TicketsRepository, 
        //    ITicketService TicketService)
        //{
        //    _RatesRepository = RatesRepository;
        //    _RoutesRepository = RoutesRepository;


        //    _TicketServicce = TicketService;

        //    var ticket_count = _TicketServicce.Tickets.Count();

        //    var route = _RoutesRepository.Get(3);
        //    var rate = _RatesRepository.Get(1);

        //    var ticket = TicketService.CreateTicket(rate.Name, route.Name);
        //    var ticket_count2 = _TicketServicce.Tickets.Count();



        //}
    }
}
