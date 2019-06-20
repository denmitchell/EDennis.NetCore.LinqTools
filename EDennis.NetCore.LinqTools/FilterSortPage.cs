using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class FilterSortPage<TEntity>
        where TEntity : class {

        public FilterExpression<TEntity> Filter { get; set; }

        public SortExpression<TEntity> Sort { get; set; }

        public PageExpression Page { get; set; }


        public IQueryable<TEntity> ApplyTo(IEnumerable<TEntity> source) {

            var query = source.AsQueryable();
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
