# EDennis.NetCore.LinqTools
Dynamically applies filtering, sorting, and paging to IEnumerable, IQueryable, and DbSet objects using a JSON-friendly spec. 

## The Filtering/Sorting/Paging Spec
The specification for filtering, sorting, and paging is represented by a simple, JSON-friendly hierarchical structure.

### Example Specification as JSON
```json
{
   filter: 
   [
      [
         {
            property: "Name",
            operation: "Eq",
            stringValue: "AliceBlue"
         }
      ],
      [
         {
            property: "Red",
            operation: "Eq",
            stringValue: "0"
         },
         {
            property: "Blue",
            operation: "Eq",
            stringValue: "0"
         }
      ]
   ],
   sort: 
   [
      {
         property: "Red",
         direction: "Ascending"
      },
      {
         property: "Green",
         direction: "Ascending"
      }
   ],
   page: 
   {
      pageNumber: 2,
      pageSize: 3
   }
}
```
### Filtering Spec
The filtering specification is a two-dimensional array of objects, which can be interpreted as a filter table.  Each row in the filter table (an outer array element) represents an intersection (ANDed) over one or more expression units.  Each expression unit consists of a property name, and operation, and a literal value (as a string).  Currently supported operations are: Eq, Lt, Le, Gt, Ge, Contains, StartsWith, and EndsWith. All filter rows are unioned (ORed) to provide the complete filter.  This is analogous to the filtering table in the design mode of a Microsoft Access query.

In the above example, we select all objects where Name = "AliceBlue" or where both Red and Blue properties were equal to zero.  

### Sorting Spec
The sorting specification is a one-dimensional array of objects, where each object provides a property name and sort direction.

In the above example, we sort by Red (ascending) and then by Green (ascending) 

### Paging Spec
The paging specification is a single object consisting of a page number and page size.

In the above example, we return the second page, where each page holds three objects.

## Other Examples
The test project in this solution includes other examples of filtering, sorting, and paging.
