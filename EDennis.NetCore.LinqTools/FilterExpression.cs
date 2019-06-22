using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    
    /// <summary>
    /// Holds a filtering specification and 
    /// provides a method to apply filtering to
    /// an IQueryable according to the spec.
    /// A FilterExpression object represents 
    /// a union of (Or over) a set of FilterRow 
    /// objects.
    /// </summary>
    /// <typeparam name="TEntity">The model class</typeparam>
    public class FilterExpression<TEntity> : List<FilterRow<TEntity>> 
        where TEntity: class {


        /// <summary>
        /// Applies filtering to an IQueryable, according
        /// to the filtering spec provided by this FilterExpression object
        /// </summary>
        /// <param name="source">an IQueryable</param>
        /// <param name="pe">The parameter expression shared by 
        /// all filter expressions and sort expressions</param>
        /// <returns></returns>
        public IQueryable<TEntity> ApplyTo(IQueryable<TEntity> source, ParameterExpression pe) {
            return source.Where(GetLambdaExpression(pe));
        }

        /// <summary>
        /// Gets the union of a set of FilterRow objects
        /// </summary>
        /// <param name="pe">The parameter expression shared by 
        /// all filter expressions and sort expressions</param>
        /// <returns></returns>
        protected Expression GetExpression(ParameterExpression pe) {            
                Expression or = Expression.Constant(false);
                foreach (var e in this) {
                    or = Expression.Or(or, e.GetExpression(pe));
                }
                return or;
        }

        /// <summary>
        /// Gets the Lambda expression associated with this
        /// FilterExpression object
        /// </summary>
        /// <param name="pe">The parameter expression shared by 
        /// all filter expressions and sort expressions</param>
        /// <returns></returns>
        protected Expression<Func<TEntity, bool>> GetLambdaExpression(ParameterExpression pe) {
                var type = typeof(TEntity);
                var expr = Expression.Lambda<Func<TEntity, bool>>(GetExpression(pe), pe);
                return expr;
        }

    }
}
