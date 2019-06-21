using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EDennis.NetCore.LinqTools {

    public enum StringFilterType {
        StartsWith, EndsWith, Contains
    }

    public class FilterUnit<TEntity>
        where TEntity : class {
        public string Property { get; set; }
        public FilterOperation Operation { get; set; }
        public string StringValue { get; set; }

        public Expression GetExpression(ParameterExpression pe) {

            var type = typeof(TEntity);
            //var pe = Expression.Parameter(type, "e");

            var propertyInfo = type.GetProperty(Property);
            if (propertyInfo == null)
                throw new ArgumentException($"Property \"{Property},\" specified in the filter, does not exist in {typeof(TEntity).Name}");
            object objVal = Convert.ChangeType(StringValue, propertyInfo.PropertyType);
            var left = Expression.Property(pe, type.GetProperty(Property));
            var right = Expression.Constant(objVal);

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


            return null;
        }

    }
}
