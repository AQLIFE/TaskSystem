using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManangerSystem.Services.Repository;

namespace TaskManangerSystem.IServices
{
    public interface IRepository<TSource> where TSource : class
    {
        public DbSet<TSource> AsEntity { set; get; }

        bool Exist(Expression<Func<TSource, bool>> keySelector);
        bool Add(TSource info);
        bool Update(TSource info);
        bool Delete(Expression<Func<TSource, bool>> keySelector);

        TSource? TryGet(Guid id);
        TSource? TryGet(Expression<Func<TSource, bool>> keySelector);
        int Count(Expression<Func<TSource, bool>>? keySelector);

        PageContent<TSource>? Search<TKey>(Expression<Func<TSource, TKey>> keySelector, int pageIndex = 1, int pageSize = int.MaxValue, Expression<Func<TSource, bool>>? predicate = null, bool isAsc = true);

    }

    public interface IRepositoryAsync<TSource> where TSource : class
    {
        public DbSet<TSource> AsEntity { set; get; }

        Task<bool> ExistAsync(Expression<Func<TSource, bool>> keySelector);

        Task<bool> AddAsync(TSource info);
        Task<bool> UpdateAsync(TSource info);
        Task<bool> DeleteAsync(Expression<Func<TSource, bool>> keySelector);

        Task<TSource?> TryGetAsync(Guid id);
        Task<TSource?> TryGetAsync(Expression<Func<TSource, bool>> keySelector);

        Task<int> CountAsync(Expression<Func<TSource, bool>>? keySelector);


        /// <summary>
        /// 复合查询
        /// </summary>
        /// <typeparam name="TKey">排序关键字段类型</typeparam>
        /// <param name="keySelector">排序关键字</param>
        /// <param name="pageIndex">搜索数据起始位</param>
        /// <param name="pageSize">单页数据量</param>
        /// <param name="predicate">搜索条件</param>
        /// <param name="isAsc">升序Or降序(默认升序)</param>
        /// <returns></returns>
        Task<PageContent<TSource>?> SearchAsync<TKey>(Expression<Func<TSource, TKey>> keySelector, int pageIndex = 1, int pageSize = int.MaxValue, Expression<Func<TSource, bool>>? predicate = null, bool isAsc = true);

    }
}
