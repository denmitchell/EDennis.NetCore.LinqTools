using EDennis.NetCore.LinqTools;
using EDennis.NetCoreTestingUtilities.Extensions;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Contoso.Tests {
    public class StudentTests {


        private readonly ITestOutputHelper _output;

        public StudentTests(ITestOutputHelper output) {
            _output = output;
        }

        internal class FilterSortPageContosoStudents : FilterSortPage<Student> {
            public FilterSortPageContosoStudents(
                string sortOrder = "Name", string searchString = null, int? pageNumber = null) :
                base(sortOrder,
                    new Dictionary<string, SortUnit<Student>> {
                        { "Name", new SortUnit<Student> { Property = "LastName", Direction = SortDirection.Ascending } },
                        { "name_desc", new SortUnit<Student> { Property = "LastName", Direction = SortDirection.Descending } },
                        { "Date", new SortUnit<Student> { Property = "EnrollmentDate", Direction = SortDirection.Ascending } },
                        { "date_desc", new SortUnit<Student> { Property = "EnrollmentDate", Direction = SortDirection.Descending } }
                    },
                    searchString,
                    new string[] { "LastName", "FirstMidName" },
                    pageNumber, 3) { }
        }


        [Theory]
        [InlineData("date_desc","an",1)]
        public void Test1(string sortOrder, string searchString, int? pageNumber) {
            var fsp = new FilterSortPageContosoStudents(sortOrder, searchString, pageNumber);
            var filteredList = fsp.ApplyTo(StudentRepo.Students, out int pageCount).ToList();

            var filteredStudentsJson = JToken.FromObject(filteredList).ToString();
            _output.WriteLine(filteredStudentsJson);

            var expectedJson = File.ReadAllText("TestCases\\Expected.json");
            var expected = JToken.Parse(expectedJson).ToObject<List<Student>>();
            Assert.True(filteredList.IsEqualOrWrite(expected,_output));
        }
    }
}
