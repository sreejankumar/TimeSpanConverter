using System;
using FluentAssertions;
using NUnit.Framework;
using TimeSpanConverter.Enum;
using TimeZoneConverter;

namespace TimeSpanConverter.UnitTests
{
    [TestFixture]
    public class TimeSpanConverterTests
    {

        [Test]
        public void GetTimeZoneInfo_Empty_Input()
        {
            const string input = "";
            var result = TimeSpanConverter.GetTimeZoneInfo(input);

            result.Should().Be(TimeZoneInfo.Utc);
        }

        [Test]
        public void GetTimeZoneInfo_UK_Input()
        {
            const string input = "Europe/London";
            var result = TimeSpanConverter.GetTimeZoneInfo(input);
            result.Should().Be(TZConvert.GetTimeZoneInfo("GMT Standard Time"));
        }

        [Test]
        public void GetTimeSpansFromDayPeriod_For_US_AM_Test()
        {
            const string input = "America/New_York";
            var timeZones = TimeSpanConverter.GetTimeSpansFromDayPeriod(DayPeriods.Am, input);

            var from = new DateTime(timeZones[0].From);
            var to = new DateTime(timeZones[0].To);

            // Midnight in US is 05:00:00
            // Noon in US is 17:00:00
            timeZones.Count.Should().Be(1);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 5, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 17, 0, 0).TimeOfDay);
        }

        [Test]
        public void GetTimeSpansFromDayPeriod_For_US_PM_Test()
        {
            const string input = "America/New_York";
            var timeZones = TimeSpanConverter.GetTimeSpansFromDayPeriod(DayPeriods.Pm, input);

            var from = new DateTime(timeZones[0].From);
            var to = new DateTime(timeZones[0].To);

            timeZones.Count.Should().Be(2);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            // Noon in US is 17:00:00
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 17, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 23, 59, 59).TimeOfDay);

            from = new DateTime(timeZones[1].From);
            to = new DateTime(timeZones[1].To);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            // Midnight in US is 05:00:00
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 0, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 4, 59, 59).TimeOfDay);
        }

        [Test]
        public void GetTimeSpansFromDayPeriod_For_UK_AM_Test()
        {
            const string input = "Europe/London";
            var timeZones = TimeSpanConverter.GetTimeSpansFromDayPeriod(DayPeriods.Am, input);

            var from = new DateTime(timeZones[0].From);
            var to = new DateTime(timeZones[0].To);

            timeZones.Count.Should().Be(1);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            from.TimeOfDay.Should().Be(GetDateTimeToUtc(new DateTime(1, 1, 1, 0, 0, 0), input).TimeOfDay);
            to.TimeOfDay.Should().Be(GetDateTimeToUtc(new DateTime(1, 1, 1, 12, 0, 0), input).TimeOfDay);
        }

        [Test]
        public void GetTimeSpansFromDayPeriod_For_UK_PM_Test()
        {
            const string input = "Europe/London";
            var timeZones = TimeSpanConverter.GetTimeSpansFromDayPeriod(DayPeriods.Pm, input);

            var from = new DateTime(timeZones[0].From);
            var to = new DateTime(timeZones[0].To);

            timeZones.Count.Should().Be(1);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            from.TimeOfDay.Should().Be(GetDateTimeToUtc(new DateTime(1, 1, 1, 12, 0, 0), input).TimeOfDay);
            to.TimeOfDay.Should().Be(GetDateTimeToUtc(new DateTime(1, 1, 1, 23, 59, 59), input).TimeOfDay);
        }

        [Test]
        public void GetTimeSpansFromDayPeriod_For_AUS_AM_Test()
        {
            const string input = "Australia/Sydney";
            var timeZones = TimeSpanConverter.GetTimeSpansFromDayPeriod(DayPeriods.Am, input);

            var from = new DateTime(timeZones[0].From);
            var to = new DateTime(timeZones[0].To);

            timeZones.Count.Should().Be(2);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            // Noon in AUS is 13:00:00
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 13, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 23, 59, 59).TimeOfDay);

            from = new DateTime(timeZones[1].From);
            to = new DateTime(timeZones[1].To);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            // Midnight in AUS is 01:00:00
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 0, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 1, 0, 0).TimeOfDay);
        }

        [Test]
        public void GetTimeSpansFromDayPeriod_For_AUS_PM_Test()
        {
            const string input = "Australia/Sydney";
            var timeZones = TimeSpanConverter.GetTimeSpansFromDayPeriod(DayPeriods.Pm, input);

            var from = new DateTime(timeZones[0].From);
            var to = new DateTime(timeZones[0].To);

            timeZones.Count.Should().Be(1);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            // Midnight in AUS is 13:00:00
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 01, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 12, 59, 59).TimeOfDay);
        }

        [Test]
        public void GetTimeSpansFromDayPeriod_For_NZ_AM_Test()
        {
            const string input = "Pacific/Auckland";
            var timeZones = TimeSpanConverter.GetTimeSpansFromDayPeriod(DayPeriods.Am, input);

            var from = new DateTime(timeZones[0].From);
            var to = new DateTime(timeZones[0].To);

            timeZones.Count.Should().Be(1);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            // Noon in NZ is 23:00:00
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 11, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 23, 0, 0).TimeOfDay);
        }

        [Test]
        public void GetTimeSpansFromDayPeriod_For_NZ_PM_Test()
        {
            const string input = "Pacific/Auckland";
            var timeZones = TimeSpanConverter.GetTimeSpansFromDayPeriod(DayPeriods.Pm, input);

            var from = new DateTime(timeZones[0].From);
            var to = new DateTime(timeZones[0].To);

            timeZones.Count.Should().Be(2);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            // Noon in NZ is 23:00:00
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 23, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 23, 59, 59).TimeOfDay);

            from = new DateTime(timeZones[1].From);
            to = new DateTime(timeZones[1].To);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            //Midnight in NZ is 11:00:00
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 0, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 10, 59, 59).TimeOfDay);
        }

        [Test]
        public void GetTimeSpansFromDayPeriod_For_Singapore_AM_Test()
        {
            const string input = "Asia/Singapore";
            var timeZones = TimeSpanConverter.GetTimeSpansFromDayPeriod(DayPeriods.Am, input);

            var from = new DateTime(timeZones[0].From);
            var to = new DateTime(timeZones[0].To);

            timeZones.Count.Should().Be(2);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);


            //Noon in Singapore is 04:00:00
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 16, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 23, 59, 59).TimeOfDay);

            from = new DateTime(timeZones[1].From);
            to = new DateTime(timeZones[1].To);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            // Midnight in NZ is 16:00:00
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 0, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 4, 0, 0).TimeOfDay);
        }

        [Test]
        public void GetTimeSpansFromDayPeriod_For_Singapore_PM_Test()
        {
            const string input = "Asia/Singapore";
            var timeZones = TimeSpanConverter.GetTimeSpansFromDayPeriod(DayPeriods.Pm, input);

            var from = new DateTime(timeZones[0].From);
            var to = new DateTime(timeZones[0].To);

            timeZones.Count.Should().Be(1);
            from.TimeOfDay.Should().BeLessThan(to.TimeOfDay);

            //Midnight in Singapore is 04:00:00
            // Noon in NZ is 16:00:00
            from.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 04, 0, 0).TimeOfDay);
            to.TimeOfDay.Should().Be(new DateTime(1, 1, 1, 15, 59, 59).TimeOfDay);
        }


        [TestCase("random")]
        [TestCase("random/string")]
        [TestCase("London")]
        [TestCase("Europe")]
        [TestCase("UK")]
        [TestCase("US")]
        [TestCase("America / Sao_Paulo")]
        public void Validate_TimeZone_Exception(string input)
        {
            var result = TimeSpanConverter.ValidTimezone(input);
            result.Should().BeFalse();
        }

        [TestCase("America/Sao_Paulo")]
        [TestCase("Asia/Singapore")]
        public void Validate_TimeZone_Success(string input)
        {
            var result = TimeSpanConverter.ValidTimezone(input);
            result.Should().BeTrue();
        }

        private static DateTime GetDateTimeToUtc(DateTime time, string timezone)
        {
            var timeZoneInfo = TimeSpanConverter.GetTimeZoneInfo(timezone);
            return TimeSpanConverter.ConvertTimeToUtcUsingTimeZone(time.TimeOfDay, timeZoneInfo);
        }
    }
}