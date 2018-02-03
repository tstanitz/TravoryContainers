using System;
using System.Collections.Generic;
using TravoryContainers.Services.Flickr.API.Controllers;
using TravoryContainers.Services.Flickr.API.Helpers;
using Xunit;

namespace UnitTest.Flickr
{
    public class DateCalculatorTest
    {
        [Theory]
        [MemberData(nameof(GetFromToDates))]
        public void GetDate_ReturnsDateFromString(string dateString, DateTime? expectedDate = null)
        {
            IDateCalculator calculator = new DateCalculator();

            var date = calculator.GetDate(dateString);

            Assert.Equal(expectedDate, date);
        }

        public static IEnumerable<object[]> GetFromToDates()
        {
            yield return new object[] { "2017. 05. 30.", new DateTime(2017, 5, 30) };
            yield return new object[] { "" };
            yield return new object[] { null };
        }
    }
}