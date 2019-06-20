using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class OrderByUtils {



        public static IOrderedQueryable<TEntity> BuildOrderedQueryable<TEntity, TKey>
                (IQueryable<TEntity> source, OrderByExpression<TEntity, TKey>[] orderByExpressions)
                    where TEntity : class {

            var propertySelectors = GetPropertySelectors(orderByExpressions);
            return BuildOrderedQueryable(source, propertySelectors);

        }


        public static IOrderedQueryable<TEntity> BuildOrderedQueryable<TEntity, TKey>
            (IQueryable<TEntity> source, params Expression<Func<TEntity, TKey>>[] propertySelectors)
                where TEntity : class {
            IOrderedQueryable<TEntity> ordered = null;
            if (propertySelectors.Length > 0)
                ordered = AddOrderByExpression(source, propertySelectors[0]);
            var props = propertySelectors.Skip(1).ToArray();

            foreach (var prop in props)
                ordered = AddOrderByExpression(ordered, prop);

            return ordered;
        }

        private static IOrderedQueryable<TEntity> AddOrderByExpression<TEntity, TKey>(IQueryable<TEntity> source,
            Expression<Func<TEntity, TKey>> prop)
            where TEntity : class {
            var name = prop.Parameters[0].Name;
            if (name.ToLower() == "d"
                || name.ToLower() == "desc"
                || name.ToLower() == "descending") {
                return source.OrderByDescending(prop);
            } else if (name.ToLower() == "a"
                || name.ToLower() == "asc"
                || name.ToLower() == "ascending") {
                return source.OrderBy(prop);
            } else
                throw new ArgumentException("Order by expression requires a lamdba parameter name of 'd','desc','descending','a','asc', or 'ascending'");
        }


        private static IOrderedQueryable<TEntity> AddOrderByExpression<TEntity, TKey>(IOrderedQueryable<TEntity> source,
            Expression<Func<TEntity, TKey>> prop)
            where TEntity : class {
            var name = prop.Parameters[0].Name;
            if (name.ToLower() == "d"
                || name.ToLower() == "desc"
                || name.ToLower() == "descending") {
                return source.ThenByDescending(prop);
            } else if (name.ToLower() == "a"
                || name.ToLower() == "asc"
                || name.ToLower() == "ascending") {
                return source.ThenBy(prop);
            } else
                throw new ArgumentException("Order by expression requires a lamdba parameter name of 'd','desc','descending','a','asc', or 'ascending'");
        }



        public static Expression<Func<TEntity, TKey>>[] GetPropertySelectors<TEntity, TKey>(OrderByExpression<TEntity, TKey>[] orderByExpressions) {
            var list = new List<Expression<Func<TEntity, TKey>>>();
            foreach (var orderByExpression in orderByExpressions) {
                list.Add(GetProperty(orderByExpression));
            }
            return list.ToArray();
        }

        public static Expression<Func<TEntity, TKey>> GetProperty<TEntity, TKey>(OrderByExpression<TEntity, TKey> orderByExpression) {
            var type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, (orderByExpression.Direction == OrderByDirection.Ascending) ? "asc" : "desc");
            Expression expr = arg;
            PropertyInfo pi = type.GetProperty(orderByExpression.Property);
            expr = Expression.Property(expr, pi);
            return Expression.Lambda<Func<TEntity, TKey>>(expr, arg);
        }

    }
}
