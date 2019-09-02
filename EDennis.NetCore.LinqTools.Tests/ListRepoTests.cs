using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.ColorsRepo.List;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.NetCore.LinqTools.Tests {
    public class ListRepoTests {

        private readonly ITestOutputHelper _output;

        public ListRepoTests(ITestOutputHelper output) {
            _output = output;
        }


        [Theory]
        [InlineData("TestCases\\FilteringSortingPagingTests\\Eq")]
        [InlineData("TestCases\\FilteringSortingPagingTests\\Contains")]
        [InlineData("TestCases\\FilteringSortingPagingTests\\StartsWith")]
        [InlineData("TestCases\\FilteringSortingPagingTests\\CommaDelim")]
        public void TestFSP(string folder) {
            var input = File.ReadAllText($"{folder}\\Input.json");
            var expectedJson = File.ReadAllText($"{folder}\\Expected.json");

            var filterSortPageSelect = JToken.Parse(input).ToObject<FilterSortPageSelect<Color>>();

            var colors = ColorRepo.Colors.AsQueryable();
            var filteredColors = filterSortPageSelect.ApplyTo(colors);

            var expected = JToken.Parse(expectedJson).ToObject(filteredColors.GetType());

            Assert.True(filteredColors.IsEqualOrWrite(expected, _output));

        }

        [Theory]
        [InlineData("TestCases\\FilteringSortingPagingSelectingTests\\Eq")]
        [InlineData("TestCases\\FilteringSortingPagingSelectingTests\\Contains")]
        [InlineData("TestCases\\FilteringSortingPagingSelectingTests\\StartsWith")]
        [InlineData("TestCases\\FilteringSortingPagingSelectingTests\\CommaDelim")]
        public void TestFSPS(string folder) {
            var input = File.ReadAllText($"{folder}\\Input.json");
            var expectedJson = File.ReadAllText($"{folder}\\Expected.json");

            var filterSortPageSelect = JToken.Parse(input).ToObject<FilterSortPageSelect<Color>>();

            var colors = ColorRepo.Colors.AsQueryable();
            var filteredColors = filterSortPageSelect.ApplyTo(colors);

            var expected = JToken.Parse(expectedJson).ToObject(filteredColors.GetType());


            Assert.True(filteredColors.IsEqualOrWrite(expected, _output));

        }


    }


}
