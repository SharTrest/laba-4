using Aviasales.Interfaces;

namespace Aviasales.DAL.Entities.Base
{
    public abstract class Entity : IEntity
    {
        public int ID { get; set; }
    }

}
