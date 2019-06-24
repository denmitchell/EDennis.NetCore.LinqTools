using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EDennis.NetCore.LinqTools {

    /// <summary>
    /// Applies filtering, sorting, and paging for
    /// to IEnumerable, IQueryable, and DbSet objects
    /// using a JSON-friendly spec. See 
    /// EDennis.NetCore.LinqTools.Tests for examples.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class FilterSortPage<TEntity>
        where TEntity : class, new() {

        public FilterExpression<TEntity> Filter { get; set; }

        public SortExpression<TEntity> Sort { get; set; }

        public PageExpression<TEntity> Page { get; set; }


        /// <summary>
        /// Applies filtering, sorting, and paging
        /// to a DbSet
        /// </summary>
        /// <param name="source">DbSet</param>
        /// <returns></returns>
        public IQueryable<TEntity> ApplyTo(DbSet<TEntity> source, out int pageCount) {
            var query = source as IQueryable<TEntity>;
            return ApplyTo(query, out pageCount);
        }

        /// <summary>
        /// Applies filtering, sorting, and paging
        /// to an IEnumerable
        /// </summary>
        /// <param name="source">IEnumerable</param>
        /// <returns></returns>
        public IQueryable<TEntity> ApplyTo(IEnumerable<TEntity> source, out int pageCount) {
            var query = source as IQueryable<TEntity>;
            return ApplyTo(query, out pageCount);
        }


        /// <summary>
        /// Applies filtering, sorting, and paging
        /// to an IQueryable
        /// </summary>
        /// <param name="source">IQueryable</param>
        /// <returns></returns>
        public IQueryable<TEntity> ApplyTo(IQueryable<TEntity> source, out int pageCount) {

            var query = source;
            var type = typeof(TEntity);
            var pe = Expression.Parameter(type, "e");

            //apply filtering, when needed
            if (Filter != null && Filter.Count > 0)
                query = Filter.ApplyTo(query, pe);

            //apply sorting, when needed
            if (Sort != null && Sort.Count > 0)
                query = Sort.ApplyTo(query, pe);

            //apply paging, when needed
            if (Page != null)
                query = Page.ApplyTo(query);

            pageCount = Page.PageCount.Value;

            return query;

        }

    }
}
