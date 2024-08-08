using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManangerSystem.IServices;
using TaskManangerSystem.Services.Tool;

namespace TaskManangerSystem.Services.Repository
{

    public interface IUpdateable<TTarget>
    {
        void Update(TTarget newData);
    }

    public class RepositoryAsync<TSource>(ManagementSystemContext storage) : IRepositoryAsync<TSource> where TSource : class
    {
        public DbSet<TSource> AsEntity { set; get; } = storage.Set<TSource>();

        public async Task<int> CountAsync(Expression<Func<TSource, bool>>? keySelector) => await (keySelector is null ? AsEntity.CountAsync() : AsEntity.Where(keySelector).CountAsync());
        public async Task<bool> ExistAsync(Expression<Func<TSource, bool>> keySelector) => await AsEntity.AnyAsync(keySelector);
        public async Task<TSource?> TryGetAsync(Guid id) => await AsEntity.FindAsync(id);
        public async Task<TSource?> TryGetAsync(Expression<Func<TSource, bool>> keySelector) => await AsEntity.FirstOrDefaultAsync(keySelector);
        public async Task<bool> AddAsync(TSource info) { await AsEntity.AddAsync(info); return await storage.SaveChangesAsync() == 1; }
        public async Task<bool> UpdateAsync(TSource info) { await Task.Run(() => { AsEntity.Update(info); }); return await storage.SaveChangesAsync() == 1; }
        public async Task<bool> DeleteAsync(Expression<Func<TSource, bool>> keySelector) { await Task.Run(() => AsEntity.Where(keySelector)); return await storage.SaveChangesAsync() == 1; }


        /// <summary>
        /// 通用复合查询
        /// </summary>
        /// <typeparam name="TKey">排序关键字段</typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="predicate">条件排序</param>
        /// <param name="keySelector">指定排序的字段</param>
        /// <param name="isAsc">排序方式</param>
        /// <returns></returns>
        public async Task<PageContent<TSource>?> SearchAsync<TKey>(Expression<Func<TSource, TKey>> keySelector, int pageIndex = 1, int pageSize = int.MaxValue, Expression<Func<TSource, bool>>? predicate = null, bool isAsc = true)
        {
            IQueryable<TSource> queryable = AsEntity.AsQueryable().AsNoTracking();
            if (predicate is not null)
                queryable = queryable.Where(predicate);

            int count = await queryable.CountAsync();

            IQueryable<TSource> query = isAsc ?
                    queryable.OrderBy(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize) :
                    queryable.OrderByDescending(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            int maxPage = (int)Math.Ceiling(count / (double)pageSize);

            return new PageContent<TSource>(index: pageIndex, max: maxPage, count: count, info: await query.ToListAsync());
        }

    }

    public class Repository<TSource>(ManagementSystemContext storage) : IRepository<TSource> where TSource : class
    {
        public DbSet<TSource> AsEntity { set; get; } = storage.Set<TSource>();

        public int Count(Expression<Func<TSource, bool>>? keySelector) => keySelector is null ? AsEntity.Count() : AsEntity.Where(keySelector).Count();
        public bool Exist(Expression<Func<TSource, bool>> keySelector) => AsEntity.Any(keySelector);
        public TSource? TryGet(Guid id) => AsEntity.Find(id);
        public TSource? TryGet(Expression<Func<TSource, bool>> keySelector) => AsEntity.FirstOrDefault(keySelector);
        public bool Add(TSource info) { AsEntity.Add(info); return storage.SaveChanges() == 1; }
        public bool Update(TSource info) { AsEntity.Update(info); ; return storage.SaveChanges() == 1; }
        public bool Delete(Expression<Func<TSource, bool>> keySelector) { AsEntity.Where(keySelector); return storage.SaveChanges() == 1; }

        public PageContent<TSource>? Search<TKey>(Expression<Func<TSource, TKey>> keySelector, int pageIndex = 1, int pageSize = int.MaxValue, Expression<Func<TSource, bool>>? predicate = null, bool isAsc = true)
        {
            IQueryable<TSource> queryable = AsEntity.AsQueryable().AsNoTracking();
            if (predicate is not null)
                queryable = queryable.Where(predicate);

            int count = queryable.Count();

            IQueryable<TSource> query = isAsc ?
                    queryable.OrderBy(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize) :
                    queryable.OrderByDescending(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            int maxPage = (int)Math.Ceiling(count / (double)pageSize);

            return new PageContent<TSource>(index: pageIndex, max: maxPage, count: count, info: [.. query]);
        }

    }

    public class IncludeRepositoryAsync<TSource>(ManagementSystemContext storage, Expression<Func<TSource, object?>>[] attribute) : RepositoryAsync<TSource>(storage) where TSource : class
    {
        public new async Task<TSource?> TryGetAsync(Expression<Func<TSource, bool>> keySelector)
        {
            IQueryable<TSource> queryable = AsEntity;
            foreach (var item in attribute) queryable = queryable.Include(item);

            return await queryable.FirstOrDefaultAsync(keySelector);

        }


        /// <summary>
        /// 失败的异步查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="predicate"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public new async Task<PageContent<TSource>?> SearchAsync<TKey>(Expression<Func<TSource, TKey>> keySelector, int pageIndex = 1, int pageSize = int.MaxValue, Expression<Func<TSource, bool>>? predicate = null, bool isAsc = true)
        {
            IQueryable<TSource> queryable = AsEntity;
            foreach (var item in attribute) queryable = queryable.Include(item);

            if (predicate is not null)
                queryable = queryable.Where(predicate);

            int count = await queryable.CountAsync();

            IQueryable<TSource> query = isAsc ?
                    queryable.OrderBy(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize) :
                    queryable.OrderByDescending(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            int maxPage = (int)Math.Ceiling(count / (double)pageSize);

            return new PageContent<TSource>(index: pageIndex, max: maxPage, count: count, info: [.. query]);
        }

    }

    public class IncludeRepository<TSource>(ManagementSystemContext storage, params Expression<Func<TSource, object?>>[] attribute) : Repository<TSource>(storage) where TSource : class
    {
        public new TSource? TryGet(Expression<Func<TSource, bool>> keySelector)
        {
            IQueryable<TSource> queryable = AsEntity;
            foreach (var item in attribute) queryable = queryable.Include(item);
            return queryable.FirstOrDefault(keySelector);

        }


        /// <summary>
        /// 异步查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="predicate"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public new PageContent<TSource>? Search<TKey>(Expression<Func<TSource, TKey>> keySelector, int pageIndex = 1, int pageSize = int.MaxValue, Expression<Func<TSource, bool>>? predicate = null, bool isAsc = true)
        {
            IQueryable<TSource> queryable = AsEntity;
            foreach (var item in attribute) queryable = queryable.Include(item);

            if (predicate is not null)
                queryable = queryable.Where(predicate);

            int count = queryable.Count();

            IQueryable<TSource> query = isAsc ?
                    queryable.OrderBy(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize) :
                    queryable.OrderByDescending(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            int maxPage = (int)Math.Ceiling(count / (double)pageSize);

            return new PageContent<TSource>(index: pageIndex, max: maxPage, count: count, info: [.. query]);
        }

    }

    public class PageContent<TSource>(int index, int max, int count, List<TSource> info)
    {
        public int PageIndex { set; get; } = index;
        public int MaxPage { set; get; } = max;
        public int Sum { set; get; } = count;
        public List<TSource> PageData { set; get; } = info;
    }
}
