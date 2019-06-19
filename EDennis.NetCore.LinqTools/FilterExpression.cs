using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class FilterExpression<TEntity>
        where TEntity: class {
        public string Property { get; set; }
        public string Operation { get; set; }
        public string StringValue { get; set; }

        public Expression Expression {

            get {
                var type = typeof(TEntity);
                var pe = Expression.Parameter(type, "e");

                var propertyInfo = type.GetProperty(Property);
                object objVal = Convert.ChangeType(StringValue, propertyInfo.PropertyType);
                var left = Expression.Property(pe, type.GetProperty(Property));
                var right = Expression.Constant(objVal);

                switch (Operation) {
                    case "Eq":
                        return Expression.Equal(left, right);
                    case "Lt":
                        return Expression.LessThan(left, right);
                    case "Le":
                        return Expression.LessThanOrEqual(left, right);
                    case "Gt":
                        return Expression.GreaterThan(left, right);
                    case "Ge":
                        return Expression.GreaterThanOrEqual(left, right);
                    case "Like":
                        return Expression.Call(left, "Contains", Type.EmptyTypes, right); 

                }


                return null;
            }

        }
    }
}
