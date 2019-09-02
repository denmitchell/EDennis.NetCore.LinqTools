using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EDennis.NetCore.LinqTools {

    /// <summary>
    /// Applies filtering, sorting, paging, and
    /// selecting (property projection) for
    /// to IEnumerable, IQueryable, and DbSet objects
    /// using a JSON-friendly spec. See 
    /// EDennis.NetCore.LinqTools.Tests for examples.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class FilterSortPageSelect<TEntity> : FilterSortPage<TEntity>
        where TEntity : class, new() {

        public SelectExpression<TEntity> Select { get; set; }

        public FilterSortPageSelect() { }


        /// <summary>
        /// Construct a new FilterSortPageSelect object, using
        /// simple, parameters from a query string, as 
        /// is done in Microsoft's Contoso University example
        /// 
        /// NOTE: For convenience, FilterSortPageSelect can be extended
        /// such that the subclass hard-codes properties, 
        /// the sortUnitMapping, propertiesToSearch, and pageSize.
        public FilterSortPageSelect(
            string[] properties,
            string sortOrder, Dictionary<string, SortUnit<TEntity>> sortUnitMapping,
            string searchString, string[] propertiesToSearch,
            int? pageNumber, int? pageSize) {

            BuildFilter(searchString, propertiesToSearch);
            BuildSort(sortOrder, sortUnitMapping);
            BuildPage(pageNumber, pageSize);
            BuildSelect(properties);
        }


        /// <summary>
        /// Construct a new FilterSortPageSelect object, using
        /// simple, parameters from a query string, as 
        /// is done in Microsoft's Contoso University example.
        /// This overload assumes that a descending sort
        /// direction is specified with "_desc" or " desc" as
        /// a suffix to the sort order.
        /// 
        /// NOTE: For convenience, FilterSortPage can be extended
        /// such that the subclass hard-codes properties, 
        /// propertiesToSearch and pageSize.
        /// </summary>
        public FilterSortPageSelect(
            string[] properties,
            string sortOrder,
            string searchString, string[] propertiesToSearch,
            int? pageNumber, int? pageSize) {

            BuildFilter(searchString, propertiesToSearch);
            BuildSort(sortOrder);
            BuildPage(pageNumber, pageSize);
            BuildSelect(properties);
        }

        /// <summary>
        /// Construct a new FilterSortPageSelect object, using
        /// simple, parameters from a query string, as 
        /// is done in Microsoft's Contoso University example.
        /// 
        /// NOTE: this overload accepts a comma-delimited list
        /// of properties, which can be passed via the query string,
        /// as is done with OData
        /// 
        /// NOTE: For convenience, FilterSortPageSelect can be extended
        /// such that the subclass hard-codes properties, 
        /// the sortUnitMapping, propertiesToSearch, and pageSize.
        public FilterSortPageSelect(
            string properties,
            string sortOrder, Dictionary<string, SortUnit<TEntity>> sortUnitMapping,
            string searchString, string[] propertiesToSearch,
            int? pageNumber, int? pageSize) {

            BuildFilter(searchString, propertiesToSearch);
            BuildSort(sortOrder, sortUnitMapping);
            BuildPage(pageNumber, pageSize);
            BuildSelect(properties);
        }


        /// <summary>
        /// Construct a new FilterSortPageSelect object, using
        /// simple, parameters from a query string, as 
        /// is done in Microsoft's Contoso University example.
        /// This overload assumes that a descending sort
        /// direction is specified with "_desc" or " desc" as
        /// a suffix to the sort order.
        /// 
        /// NOTE: this overload accepts a comma-delimited list
        /// of properties, which can be passed via the query string,
        /// as is done with OData
        /// 
        /// NOTE: For convenience, FilterSortPage can be extended
        /// such that the subclass hard-codes properties, 
        /// propertiesToSearch and pageSize.
        /// </summary>
        public FilterSortPageSelect(
            string properties,
            string sortOrder,
            string searchString, string[] propertiesToSearch,
            int? pageNumber, int? pageSize) {

            BuildFilter(searchString, propertiesToSearch);
            BuildSort(sortOrder);
            BuildPage(pageNumber, pageSize);
            BuildSelect(properties);
        }

        /// <summary>
        /// Builds a SelectExpression object from
        /// an array of properties
        /// </summary>
        /// <param name="properties"></param>
        private void BuildSelect(string[] properties) {
            Select = new SelectExpression<TEntity>();
            Select.AddRange(properties);
        }

        /// <summary>
        /// Builds a SelectExpression object from
        /// a comma-delimited list of properties
        /// </summary>
        /// <param name="properties"></param>
        private void BuildSelect(string properties) {
            Select = new SelectExpression<TEntity>();
            Select.AddRange(properties.Split());
        }




        /// <summary>
        /// Applies filtering, sorting, paging
        /// and selecting to a DbSet
        /// </summary>
        /// <param name="source">DbSet</param>
        /// <returns></returns>
        public IQueryable ApplyTo(DbSet<TEntity> source) {
            var query = source as IQueryable<TEntity>;
            return ApplyTo(query);
        }

        /// <summary>
        /// Applies filtering, sorting, paging
        /// and selecting to an IEnumerable
        /// </summary>
        /// <param name="source">IEnumerable</param>
        /// <returns></returns>
        public IQueryable ApplyTo(IEnumerable<TEntity> source) {
            var query = source as IQueryable<TEntity>;
            return ApplyTo(query);
        }


        /// <summary>
        /// Applies filtering, sorting, paging
        /// and selecting to an IQueryable
        /// </summary>
        /// <param name="source">IQueryable</param>
        /// <returns></returns>
        public IQueryable ApplyTo(IQueryable<TEntity> source) {

            var query = source;
            var type = typeof(TEntity);
            var pe = Expression.Parameter(type, "e");

            //apply filtering, when needed
            if (Filter != null && Filter.Count > 0)
                query = Filter.ApplyTo(query, pe);

            //apply sorting, when needed
            if (Sort != null && Sort.Count > 0)
                query = Sort.ApplyTo(query, pe);

            //apply paging, when needed
            if (Page != null)
                query = Page.ApplyTo(query);

            //apply selecting, when needed
            if (Select != null)
                return Select.ApplyTo(query, pe);

            return query;

        }

    }
}
