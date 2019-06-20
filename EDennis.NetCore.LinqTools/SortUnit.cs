using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EDennis.NetCore.LinqTools {
    public class SortUnit<TEntity>{
        public string Property { get; set; }
        public SortDirection Direction { get; set; }

    }
}
