using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace EDennis.NetCore.LinqTools {

    /// <summary>
    /// Applies filtering, sorting, and paging for
    /// to IEnumerable, IQueryable, and DbSet objects
    /// using a JSON-friendly spec. See 
    /// EDennis.NetCore.LinqTools.Tests for examples.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class FilterSortPage<TEntity>
        where TEntity : class, new() {

        public FilterExpression<TEntity> Filter { get; set; }

        public SortExpression<TEntity> Sort { get; set; }

        public PageExpression<TEntity> Page { get; set; }

        public SelectExpression<TEntity> Select { get; set; }


        public FilterSortPage() { }

        /// <summary>
        /// Construct a new FilterSortPage object, using
        /// simple, parameters from a query string, as 
        /// is done in Microsoft's Contoso University example
        /// 
        /// NOTE: For convenience, FilterSortPage can be extended
        /// such that the subclass hard-codes the sortUnitMapping,
        /// propertiesToSearch, and pageSize.
        /// </summary>
        public FilterSortPage(
            string sortOrder, Dictionary<string,SortUnit<TEntity>> sortUnitMapping,
            string searchString, string[] propertiesToSearch,
            int? pageNumber, int? pageSize) {

            BuildFilter(searchString, propertiesToSearch);
            BuildSort(sortOrder, sortUnitMapping);
            BuildPage(pageNumber, pageSize);
        }


        /// <summary>
        /// Construct a new FilterSortPage object, using
        /// simple, parameters from a query string, as 
        /// is done in Microsoft's Contoso University example.
        /// This overload assumes that a descending sort
        /// direction is specified with "_desc" or " desc" as
        /// a suffix to the sort order.
        /// 
        /// NOTE: For convenience, FilterSortPage can be extended
        /// such that the subclass hard-codes propertiesToSearch 
        /// and pageSize.
        /// </summary>
        public FilterSortPage(
            string sortOrder,
            string searchString, string[] propertiesToSearch,
            int? pageNumber, int? pageSize) {

            BuildFilter(searchString, propertiesToSearch);
            BuildSort(sortOrder);
            BuildPage(pageNumber, pageSize);
        }



        /// <summary>
        /// Applies filtering, sorting, and paging
        /// to a DbSet
        /// </summary>
        /// <param name="source">DbSet</param>
        /// <returns></returns>
        public IQueryable<TEntity> ApplyTo(DbSet<TEntity> source, out int pageCount) {
            var query = source as IQueryable<TEntity>;
            return ApplyTo(query, out pageCount);
        }

        /// <summary>
        /// Applies filtering, sorting, and paging
        /// to an IEnumerable
        /// </summary>
        /// <param name="source">IEnumerable</param>
        /// <returns></returns>
        public IQueryable<TEntity> ApplyTo(IEnumerable<TEntity> source, out int pageCount) {
            var query = source.AsQueryable();
            return ApplyTo(query, out pageCount);
        }


        /// <summary>
        /// Applies filtering, sorting, and paging
        /// to an IQueryable
        /// </summary>
        /// <param name="source">IQueryable</param>
        /// <returns></returns>
        public IQueryable<TEntity> ApplyTo(IQueryable<TEntity> source, out int pageCount) {

            //add no tracking for better performance
            var query = source.AsNoTracking();

            var type = typeof(TEntity);
            var pe = Expression.Parameter(type, "e");

            //apply filtering, when needed
            if (Filter != null && Filter.Count > 0)
                query = Filter.ApplyTo(query, pe);

            //apply sorting, when needed
            if (Sort != null && Sort.Count > 0)
                query = Sort.ApplyTo(query, pe);

            //apply paging, when needed
            if (Page != null) {
                query = Page.ApplyTo(query);
                pageCount = Page.PageCount.Value;
            } else {
                pageCount = 1;
            }

            return query;

        }


        /// <summary>
        /// Builds the filter expression
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="props"></param>
        private void BuildFilter(string filter, string[] props) {
            if (filter != null) {
                Filter = new FilterExpression<TEntity>();
                foreach (var prop in props) {
                    var row = new FilterRow<TEntity> {
                    new FilterUnit<TEntity> {
                        Property = prop,
                        Operation = FilterOperation.Contains,
                        StringValue = filter
                    }
                };
                    Filter.Add(row);
                }
            }
        }


        /// <summary>
        /// Builds the sort expression, based upon a mapping of
        /// string expressions to sort units
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="sortUnitMapping"></param>
        private void BuildSort(string sortOrder, Dictionary<string, SortUnit<TEntity>> sortUnitMapping) {
            if (sortOrder != null) {
                var sortUnit = sortUnitMapping[sortOrder];
                Sort = new SortExpression<TEntity> {
                    sortUnit
                };
            }
        }


        /// <summary>
        /// Builds the sort expression, based upon a string sort
        /// expression and the assumption that the string expression
        /// ends with " desc" or "_desc" (ignoring case) for descending
        /// sorts.
        /// </summary>
        /// <param name="sortOrder"></param>
        private void BuildSort(string sortOrder) {
            if (sortOrder != null) {

                var components = sortOrder.Split(' ', '_');
                SortDirection dir = SortDirection.Ascending;
                if (components.Length > 1 && components[1].ToUpper().StartsWith("DESC"))
                    dir = SortDirection.Descending;

                Sort = new SortExpression<TEntity> {
                new SortUnit<TEntity> {
                    Property = components[0],
                    Direction = dir
                    }
                };
            }
        }


        /// <summary>
        /// Builds the page expression
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        private void BuildPage(int? pageNumber, int? pageSize) {
            if (pageNumber != null) {
                Page = new PageExpression<TEntity> {
                    PageNumber = pageNumber.Value,
                    PageSize = pageSize.Value
                };
            }
        }

    }
}
