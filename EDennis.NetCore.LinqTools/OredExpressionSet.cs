using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class OredExpressionSet<TEntity> : List<AndedExpressionSet<TEntity>> 
        where TEntity: class {

        public Expression Expression {            
            get {
                Expression or = Expression.Constant(true);
                foreach (var e in this) {
                    or = Expression.Or(or, e.Expression);
                }
                return or;
            }
        } 
    }
}
