using Aviasales.DAL.Context;
using Aviasales.DAL.Entities;
using MathCore.PE.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviasales.Data
{
    class DbInitial
    {
        private readonly AviasalesDB _db;
        private readonly ILogger _logger;
        public DbInitial(AviasalesDB db, ILogger<DbInitial> Logger)
        {
            _db = db;
            _logger = Logger;
        }

        public async Task InitialAsync()
        {
            var timer = Stopwatch.StartNew();

            _logger.LogInformation("Инициализация БД");
            _logger.LogInformation("Удаление БД");

            await _db.Database.EnsureDeletedAsync().ConfigureAwait(false);


            _logger.LogInformation("Удаление выполнено за {0} мс", timer.ElapsedMilliseconds);

            _logger.LogInformation("Миграция БД");
            await _db.Database.MigrateAsync();
            _logger.LogInformation("Миграция БД выполнена за {0} мс", timer.ElapsedMilliseconds);


            if (await _db.Routes.AnyAsync()) return;

            await InitialRoutes();
            await InitilRates();
            await InitialTickets();

            _logger.LogInformation("Миграция БД выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

        private Rate[] _Rates;
        private const int _RatesCount = 3;

        private async Task InitilRates()
        { 
            _Rates = new Rate[_RatesCount];
            for (var i = 0; i < _RatesCount; i++)
                _Rates[i] = new Rate
                {
                    Name = $"Место категории {i}",
                    Multiplier = (decimal)i
                };
            await _db.Rates.AddRangeAsync(_Rates);
            await _db.SaveChangesAsync();
        }

        private const int _RoutesCount = 10;
        private Route[] _Routes;

        private async Task InitialRoutes()
        {
            _Routes = new Route[_RoutesCount];
            for (var i = 0; i < _RoutesCount; i++)
                _Routes[i] = new Route
                {
                    Name = $"40{i}",
                    StartPoint = $"Населенный пункт{i}",
                    EndPoint = $"Населенный пункт {i + 10}",
                    Distance = i * 100

                };

            await _db.Routes.AddRangeAsync(_Routes);
            await _db.SaveChangesAsync();
        }

        private const int _TicketCount = 5;
        private Ticket[] _Tickets;

        private async Task InitialTickets()
        {
            var random = new Random();
            _Tickets = Enumerable.Range(1, _TicketCount)
                .Select(i => new Ticket
                {
                    Name = $"Маршрут {i}",
                    Rate = random.NextItem(_Rates),
                    Route = random.NextItem(_Routes),
                    Cost = 10000
                })
                .ToArray();
            await _db.Tickets.AddRangeAsync(_Tickets);
            await _db.SaveChangesAsync();
        }
    }
}
