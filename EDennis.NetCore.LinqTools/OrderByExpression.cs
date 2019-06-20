using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class OrderByExpression<TEntity,TKey>{
        public string Property { get; set; }
        public OrderByDirection Direction { get; set; }
    }
}
