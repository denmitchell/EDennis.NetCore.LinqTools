using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class AndedExpressionSet<TEntity> : List<FilterExpression<TEntity>> 
        where TEntity: class {

        public Expression Expression {            
            get {
                Expression and = Expression.Constant(true);
                foreach (var e in this) {
                    and = Expression.And(and, e.Expression);
                }
                return and;
            }
        } 
    }
}
