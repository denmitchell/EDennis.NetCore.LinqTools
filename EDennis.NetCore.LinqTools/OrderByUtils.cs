using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class OrderByUtils {




        public static Expression<Func<TEntity, TKey>>[] GetProperties<TEntity, TKey>(string[] properties) {
            var list = new List<Expression<Func<TEntity, TKey>>>();
            foreach(var prop in properties) {
                list.Add(GetProperty<TEntity, TKey>(prop));
            }
            return list.ToArray();
        }

        public static Expression<Func<TEntity, TKey>> GetProperty<TEntity, TKey>(string property) {
            var type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "e");
            Expression expr = arg;
            PropertyInfo pi = type.GetProperty(property);
            expr = Expression.Property(expr, pi);
            return Expression.Lambda<Func<TEntity, TKey>>(expr, arg);
        }

    }
}
