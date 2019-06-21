﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class FilterRow<TEntity> : List<FilterUnit<TEntity>> 
        where TEntity: class {

        public Expression GetExpression(ParameterExpression pe) {            
                Expression and = Expression.Constant(true);
                foreach (var e in this) {
                    and = Expression.And(and, e.GetExpression(pe));
                }
                return and;
        } 
    }
}