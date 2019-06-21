using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EDennis.NetCore.LinqTools {



    public class SortExpression<TEntity> : List<SortUnit<TEntity>>
        where TEntity : class, new() {


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

            if (pi.PropertyType == typeof(int))
                return SortInt(source, sortUnit, pe);

            return null;
        }


        private IOrderedQueryable<TEntity> AddOrderByExpression(IOrderedQueryable<TEntity> source,
            SortUnit<TEntity> sortUnit,
            ParameterExpression pe) {
            var type = typeof(TEntity);
            PropertyInfo pi = type.GetProperty(sortUnit.Property);

            var propType = pi.PropertyType;

            if (pi.PropertyType == typeof(int))
                return SortInt(source, sortUnit, pe);

            return null;
        }


        private IOrderedQueryable<TEntity> SortInt(IQueryable<TEntity> source, SortUnit<TEntity> orderByExpression, ParameterExpression pe) {
            var type = typeof(TEntity);
            PropertyInfo pi = type.GetProperty(orderByExpression.Property);

            if (orderByExpression.Direction == SortDirection.Descending) 
                return source.OrderByDescending(x=> pi.GetValue(x,null));
            else
                return source.OrderBy(x => pi.GetValue(x, null));
        }


        private IOrderedQueryable<TEntity> SortInt(IOrderedQueryable<TEntity> source, SortUnit<TEntity> orderByExpression, ParameterExpression pe) {
            var type = typeof(TEntity);
            PropertyInfo pi = type.GetProperty(orderByExpression.Property);

            if (orderByExpression.Direction == SortDirection.Descending)
                return source.ThenByDescending(x => pi.GetValue(x, null));
            else
                return source.ThenBy(x => pi.GetValue(x, null));
        }


    }
}
