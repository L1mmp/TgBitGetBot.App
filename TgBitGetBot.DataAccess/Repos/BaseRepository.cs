using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using TgBitGetBot.DataAccess.Repos.Interfaces;

namespace TgBitGetBot.DataAccess.Repos;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
	#region Readonly fields

	protected readonly ApplicationDbContext _dbContext;
	private readonly DbSet<TEntity> _dbSet;

	#endregion


	protected BaseRepository(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
		_dbSet = dbContext.Set<TEntity>();
	}

	public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
	{
		ArgumentNullException.ThrowIfNull(nameof(entity));

		var addedEntity = await _dbSet.AddAsync(entity);

		await _dbContext.SaveChangesAsync();

		_dbContext.Entry(entity).State = EntityState.Detached;

		return addedEntity;
	}

	public async Task<EntityEntry<TEntity>> DeleteByIdAsync(Guid id)
	{
		var deltedEntity = _dbSet.Remove(await GetByIdAsync(id));

		await _dbContext.SaveChangesAsync();

		return deltedEntity;
	}

	public async Task<IEnumerable<TEntity>> GetAllAsync()
	{
		return await _dbSet.AsNoTracking().ToListAsync();
	}

	public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate)
	{
		return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
	}

	public async Task<TEntity> GetByIdAsync(Guid id)
	{
		return (await _dbSet.FindAsync(id))!;
	}

	public async Task UpdateAsync(TEntity entity)
	{
		_dbContext.ChangeTracker.Clear();

		_dbContext.Entry<TEntity>(entity).State = EntityState.Modified;

		await _dbContext.SaveChangesAsync();
	}

	public IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
	{
		return Include(includeProperties).ToList();
	}

	public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
	{
		var query = Include(includeProperties);
		return query.Where(predicate).ToList();
	}

	public IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
	{
		IQueryable<TEntity> query = _dbSet.AsNoTracking();
		return includeProperties
			.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
	}

	public async Task<IEnumerable<TEntity>> GetWithIncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties)
	{
		return await (Include(includeProperties)).ToListAsync();
	}
}