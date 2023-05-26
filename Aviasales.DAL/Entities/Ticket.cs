using Aviasales.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviasales.DAL.Entities
{
    public class Ticket : NamedEntity
    { 
        public virtual Route Route { get; set; }
        public virtual Rate Rate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }
    }
}
