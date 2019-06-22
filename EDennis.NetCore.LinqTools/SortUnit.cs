namespace EDennis.NetCore.LinqTools {

    /// <summary>
    /// Represents an atomic unit for sorting
    /// </summary>
    /// <typeparam name="TEntity">The model class</typeparam>
    public class SortUnit<TEntity>{
        public string Property { get; set; }
        public SortDirection Direction { get; set; }

    }
}
