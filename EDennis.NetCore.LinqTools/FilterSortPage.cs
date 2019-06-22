using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

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

        public PageExpression Page { get; set; }

        public IQueryable<TEntity> ApplyTo(DbSet<TEntity> source) {
            var query = source as IQueryable<TEntity>;
            return ApplyTo(query);
        }

        public IQueryable<TEntity> ApplyTo(IEnumerable<TEntity> source) {
            var query = source as IQueryable<TEntity>;
            return ApplyTo(query);
        }

        public IQueryable<TEntity> ApplyTo(IQueryable<TEntity> source) {

            var query = source;
            var type = typeof(TEntity);
            var pe = Expression.Parameter(type, "e");

            if (Filter != null && Filter.Count > 0)
                query = Filter.ApplyTo(query, pe);
            if (Sort != null && Sort.Count > 0)
                query = Sort.ApplyTo(query, pe);
            if (Page != null)
                query = Page.ApplyTo(query);
            return query;

        }


    }
}
