using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class PageExpression {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        public IQueryable<TEntity> ApplyTo<TEntity>(IQueryable<TEntity> source)
            where TEntity : class {

            var query = source.Skip((PageNumber - 1) * PageSize);
            query = query.Take(PageSize);

            return query;
        }

    }
}
