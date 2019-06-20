using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class FilterExpression<TEntity> : List<FilterRow<TEntity>> 
        where TEntity: class {

        public Expression GetExpression(ParameterExpression pe) {            
                Expression or = Expression.Constant(false);
                foreach (var e in this) {
                    or = Expression.Or(or, e.GetExpression(pe));
                }
                return or;
        } 

        public Expression<Func<TEntity, bool>> GetLambdaExpression(ParameterExpression pe) {
                var type = typeof(TEntity);
                var expr = Expression.Lambda<Func<TEntity, bool>>(GetExpression(pe), pe);
                return expr;
        }

        public IQueryable<TEntity> ApplyTo(IQueryable<TEntity> source, ParameterExpression pe) {
            return source.Where(GetLambdaExpression(pe));
        }
    }
}
