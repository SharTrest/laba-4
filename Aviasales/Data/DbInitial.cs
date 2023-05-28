using Aviasales.DAL.Context;
using Aviasales.DAL.Entities;
using MathCore.PE.Headers;
using MathCore.Statistic.RandomNumbers;
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

        private const int _RoutesCount = 100;
        private Route[] _Routes;

        private async Task InitialRoutes()
        {
            _Routes = new Route[_RoutesCount];
            var random = new Random();

            for (var i = 0; i < _RoutesCount; i++)
            {
                int companyName = random.Next(1,4);
                int point1 = 0;
                int point2 = 0;
                while (point1 == point2)
                {
                    point1 = random.Next(1, 10);
                    point2 = random.Next(1, 10);
                }
                _Routes[i] = new Route
                {
                    Name = ChoiceName(companyName),
                    StartPoint = ChoicePoint(point1),
                    EndPoint = ChoicePoint(point2),
                    Distance = 900 + i * 100,
                    Date = DateOnly.FromDateTime(RandomDay(random)).ToString(),
                    Time = RandomTime(random).ToString()
                };
            }

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
                    Name = $"User {i}",
                    Rate = random.NextItem(_Rates),
                    Route = random.NextItem(_Routes),
                    Cost = 10000,
                })
                .ToArray();
            await _db.Tickets.AddRangeAsync(_Tickets);
            await _db.SaveChangesAsync();
        }
        DateTime RandomDay(Random gen)
        {
            DateTime start = new DateTime(2024, 1, 1);
            int range = (start - DateTime.Today).Days;
            return start.AddDays(gen.Next(range));
        }

        TimeSpan RandomTime(Random rnd)
        {
            return TimeSpan.FromMinutes(rnd.Next(24 * 4) * 15);
        }

        string ChoiceName(int n)
        {
            switch (n)
            {
                case 1:
                    return "Победа";
                case 2:
                    return "Аэрофлот";
                case 3:
                    return "S7 Airlines";
                case 4:
                    return "Уральские авиалинии";
            }
            return "";
        }

        string ChoicePoint(int p)
        {
            switch (p)
            {
                case 1:
                    return "Москва";
                case 2:
                    return "Нижний Новгород";
                case 3:
                    return "Мурманск";
                case 4:
                    return "Курск";
                case 5:
                    return "Рязань";
                case 6:
                    return "Орск";
                case 7:
                    return "Санкт-Петербург";
                case 8:
                    return "Казань";
                case 9:
                    return "Хабаровск";
                case 10:
                    return "Кемерово";
            }
            return "";
        }
    }
}
