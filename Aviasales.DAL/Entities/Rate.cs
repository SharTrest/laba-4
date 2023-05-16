using Aviasales.DAL.Entities.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aviasales.DAL.Entities
{
    public class Rate : NamedEntity
    {
        [Column(TypeName = "decimal(18,2)")]
        public decimal Multiplier { get; set; }
    }
}
