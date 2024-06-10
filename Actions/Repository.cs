using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Actions
{
    public interface IRepository<T> where T : class
    {
        bool Exist(Expression<Func<T, bool>> keySelector);
        Task<bool> ExistAsync(Expression<Func<T, bool>> keySelector);

        Task<bool> AddInfoAsync(T info);
        bool AddInfo(T info);

        bool UpdateInfo(T info);
        Task<bool> UpdateInfoAsync(T info);

        bool DeleteInfo(Expression<Func<T, bool>> keySelector);
        Task<bool> DeleteInfoAsync(Expression<Func<T, bool>> keySelector);

        T? GetInfo(Guid id);
        Task<T?> GetInfoAsync(Guid id);

        T? GetInfo(Expression<Func<T, bool>> keySelector);
        Task<T?> GetInfoAsync(Expression<Func<T, bool>> keySelector);

        int Count(Expression<Func<T, bool>>? keySelector);
        Task<int> CountAsync(Expression<Func<T, bool>>? keySelector);

        Task<PageContext<T>?> SearchAsync<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>>? predicate = null, bool isAsc = true);
    }

    public class Repository<T>(ManagementSystemContext storage) : IRepository<T> where T : class
    {
        public DbSet<T> AsEntity => storage.Set<T>();



        public async Task<int> CountAsync(Expression<Func<T, bool>>? keySelector) => await (keySelector is null ? AsEntity.CountAsync() : AsEntity.Where(keySelector).CountAsync());
        public int Count(Expression<Func<T, bool>>? keySelector) => keySelector is null ? AsEntity.Count() : AsEntity.Where(keySelector).Count();



        public bool Exist(Expression<Func<T, bool>> keySelector) => AsEntity.Any(keySelector);
        public async Task<bool> ExistAsync(Expression<Func<T, bool>> keySelector) => await AsEntity.AnyAsync(keySelector);



        public T? GetInfo(Guid id) => AsEntity.Find(id);
        public async Task<T?> GetInfoAsync(Guid id) => await AsEntity.FindAsync(id);

        public T? GetInfo(Expression<Func<T, bool>> keySelector) => AsEntity.FirstOrDefault(keySelector);
        public async Task<T?> GetInfoAsync(Expression<Func<T, bool>> keySelector) => await AsEntity.FirstOrDefaultAsync(keySelector);



        public bool AddInfo(T info) { AsEntity.Add(info); return storage.SaveChanges() == 1; }
        public async Task<bool> AddInfoAsync(T info) { await AsEntity.AddAsync(info); return await storage.SaveChangesAsync() == 1; }


        public bool UpdateInfo(T info) { AsEntity.Update(info); return storage.SaveChanges() == 1; }
        public async Task<bool> UpdateInfoAsync(T info) { await Task.Run(() => AsEntity.Update(info)); return await storage.SaveChangesAsync() == 1; }


        public bool DeleteInfo(Expression<Func<T, bool>> keySelector) { AsEntity.Where(keySelector); return storage.SaveChanges() == 1; }
        public async Task<bool> DeleteInfoAsync(Expression<Func<T, bool>> keySelector) { await Task.Run(() => AsEntity.Where(keySelector)); return await (storage.SaveChangesAsync()) == 1; }


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
        public async Task<PageContext<T>?> SearchAsync<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>>? predicate = null, bool isAsc = true)
        {
            IQueryable<T> queryable = AsEntity.AsQueryable().AsNoTracking();
            if (predicate is not null)
                queryable = queryable.Where(predicate);

            int count = await queryable.CountAsync();

            IQueryable<T> query = isAsc ?
                    queryable.OrderBy(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize) :
                    queryable.OrderByDescending(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            int maxPage = (int)Math.Ceiling(count / (double)pageSize);

            return new PageContext<T>(index: pageIndex, max: maxPage, count: count, info: await query.ToListAsync());
        }

        

        public async Task<PageContext<T>?> SearchNotLimitAsync<TKey>(Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>>? predicate = null, bool isAsc = true)
        {
            IQueryable<T> queryable = AsEntity.AsQueryable().AsNoTracking();
            if (predicate is not null)
                queryable = queryable.Where(predicate);

            int count = await queryable.CountAsync();

            IQueryable<T> query = isAsc ?
                    queryable.OrderBy(keySelector) :
                    queryable.OrderByDescending(keySelector);

            // int maxPage = (int)Math.Ceiling(count / (double)pageSize);

            return new PageContext<T>(index: 1, max: 1, count: count, info: await query.ToListAsync());
        }

    }

    public class IncludeRepository<T, Tflag>(ManagementSystemContext storage) : IRepository<T> where T : class
    {
        public DbSet<T> AsEntity => storage.Set<T>();



        public int Count(Expression<Func<T, bool>>? keySelector) => keySelector is null ? AsEntity.Count() : AsEntity.Where(keySelector).Count();
        public async Task<int> CountAsync(Expression<Func<T, bool>>? keySelector) => await (keySelector is null ? AsEntity.CountAsync() : AsEntity.Where(keySelector).CountAsync());
        



        public bool Exist(Expression<Func<T, bool>> keySelector) => AsEntity.Any(keySelector);
        public async Task<bool> ExistAsync(Expression<Func<T, bool>> keySelector) => await AsEntity.AnyAsync(keySelector);



        
        public T? GetInfo(Guid id) => AsEntity.Find(id);
        public async Task<T?> GetInfoAsync(Guid id) => await AsEntity.FindAsync(id);
        public T? GetInfo(Expression<Func<T, bool>> keySelector) => AsEntity.FirstOrDefault(keySelector);
        public T? GetInfo(Expression<Func<T, bool>> keySelector, Expression<Func<T, Tflag>> attribute) => AsEntity.Include(attribute).FirstOrDefault(keySelector);
        public async Task<T?> GetInfoAsync(Expression<Func<T, bool>> keySelector) => await AsEntity.FirstOrDefaultAsync(keySelector);

        public async Task<T?> GetInfoAsync(Expression<Func<T, bool>> keySelector, Expression<Func<T, Tflag>> attribute) => await AsEntity.Include(attribute).FirstOrDefaultAsync(keySelector);



        public bool AddInfo(T info) { AsEntity.Add(info); return storage.SaveChanges() == 1; }
        public async Task<bool> AddInfoAsync(T info) { await AsEntity.AddAsync(info); return await storage.SaveChangesAsync() == 1; }


        public bool UpdateInfo(T info) { AsEntity.Update(info); return storage.SaveChanges() == 1; }
        public async Task<bool> UpdateInfoAsync(T info) { await Task.Run(() => AsEntity.Update(info)); return await storage.SaveChangesAsync() == 1; }


        public bool DeleteInfo(Expression<Func<T, bool>> keySelector) { AsEntity.Where(keySelector); return storage.SaveChanges() == 1; }
        public async Task<bool> DeleteInfoAsync(Expression<Func<T, bool>> keySelector) { await Task.Run(() => AsEntity.Where(keySelector)); return await (storage.SaveChangesAsync()) == 1; }


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
        public async Task<PageContext<T>?> SearchAsync<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>>? predicate = null, bool isAsc = true)
        {
            IQueryable<T> queryable = AsEntity.AsQueryable().AsNoTracking();
            if (predicate is not null)
                queryable = queryable.Where(predicate);

            int count = await queryable.CountAsync();

            IQueryable<T> query = isAsc ?
                    queryable.OrderBy(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize) :
                    queryable.OrderByDescending(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            int maxPage = (int)Math.Ceiling(count / (double)pageSize);

            return new PageContext<T>(index: pageIndex, max: maxPage, count: count, info: await query.ToListAsync());
        }

        public async Task<PageContext<T>?> SearchNotLimitAsync<TKey>(Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>>? predicate = null, bool isAsc = true)
        {
            IQueryable<T> queryable = AsEntity.AsQueryable().AsNoTracking();
            if (predicate is not null)
                queryable = queryable.Where(predicate);

            int count = await queryable.CountAsync();

            IQueryable<T> query = isAsc ?
                    queryable.OrderBy(keySelector) :
                    queryable.OrderByDescending(keySelector);

            // int maxPage = (int)Math.Ceiling(count / (double)pageSize);

            return new PageContext<T>(index: 1, max: 1, count: count, info: await query.ToListAsync());
        }


        public async Task<PageContext<T>?> IncludeSearchAsync<TKey>(int pageIndex, int pageSize, Expression<Func<T, Tflag>> attribute, Expression<Func<T, TKey>> keySelector, Expression<Func<T, bool>>? predicate = null, bool isAsc = true)
        {
            IQueryable<T> queryable = AsEntity.AsQueryable().AsNoTracking();
            if (predicate is not null)
                queryable = queryable.Include(attribute).Where(predicate);

            int count = await queryable.CountAsync();

            IQueryable<T> query = isAsc ?
                    queryable.Include(attribute).OrderBy(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize) :
                    queryable.Include(attribute).OrderByDescending(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            int maxPage = (int)Math.Ceiling(count / (double)pageSize);

            return new PageContext<T>(index: pageIndex, max: maxPage, count: count, info: await query.ToListAsync());
        }

    }

    public class PageContext<T>(int index, int max, int count, List<T> info)
    {
        public int pageIndex { set; get; } = index;
        public int MaxPage { set; get; } = max;
        public int Sum { set; get; } = count;
        public List<T> data { set; get; } = info;
    }
}
