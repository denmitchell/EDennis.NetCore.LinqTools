using System;
using System.Linq;
using System.Linq.Expressions;

namespace EDennis.NetCore.LinqTools {

    /// <summary>
    /// Represents an atomic unit for filtering,
    /// where a property value is evaluated against a 
    /// literal value using one of the defined
    /// filtering operations.
    /// </summary>
    /// <typeparam name="TEntity">The model class</typeparam>
    public class FilterUnit<TEntity>
        where TEntity : class {
        public string Property { get; set; }
        public FilterOperation Operation { get; set; }
        public string StringValue { get; set; }

        /// <summary>
        /// Returns the Linq expression associated
        /// with this Filter unit
        /// </summary>
        /// <param name="pe">A parameter expression shared by 
        /// all filter expressions and sorting expression</param>
        /// <returns></returns>
        public Expression GetExpression(ParameterExpression pe) {

            //handle comma-delimited list
            if (StringValue.Contains(",")) {
                var row = new FilterRow<TEntity>();
                row.AddRange(
                    StringValue.Split(',').Select(x =>
                    new FilterUnit<TEntity> {
                        Property = Property,
                        Operation = Operation,
                        StringValue = x
                    }
                    ).ToArray()
                );
                return row.GetExpression(pe, BooleanOperation.Or);
            }

            var type = typeof(TEntity);

            var propertyInfo = type.GetProperty(Property);
            if (propertyInfo == null)
                throw new ArgumentException($"Property \"{Property},\" specified in the filter, does not exist in {typeof(TEntity).Name}");

            //represent the property value as a Member expression
            var left = Expression.Property(pe, type.GetProperty(Property));

            //cast the literal string value to its property type
            object objVal = Convert.ChangeType(StringValue, propertyInfo.PropertyType);

            //represent the literal value as a Constant expression
            var right = Expression.Constant(objVal);

            //apply the appropriate operation to the expression
            //and return the result
            switch (Operation) {
                case FilterOperation.Eq:
                    return Expression.Equal(left, right);
                case FilterOperation.Lt:
                    return Expression.LessThan(left, right);
                case FilterOperation.Le:
                    return Expression.LessThanOrEqual(left, right);
                case FilterOperation.Gt:
                    return Expression.GreaterThan(left, right);
                case FilterOperation.Ge:
                    return Expression.GreaterThanOrEqual(left, right);
                case FilterOperation.StartsWith:
                    return Expression.Call(left, "StartsWith", Type.EmptyTypes, right);
                case FilterOperation.EndsWith:
                    return Expression.Call(left, "EndsWith", Type.EmptyTypes, right);
                case FilterOperation.Contains:
                    return Expression.Call(left, "Contains", Type.EmptyTypes, right);
            }

            //if the filter unit isn't able to generate an 
            //expression, return null
            return null;
        }

    }
}
