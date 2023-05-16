using Aviasales.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviasales.DAL.Context
{
    public class AviasalesDB : DbContext
    {
        public DbSet<Route> Routes { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Ticket> Tickets { get; set; }


        public AviasalesDB(DbContextOptions<AviasalesDB> options) :base(options)
        {
        }
    }
}
