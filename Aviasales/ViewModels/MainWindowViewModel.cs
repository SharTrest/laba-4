using Aviasales.DAL.Entities;
using Aviasales.Interfaces;
using Aviasales.Models;
using Aviasales.Services.Interface;
using Aviasales.Views;
using MathCore.WPF.Commands;
using MathCore.WPF.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Windows.Input;
using static MathCore.Values.CSV;

namespace Aviasales.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private ObservableCollection<TicketModel> _tickets = new ObservableCollection<TicketModel>();
        private ObservableCollection<CompanyModel> _companies = new ObservableCollection<CompanyModel>();
        private ObservableCollection<TicketsRatesModel> _ticketRates = new ObservableCollection<TicketsRatesModel>();

        Ticket _ticket = new Ticket();
        string _companyName;
        string _time;
        string _cost;
        string _number;
        string _count;
        string _endpointName;
        string _startointName;
        string _rateclass;

        private readonly IRepository<Ticket> _TicketsRepository;
        private readonly IRepository<Rate> _RatesRepository;
        private readonly IRepository<Route> _RoutesRepository;
        private readonly ITicketService _TicketServicce;

        private ViewModel _CurrentViewModel;

        private ICommand _ShowTicketViewCommand;
        private ICommand _ShowRouteViewCommand;
        private ICommand _ShowMaxTicketCost;
        private ICommand _ShowCountEndPointCommand;
        private ICommand _ShowTicketsBeforeCommand;
        private ICommand _ShowAviacompanyCommand;
        private ICommand _ShowTicketesByRateClassCommand;


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
        public string TicketNumber
        {
            get { return _number; }
            set { _number = value; OnPropertyChanged(); }
        }
        public string StartTime
        {
            get { return _time; }
            set { _time = value; OnPropertyChanged();}
        }
        public string StartPoint
        {
            get { return _startointName; }
            set { _startointName = value; OnPropertyChanged(); }
        }
        public string RateClass
        {
            get { return _rateclass; }
            set { _rateclass = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TicketModel> Tickets
        {
            get { return _tickets; }
            set { _tickets = value; OnPropertyChanged(); }
        }
        public ObservableCollection<CompanyModel> Companies
        {
            get { return _companies; }
            set { _companies = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TicketsRatesModel> TicketsRates
        {
            get { return _ticketRates; }
            set { _ticketRates = value; OnPropertyChanged(); }
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

        public ICommand ShowTicketsBeforeCommand => _ShowTicketsBeforeCommand
            ??= new LambdaCommand(OnShowTicketsBeforeCommand, CanShowTicketsBeforeCommand);

        private bool CanShowTicketsBeforeCommand(object? arg) => true;

        private void OnShowTicketsBeforeCommand(object? obj)
        {
            Tickets = FindTicketsBefore(_TicketsRepository, StartTime, _tickets);
        }

        public ICommand ShowAviacompanyCommand => _ShowAviacompanyCommand
            ??= new LambdaCommand(OnShowAviacompanyCommand, CanShowAviacompanyCommand);

        private bool CanShowAviacompanyCommand(object? arg) => true;

        private void OnShowAviacompanyCommand(object? obj)
        {
            Companies = FindCompanyFromStartPoint(_RoutesRepository, StartPoint, _companies);
        }

        public ICommand ShowTicketesByRateClassCommand => _ShowTicketesByRateClassCommand
            ??= new LambdaCommand(OnShowTicketesByRateClassCommand, CanShowTicketesByRateClassCommand);

        private bool CanShowTicketesByRateClassCommand(object? arg) => true;

        private void OnShowTicketesByRateClassCommand(object? obj)
        {
            FindTicketsByRates(_TicketsRepository, RateClass, TicketsRates);
        }


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
        private ObservableCollection<TicketModel> FindTicketsBefore(IRepository<Ticket> TicketRepository, string time, ObservableCollection<TicketModel> tickets)
        {
            tickets.Clear();
            var ts = TicketRepository.Items.ToArray();
            var _time = DateTime.ParseExact(time, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
            foreach (var t in ts)
            {
                DateTime dateTime = DateTime.ParseExact(t.Route.Time, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                if (_time >= dateTime)
                {
                    var tic = new TicketModel
                    {
                        TicketNumber = t.Name,
                        Time = t.Route.Time,
                    };
                    tickets.Add(tic);
                }
            }
             return tickets;
        }
        private ObservableCollection<CompanyModel> FindCompanyFromStartPoint(IRepository<Route> RouteRepository, string startpoint, ObservableCollection<CompanyModel> companies)
        {
            companies.Clear();
            var rs = RouteRepository.Items.Where(r=>r.StartPoint == startpoint).ToArray();
            foreach (var r in rs)
            {
                var k = 0;
                foreach (var company in companies)
                {
                    if (company.CompanyName != r.Name)
                        k++;
                    else k = -10000000;
                }
                if (k > 0 || companies.Count == 0)
                {
                    var cmp = new CompanyModel
                    {
                        CompanyName = r.Name,
                        StartPoint = r.StartPoint,
                    };
                    companies.Add(cmp);
                }
            }
            return companies;
        }
        private ObservableCollection<TicketsRatesModel> FindTicketsByRates(IRepository<Ticket> TicketRepository, string rate, ObservableCollection<TicketsRatesModel> ticketRates) 
        {
            var tickets = TicketRepository.Items.Where(t=>t.Rate.Name == rate).ToArray();
            foreach (var ticket in tickets)
            {
                TicketsRatesModel t = new TicketsRatesModel
                {
                    Name = ticket.Name
                };
                ticketRates.Add(t);
            }
            return ticketRates;
        }
    }
}
