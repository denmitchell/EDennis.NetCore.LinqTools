using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.ColorsRepo.List;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.NetCore.LinqTools.Tests {
    public class ListRepoTests{

        private readonly ITestOutputHelper _output;

        public ListRepoTests(ITestOutputHelper output) {
            _output = output;
        }


        [Theory]
        [InlineData("TestCases\\FilteringSortingPagingTests\\Eq")]
        [InlineData("TestCases\\FilteringSortingPagingTests\\Contains")]
        [InlineData("TestCases\\FilteringSortingPagingTests\\StartsWith")]
        public void Test(string folder) {
            var input = File.ReadAllText($"{folder}\\Input.json");
            var expectedJson = File.ReadAllText($"{folder}\\Expected.json");
            var expected = JToken.Parse(expectedJson).ToObject<List<Color>>();

            var filterSortPage = JToken.Parse(input).ToObject<FilterSortPage<Color>>();

            var colors = ColorRepo.Colors.AsQueryable();
            var filteredColors = filterSortPage.ApplyTo(colors);

            Assert.True(filteredColors.IsEqualOrWrite(expected,_output));

        }
    }
}
