using Aviasales.DAL.Entities.Base;
using Aviasales.Interfaces;
using Aviasales.DAL.Entities.Base;
using Aviasales.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Aviasales.DAL.Entities;

namespace Aviasales.DAL
{
    internal class DbRepository<T> : IRepository<T> where T : Entity, new()
    {
        private readonly AviasalesDB _db;
        private readonly DbSet<T> _Set;

        public bool AutoSaveChanges { get; set; } = true;

        public DbRepository(AviasalesDB db)
        {
            _db = db;
            _Set = db.Set<T>();
        }

        public virtual IQueryable<T> Items => _Set;


        public T Add(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges)
                _db.SaveChanges();
            return item;
        }

        public async Task<T> AddAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges)
                await _db.SaveChangesAsync().ConfigureAwait(false);
            return item;
        }

        public T Get(int id) => Items.SingleOrDefault(item => item.ID == id);

        public async Task<T> GetAsync(int id, CancellationToken Cancel = default) => await Items
            .SingleOrDefaultAsync(item => item.ID == id, Cancel)
            .ConfigureAwait(false)
            ;

        public void Update(T item)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges)
                _db.SaveChanges();
        }

        public async Task UpdateAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _db.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges)
                await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Remove(int id)
        {
            _db.Remove(new T { ID = id });

            if (AutoSaveChanges)
                _db.SaveChanges();
           
        }

        public async Task RemoveAsync(int id, CancellationToken Cancel = default)
        {
            _db.Remove(new T { ID = id });
            if (AutoSaveChanges)
                await _db.SaveChangesAsync().ConfigureAwait(false);
        }

    }

    class TicketsRepository : DbRepository<Ticket>
    {
        public override IQueryable<Ticket> Items => base.Items
            .Include(item => item.Rate)
            .Include(item => item.Route);
        public TicketsRepository(AviasalesDB db) : base(db) {}
    }
}
