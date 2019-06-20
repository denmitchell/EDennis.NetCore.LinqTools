using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class FilterExpression<TEntity> : List<FilterRow<TEntity>> 
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

        public Expression<Func<TEntity, bool>> LambdaExpression {
            get {
                var type = typeof(TEntity);
                var pe = Expression.Parameter(type, "e");
                var expr = Expression.Lambda<Func<TEntity, bool>>(Expression, pe);
                return expr;
            }
        }

        public IQueryable<TEntity> ApplyTo(IQueryable<TEntity> source) {
            return source.Where(LambdaExpression);
        }
    }
}
