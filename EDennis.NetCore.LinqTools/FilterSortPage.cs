using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class FilterSortPage<TEntity,TKey>
        where TEntity: class {
        
        public FilterExpression<TEntity> Filter { get; set; }

        public SortExpression<TEntity,TKey> Sort { get; set; }

        public PageExpression Page { get; set; }


        public IQueryable<TEntity> ApplyTo(IQueryable<TEntity> source) {

                var query = source;
                if(Filter != null && Filter.Count > 0)
                    query = Filter.ApplyTo(query);
                if (Sort != null && Sort.Count > 0)
                    query = Sort.ApplyTo(query);
                if (Page != null)
                    query = Page.ApplyTo(query);
                return query;

        }


    }
}
