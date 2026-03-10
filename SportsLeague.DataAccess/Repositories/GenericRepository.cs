using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : AuditBase
    {
        protected readonly LeagueDbContext _context;//Ambos son solo Lectura, sirven para inyectar la 
                                                    //Dependencia que necesito de GenericRepository
        protected readonly DbSet<T> _dbSet; //Este aparte  de ser lectura me sirve para manipular las tablas

        public GenericRepository(LeagueDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();//Me ayuda a definir la entidad especígica en la cual voy a hacer el crud
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();//Retrna la lista de la vrible T que puede ser Player, Team, etc.
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = null;
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync(); //Este método SaveChangesAsync () es crucial para que los cambios se reflejen en la base de datos.
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }
    }
}