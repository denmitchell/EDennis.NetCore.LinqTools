using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EDennis.NetCore.LinqTools {

    /// <summary>
    /// Holds a paging specification and 
    /// provides a method to apply paging to
    /// an IQueryable according to the spec.
    /// </summary>
    /// <typeparam name="TEntity">The model class</typeparam>
    public class SelectExpression<TEntity> : List<string>
        where TEntity : class {

        /// <summary>
        /// Performs a sort on an unordered IQueryable
        /// (the first sort unit)
        /// </summary>
        /// <param name="source">an IQueryable</param>
        /// <param name="sortUnit">the sort spec for a specific property</param>
        /// <param name="pe">The parameter expression shared by 
        /// all filter expressions and sort expressions</param>
        /// <returns></returns>
        public IQueryable<object> ApplyTo(IQueryable<TEntity> source, ParameterExpression pe) {
            var type = typeof(TEntity);

            Expression selectExpression = pe;

            foreach (string property in this) {
                var body = Expression.PropertyOrField(pe, property);
                selectExpression = Expression.Property(selectExpression, property);
            }

            var assignments = type.GetFields().Select((prop, i) => Expression.Bind(prop, selectExpression));
            var lambdaExpression = Expression.Lambda<Func<TEntity, object>>(Expression.MemberInit(Expression.New(type.GetConstructors()[0]), assignments), pe);

            return source.Select(lambdaExpression);
        }

    }
}
