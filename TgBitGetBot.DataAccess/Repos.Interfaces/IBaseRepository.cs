using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TgBitGetBot.DataAccess.Repos.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
	public Task<EntityEntry<TEntity>> AddAsync(TEntity entity);
	public Task<TEntity> GetByIdAsync(Guid id);
	public Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate);
	public Task<IEnumerable<TEntity>> GetAllAsync();
	public Task<EntityEntry<TEntity>> DeleteByIdAsync(Guid id);
	public Task UpdateAsync(TEntity entity);
	public IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);
	public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
	public IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties);
	public Task<IEnumerable<TEntity>> GetWithIncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties);
	public Task<IEnumerable<TEntity>> GetWithIncludeAsync(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
	public Task<IQueryable<TEntity>> IncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties);
}