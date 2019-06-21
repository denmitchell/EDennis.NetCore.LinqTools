using EDennis.NetCoreTestingUtilities.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.NetCore.LinqTools.Tests {
    public class FilteringSortingPagingTests{

        private readonly ITestOutputHelper _output;

        public FilteringSortingPagingTests(ITestOutputHelper output) {
            _output = output;
        }


        [Theory]
        [InlineData("TestCases\\FilteringSortingPagingTests\\Eq")]
        [InlineData("TestCases\\FilteringSortingPagingTests\\Contains")]
        public void Test(string folder) {
            var input = File.ReadAllText($"{folder}\\Input.json");
            var expected = File.ReadAllText($"{folder}\\Expected.json");

            var filterSortPage = JToken.Parse(input).ToObject<FilterSortPage<Color>>();

            var colors = ColorRepo.Colors;
            var filteredColors = filterSortPage.ApplyTo(colors);

            Assert.True(filteredColors.IsEqualOrWrite(expected,_output));

        }
    }
}
