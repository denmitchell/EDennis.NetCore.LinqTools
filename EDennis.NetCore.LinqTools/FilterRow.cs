using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EDennis.NetCore.LinqTools {

    /// <summary>
    /// Represents the intersection of (And over) a
    /// set of FilterUnit objects.
    /// </summary>
    /// <typeparam name="TEntity">The model class</typeparam>
    public class FilterRow<TEntity> : List<FilterUnit<TEntity>>
        where TEntity : class {

        /// <summary>
        /// Gets the intersection of a set of FilterUnit objects
        /// </summary>
        /// <param name="pe">The parameter expression shared by 
        /// all filter expressions and sort expressions</param>
        /// <returns></returns>
        public Expression GetExpression(ParameterExpression pe, 
                BooleanOperation boolOp = BooleanOperation.And) {

            Expression op = null; //= Expression.Constant(true);

            if (Count > 0)
                op = this.FirstOrDefault().GetExpression(pe);

            //handle all other units
            var exprs = this.Skip(1).ToArray();
            foreach (var expr in exprs) {
                if (boolOp == BooleanOperation.And)
                    op = Expression.And(op, expr.GetExpression(pe));
                else
                    op = Expression.Or(op, expr.GetExpression(pe));
            }
            return op;
        }
    }
}
