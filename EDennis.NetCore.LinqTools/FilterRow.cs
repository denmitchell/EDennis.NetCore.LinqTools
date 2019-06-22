using System.Collections.Generic;
using System.Linq.Expressions;

namespace EDennis.NetCore.LinqTools {

    /// <summary>
    /// Represents the intersection of (And over) a
    /// set of FilterUnit objects.
    /// </summary>
    /// <typeparam name="TEntity">The model class</typeparam>
    public class FilterRow<TEntity> : List<FilterUnit<TEntity>> 
        where TEntity: class {

        /// <summary>
        /// Gets the intersection of a set of FilterUnit objects
        /// </summary>
        /// <param name="pe">The parameter expression shared by 
        /// all filter expressions and sort expressions</param>
        /// <returns></returns>
        public Expression GetExpression(ParameterExpression pe) {            
                Expression and = Expression.Constant(true);
                foreach (var e in this) {
                    and = Expression.And(and, e.GetExpression(pe));
                }
                return and;
        } 
    }
}
