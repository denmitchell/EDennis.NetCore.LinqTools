using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace EDennis.NetCore.LinqTools.Tests {
    public class FilteringSortingPagingTests{

        [Theory]
        [InlineData("TestCases\\FilteringSortingPagingTests\\A")]
        public void Test(string folder) {
            var input = File.ReadAllText($"{folder}\\Input.json");
            var expected = File.ReadAllText($"{folder}\\Expected.json");

            var filterSortPage = JToken.Parse(input).ToObject<FilterSortPage<Color>>();

            var colors = ColorRepo.Colors;
            var filteredColors = filterSortPage.ApplyTo(colors);

        }
    }
}
