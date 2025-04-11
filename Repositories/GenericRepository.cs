using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TheBookClub.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            try
            {
                _dbSet.Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while adding the entity to the database.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while deleting the entity from the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the entity.", ex);
            }
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    // Assuming the entity has a property called "IsDeleted" for soft deletion
                    var entry = _dbContext.Entry(entity);
                    entry.Property("IsDeleted").CurrentValue = true;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while deleting the entity.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the entity.", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var entities = await _dbSet.ToListAsync();
                return entities.AsEnumerable();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all entities.", ex);
            }
        }

        public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), "Condition expression cannot be null.");
            }

            try
            {
                var entities = await _dbSet.Where(expression).ToListAsync();
                return entities.AsEnumerable();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving entities by condition.", ex);
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                return entity ?? throw new KeyNotFoundException($"Entity with the specified ID ({id}) was not found.");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the entity with ID {id}.", ex);
            }
        }

        public async Task UpdateAsync( T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            try
            {
                var entry = _dbContext.Entry(entity);
                entry.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("A concurrency error occurred while updating the entity.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while updating the entity in the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the entity.", ex);
            }
        }
    }
}