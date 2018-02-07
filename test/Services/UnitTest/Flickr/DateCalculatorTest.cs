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

        [Theory]
        [MemberData(nameof(GetTimes))]
        public void GetDateAndTime_ReturnsDateAndTimeWithoutMsFromString(string dateTimeString, DateTime? expectedDateTime = null)
        {
            IDateCalculator calculator = new DateCalculator();

            var dateTime = calculator.GetDateAndTime(dateTimeString);

            Assert.Equal(expectedDateTime, dateTime);
        }

        public static IEnumerable<object[]> GetFromToDates()
        {
            yield return new object[] { "2017-07-16 08:11:15", new DateTime(2017, 7, 16) };
            yield return new object[] { "2017. 05. 30.", new DateTime(2017, 5, 30) };
            yield return new object[] { "" };
            yield return new object[] { null };
        }
        public static IEnumerable<object[]> GetTimes()
        {
            yield return new object[] { "2017-07-16 08:11:15", new DateTime(2017, 7, 16, 8, 11, 15) };
            yield return new object[] { "" };
            yield return new object[] { null };
        }
    }
}