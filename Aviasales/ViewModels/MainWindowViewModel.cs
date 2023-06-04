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
using static MathCore.Values.CSV;

namespace Aviasales.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        Ticket _ticket = new Ticket();
        string _companyName;
        string _cost;
        string _number;
        string _count;
        string _endpointName;

        private readonly IRepository<Ticket> _TicketsRepository;
        private readonly IRepository<Rate> _RatesRepository;
        private readonly IRepository<Route> _RoutesRepository;
        private readonly ITicketService _TicketServicce;

        private ViewModel _CurrentViewModel;

        private ICommand _ShowTicketViewCommand;
        private ICommand _ShowRouteViewCommand;
        private ICommand _ShowMaxTicketCost;
        private ICommand _ShowCountEndPointCommand;


        public string Company
        {
            set { _companyName = value; }
            get { return _companyName; }
        }
        public string Number 
        { 
            set 
            { 
                _number = value;
                OnPropertyChanged(nameof(Number));
            }
            get 
            { 
                return _ticket.Name;

            }
        }
        public string Cost
        {
            set 
            { 
                _cost = value;
                OnPropertyChanged(nameof(Cost));
            }
            get
            {
                return _ticket.Cost.ToString();
            }
        }

        public string Count
        {
            set { _count = value; OnPropertyChanged(nameof(Count)); }
            get { return _count; }
        }
        public string CountEndPoint
        {
            set { _endpointName = value; }
            get { return _endpointName; }
        }

        public ViewModel CurrentViewModel { get => _CurrentViewModel; private set => Set(ref _CurrentViewModel, value); }

        #region commands;
        public ICommand ShowMaxTicketCostCommand => _ShowMaxTicketCost 
            ??= new LambdaCommand(OnShowMaxTicketCostCommandExecuted, CanShowMaxTicketCostCommandExecute);

        private bool CanShowMaxTicketCostCommandExecute(object? arg)
        {
           return true;
        }

        private void OnShowMaxTicketCostCommandExecuted(object? obj)
        {
            var id = FindTicketIdMaxCost(_TicketsRepository, _companyName);
            _ticket = _TicketsRepository.Items.FirstOrDefault(t => t.ID == id);
            Company = _ticket.Route.Name;
            Cost = _ticket.Cost.ToString();
            Number = _ticket.Name;
        }

        public ICommand ShowTicketViewCommand => _ShowTicketViewCommand 
            ??= new LambdaCommand(OnShowTicketViewCommandExecuted, CanShowTicketViewCommandExecute);

        private void OnShowTicketViewCommandExecuted(object? obj)
        {
            CurrentViewModel = new TicketsViewModel(_TicketsRepository, _RoutesRepository, _RatesRepository);
        }

        private bool CanShowTicketViewCommandExecute(object parameter) => true;

        public ICommand ShowRouteViewCommand => _ShowRouteViewCommand
            ??= new LambdaCommand(OnShowRouteViewCommandExecuted, CanShowRoutemmandExecute);

        private void OnShowRouteViewCommandExecuted(object? obj)
        {
            CurrentViewModel = new RoutsViewModel(_RoutesRepository);
        }

        public ICommand ShowCountEndPointCommand => _ShowCountEndPointCommand
            ??= new LambdaCommand(OnShowCountEndPointCommand, CanShowCountEndPointCommand);

        private bool CanShowCountEndPointCommand(object? arg) => true;

        private void OnShowCountEndPointCommand(object? obj)
        {
            var count = FindCountRndPointCost(_TicketsRepository, _endpointName);
            Count = count.ToString();
        }

        private bool CanShowRoutemmandExecute(object? arg) => true;
        #endregion

        public MainWindowViewModel(IRepository<Route> RoutesRepository,
            IRepository<Rate> RatesRepository,
            IRepository<Ticket> TicketsRepository,
            ITicketService TicketService)
        {
            _RatesRepository = RatesRepository;
            _RoutesRepository = RoutesRepository;
            _TicketsRepository = TicketsRepository;
           
        }

        private int FindTicketIdMaxCost(IRepository<Ticket> TicketRepository, string company)
        {
            var tickets = TicketRepository.Items.Where(p=>p.Route.Name == company).ToArray();
            Ticket ticket = tickets[0];

            for (var i = 0; i < tickets.Length; i++)
                ticket = tickets[i].Cost > ticket.Cost ? tickets[i] : ticket;
            return ticket.ID;
        }
        private int FindCountRndPointCost(IRepository<Ticket> TicketRepository, string endpoint)
        {
            var tickets = TicketRepository.Items.Where(p => p.Route.EndPoint == endpoint).ToArray();
            return tickets.Count();
        }
    }
}
