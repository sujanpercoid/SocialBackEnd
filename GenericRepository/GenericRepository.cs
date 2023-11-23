using Data;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly TcDbcontext _context;
        DbSet<T> _entity = null;

        public GenericRepository(TcDbcontext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }

        public async Task<string> Delete(object id)
        {
            if (id is int intId)
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<T>().Remove(entity);
                    await _context.SaveChangesAsync();
                    return "Deleted";
                }
            }

            return "Not Found"; // Or any other appropriate message.
        }

        public async Task<string> Add(T entity)
        {
            var add = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return "Added";
        }



    }
}