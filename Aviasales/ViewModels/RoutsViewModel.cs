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
    class RoutsViewModel: ViewModel
    {
        private readonly IRepository<Route> _Routs;
        public RoutsViewModel(IRepository<Route> Routs)
        {
            _Routs = Routs;
        }

    }
}
