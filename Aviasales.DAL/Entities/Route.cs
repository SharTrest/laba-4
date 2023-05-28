using Aviasales.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aviasales.DAL.Entities
{
    public class Route : NamedEntity
    {
        public string StartPoint { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }

        public string EndPoint { get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal Distance { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}

