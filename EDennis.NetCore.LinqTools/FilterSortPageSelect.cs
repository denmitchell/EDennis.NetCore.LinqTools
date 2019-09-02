using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EDennis.NetCore.LinqTools {

    /// <summary>
    /// Applies filtering, sorting, paging, and
    /// selecting (property projection) for
    /// to IEnumerable, IQueryable, and DbSet objects
    /// using a JSON-friendly spec. See 
    /// EDennis.NetCore.LinqTools.Tests for examples.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class FilterSortPageSelect<TEntity>
        where TEntity : class, new() {

        public FilterExpression<TEntity> Filter { get; set; }

        public SortExpression<TEntity> Sort { get; set; }

        public PageExpression<TEntity> Page { get; set; }

        public SelectExpression<TEntity> Select { get; set; }

        /// <summary>
        /// Applies filtering, sorting, paging
        /// and selecting to a DbSet
        /// </summary>
        /// <param name="source">DbSet</param>
        /// <returns></returns>
        public IQueryable ApplyTo(DbSet<TEntity> source) {
            var query = source as IQueryable<TEntity>;
            return ApplyTo(query);
        }

        /// <summary>
        /// Applies filtering, sorting, paging
        /// and selecting to an IEnumerable
        /// </summary>
        /// <param name="source">IEnumerable</param>
        /// <returns></returns>
        public IQueryable ApplyTo(IEnumerable<TEntity> source) {
            var query = source as IQueryable<TEntity>;
            return ApplyTo(query);
        }


        /// <summary>
        /// Applies filtering, sorting, paging
        /// and selecting to an IQueryable
        /// </summary>
        /// <param name="source">IQueryable</param>
        /// <returns></returns>
        public IQueryable ApplyTo(IQueryable<TEntity> source) {

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

            //apply selecting, when needed
            if (Select != null)
                return Select.ApplyTo(query, pe);

            return query;

        }

    }
}
