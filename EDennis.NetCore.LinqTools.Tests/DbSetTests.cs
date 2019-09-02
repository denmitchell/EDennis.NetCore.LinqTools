using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.Samples.ColorsRepo.EfCore.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.NetCore.LinqTools.Tests {
    public class DbSetTests{

        private readonly ITestOutputHelper _output;

        public DbSetTests(ITestOutputHelper output) {
            _output = output;
        }


        [Theory]
        [InlineData("TestCases\\FilteringSortingPagingTests\\Eq")]
        [InlineData("TestCases\\FilteringSortingPagingTests\\Contains")]
        [InlineData("TestCases\\FilteringSortingPagingTests\\StartsWith")]
        [InlineData("TestCases\\FilteringSortingPagingTests\\CommaDelim")]
        public void Test(string folder) {
            var input = File.ReadAllText($"{folder}\\Input.json");
            var expectedJson = File.ReadAllText($"{folder}\\Expected.json");
            var expected = JToken.Parse(expectedJson).ToObject<List<Color>>();

            var filterSortPage = JToken.Parse(input).ToObject<FilterSortPageSelect<Color>>();

            using (var context = new ColorDb2Context()) {
                var colors = context.Colors;
                var filteredColors = filterSortPage.ApplyTo(colors);

                var filteredColorsJson = JToken.FromObject(filteredColors).ToString();
                _output.WriteLine(filteredColorsJson);

                Assert.True(filteredColors.IsEqualOrWrite(expected, _output));
            }
        }
    }
}
