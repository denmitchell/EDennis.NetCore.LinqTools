using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.NetCore.LinqTools {

    /// <summary>
    /// Holds a paging specification and 
    /// provides a method to apply paging to
    /// an IQueryable according to the spec.
    /// </summary>
    /// <typeparam name="TEntity">The model class</typeparam>
    public class PageExpression<TEntity>
        where TEntity : class { 
        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        /// <summary>
        /// Applies paging to an IQueryable, according
        /// to the paging spec provided by this PageExpression object
        /// </summary>
        /// <param name="source">an IQueryable</param>
        /// <returns></returns>
        public IQueryable<TEntity> ApplyTo(IQueryable<TEntity> source){

            var query = source.Skip((PageNumber - 1) * PageSize);
            query = query.Take(PageSize);

            return query;
        }

    }
}
