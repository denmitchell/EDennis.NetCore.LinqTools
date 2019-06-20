using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EDennis.NetCore.LinqTools {



    public class SortExpression<TEntity>: List<SortUnit<TEntity>> 
        where TEntity: class{

        internal class PropertySelectorAndSortDirection<TKey> {
            public Expression<Func<TEntity, TKey>> PropertySelector { get; set; }
            public SortDirection SortDirection { get; set; }
        }


        public IOrderedQueryable<TEntity> ApplyTo
                (IQueryable<TEntity> source, ParameterExpression pe) {

            return BuildOrderedQueryable(source, pe);

        }


        private IOrderedQueryable<TEntity> BuildOrderedQueryable
            (IQueryable<TEntity> source, ParameterExpression pe) {
            IOrderedQueryable<TEntity> ordered = null;
            if (Count > 0)
                ordered = AddOrderByExpression(source, this.FirstOrDefault(), pe);
            var props = this.Skip(1).ToArray();

            foreach (var prop in props)
                ordered = AddOrderByExpression(ordered, prop, pe);

            return ordered;
        }

        private IOrderedQueryable<TEntity> AddOrderByExpression(
            IQueryable<TEntity> source, SortUnit<TEntity> sortUnit,
            ParameterExpression pe) {
            var type = typeof(TEntity);
            PropertyInfo pi = type.GetProperty(sortUnit.Property);
            var propType = pi.PropertyType;
            
            switch (propType.ToString()) {
                case "Int32":
                    var s1 = GetPropertySelector(sortUnit, pe, default(int));
                    break;
                case "Double":
                    var s2 = GetPropertySelector(sortUnit, pe, default(double));
                    break;
            }
            var propertySelector = GetPropertySelector(sortUnit, pe, val);
            if (sortUnit.Direction == SortDirection.Descending) {
                return source.OrderByDescending(propertySelector);
            } else {
                return source.OrderBy(propertySelector);
            }
        }


        private IOrderedQueryable<TEntity> AddOrderByExpression(IOrderedQueryable<TEntity> source,
            SortUnit<TEntity> sortUnit,
            ParameterExpression pe) {
            var type = typeof(TEntity);
            PropertyInfo pi = type.GetProperty(sortUnit.Property);
            var propertySelector = GetPropertySelector(sortUnit, pe, pi.PropertyType);
            if (sortUnit.Direction == SortDirection.Descending)
                return source.ThenByDescending(propertySelector);
            else
                return source.ThenBy(propertySelector);
        }


        private Expression<Func<TEntity, TKey>> GetPropertySelector<TKey>(SortUnit<TEntity> orderByExpression, ParameterExpression pe, TKey propType) {
            var type = typeof(TEntity);
            PropertyInfo pi = type.GetProperty(orderByExpression.Property);
            var expr = Expression.Property(pe, pi);
            //var propertyAccess = Expression.MakeMemberAccess(pe, pi);
            var selector = Expression.Lambda<Func<TEntity, TKey>>(expr/*propertyAccess*/, pe);

            //Type delegateType = typeof(Func<,>).MakeGenericType(type, pi.PropertyType);
            //LambdaExpression lambda = Expression.Lambda(delegateType, propertyAccess, pe);
            return selector;
        }

        private static dynamic GetDefault(Type type){
            if (type.IsValueType) {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
