using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EDennis.NetCore.LinqTools {


    /// <summary>
    /// Holds a sorting specification and 
    /// provides a method to apply sorting to
    /// an IQueryable according to the spec.
    /// A SortExpression object represents 
    /// an ordered collection of SortUnit 
    /// objects.
    /// </summary>
    /// <typeparam name="TEntity">The model class</typeparam>
    public class SortExpression<TEntity> : List<SortUnit<TEntity>>
        where TEntity : class, new() {

        /// <summary>
        /// Applies sorting to an IQueryable, according
        /// to the sorting spec provided by this SortExpression object
        /// </summary>
        /// <param name="source">an IQueryable</param>
        /// <param name="pe">The parameter expression shared by 
        /// all filter expressions and sort expressions</param>
        /// <returns></returns>
        public IOrderedQueryable<TEntity> ApplyTo
                (IQueryable<TEntity> source, ParameterExpression pe) {

            return BuildOrderedQueryable(source, pe);

        }


        /// <summary>
        /// Handles the first sort unit (which needs
        /// an OrderBy or OrderByDescending) separately,
        /// and then handles all remaining sort units 
        /// (which need ThenBy or ThenByDescending)
        /// </summary>
        /// <param name="source">an IQueryable</param>
        /// <param name="pe">The parameter expression shared by 
        /// all filter expressions and sort expressions</param>
        /// <returns></returns>
        private IOrderedQueryable<TEntity> BuildOrderedQueryable
            (IQueryable<TEntity> source, ParameterExpression pe) {
            IOrderedQueryable<TEntity> ordered = null;
            //handle first sort unit
            if (Count > 0)
                ordered = Sort(source, this.FirstOrDefault(), pe);

            //handle all other units
            var props = this.Skip(1).ToArray();
            foreach (var prop in props)
                ordered = Sort(ordered, prop, pe);

            return ordered;
        }


        /// <summary>
        /// Performs a sort on an unordered IQueryable
        /// (the first sort unit)
        /// </summary>
        /// <param name="source">an IQueryable</param>
        /// <param name="sortUnit">the sort spec for a specific property</param>
        /// <param name="pe">The parameter expression shared by 
        /// all filter expressions and sort expressions</param>
        /// <returns></returns>
        private IOrderedQueryable<TEntity> Sort(IQueryable<TEntity> source, SortUnit<TEntity> sortUnit, ParameterExpression pe) {
            var type = typeof(TEntity);
            PropertyInfo pi = type.GetProperty(sortUnit.Property);

            if (sortUnit.Direction == SortDirection.Descending) 
                return source.OrderByDescending(x=> pi.GetValue(x,null));
            else
                return source.OrderBy(x => pi.GetValue(x, null));
        }


        /// <summary>
        /// Performs a sort on an IOrderedQueryable
        /// (all sort units after the first sort unit)
        /// </summary>
        /// <param name="source">an IOrderedQueryable</param>
        /// <param name="sortUnit">the sort spec for a specific property</param>
        /// <param name="pe">The parameter expression shared by 
        /// all filter expressions and sort expressions</param>
        /// <returns></returns>
        private IOrderedQueryable<TEntity> Sort(IOrderedQueryable<TEntity> source, SortUnit<TEntity> sortUnit, ParameterExpression pe) {
            var type = typeof(TEntity);
            PropertyInfo pi = type.GetProperty(sortUnit.Property);

            if (sortUnit.Direction == SortDirection.Descending)
                return source.ThenByDescending(x => pi.GetValue(x, null));
            else
                return source.ThenBy(x => pi.GetValue(x, null));
        }

    }
}
