using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TheBookClub.Context;

namespace TheBookClub.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext dbContext)
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
                    // Check if the entity has an IsDeleted property
                    var isDeletedProperty = typeof(T).GetProperty("IsDeleted");
                    if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool))
                    {
                        // Set the IsDeleted property to true
                        isDeletedProperty.SetValue(entity, true);
                        await _dbContext.SaveChangesAsync();
                        return true;
                    }

                    throw new InvalidOperationException("The entity does not have an IsDeleted property.");
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
                var nonDeletedEntities = entities.Where(e => !(bool)e.GetType().GetProperty("IsDeleted").GetValue(e));

                return nonDeletedEntities.AsEnumerable();
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
                var nonDeletedEntities = entities.Where(e => !(bool)e.GetType().GetProperty("IsDeleted").GetValue(e));
                return nonDeletedEntities.AsEnumerable();
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
                var isDeletedProperty = typeof(T).GetProperty("IsDeleted");
                if (isDeletedProperty != null && isDeletedProperty.PropertyType == typeof(bool))
                {
                    if (entity != null && (bool)isDeletedProperty.GetValue(entity))
                    {
                        throw new InvalidOperationException("The entity has been deleted.");
                    }
                }
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