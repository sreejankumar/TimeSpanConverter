using System;
using System.Collections.Generic;
using TimeSpanConverter.Enum;
using TimeSpanConverter.Model;
using TimeZoneConverter;

namespace TimeSpanConverter
{
    public static class TimeSpanConverter
    {
        private static readonly TimeSpan Noon = new TimeSpan(12, 0, 0);
        private static readonly TimeSpan MidNight = new TimeSpan(0, 0, 0);
        private static readonly TimeSpan OneMinBeforeMidNight = new TimeSpan(23, 59, 59);
        private static readonly DateTime DateTime = DateTime.Today;

        /// <summary>
        /// Get your timespan filters in UTC if you want to search based on AM OR PM.
        /// </summary>
        /// <param name="dayPeriod"></param>
        /// <param name="timeZone"></param>
        /// <returns></returns>
        public static List<Filters> GetTimeSpansFromDayPeriod(DayPeriods dayPeriod, string timeZone)
        {
            var convertedTimezone = GetTimeZoneInfo(timeZone);

            var noonTimeToUtc = ConvertTimeToUtcUsingTimeZone(Noon, convertedTimezone);
            var midnightTimeToUtc = ConvertTimeToUtcUsingTimeZone(MidNight, convertedTimezone);
            var oneMinBeforeMidNightToUtc = ConvertTimeToUtcUsingTimeZone(OneMinBeforeMidNight, convertedTimezone);

            if (dayPeriod == DayPeriods.Am)
            {
                if (noonTimeToUtc.Date == midnightTimeToUtc.Date)
                {
                    return new List<Filters>
                    {
                        SetTimeZone(midnightTimeToUtc, noonTimeToUtc)
                    };
                }

                return SetTimeZones(midnightTimeToUtc, noonTimeToUtc);
            }

            if (dayPeriod != DayPeriods.Pm) return new List<Filters>();
            if (noonTimeToUtc.Date == oneMinBeforeMidNightToUtc.Date)
            {
                return new List<Filters>
                {
                    SetTimeZone(noonTimeToUtc, oneMinBeforeMidNightToUtc)
                };
            }

            return SetTimeZones(noonTimeToUtc, oneMinBeforeMidNightToUtc);
        }

        /// <summary>
        /// Set a single timezone
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private static Filters SetTimeZone(DateTime from, DateTime to) =>
            new Filters
            {
                From = @from.TimeOfDay.Ticks,
                To = to.TimeOfDay.Ticks
            };

        /// <summary>
        /// Set the Timezones data
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private static List<Filters> SetTimeZones(DateTime from, DateTime to) =>
            new List<Filters>
            {
                SetTimeZone(@from, GetCurrentDateTime(OneMinBeforeMidNight)),
                SetTimeZone(GetCurrentDateTime(MidNight), to)
            };

        /// <summary>
        /// Convert the Datetime to Utc using Timezone
        /// </summary>
        /// <param name="span"></param>
        /// <param name="zoneInfo"></param>
        /// <returns></returns>
        public static DateTime ConvertTimeToUtcUsingTimeZone(TimeSpan span, TimeZoneInfo zoneInfo)
        {
            var dateTimeUnspecific = DateTime.SpecifyKind(GetCurrentDateTime(span), DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTime(dateTimeUnspecific, zoneInfo, TimeZoneInfo.Utc);
        }

        private static DateTime GetCurrentDateTime(TimeSpan span) => DateTime.Add(span);

        /// <summary>
        /// Attempts to retrieve a <see cref="T:System.TimeZoneInfo" /> object given a valid Windows or IANA time zone identifier,
        /// regardless of which platform the application is running on. If it cant find, default it to UTC.
        /// </summary>
        /// <param name="tzTimeZoneId"></param>
        /// <returns></returns>
        public static TimeZoneInfo GetTimeZoneInfo(string tzTimeZoneId)
        {
            var windowsTimeZone = TimeZoneInfo.Utc;

            return !TZConvert.TryGetTimeZoneInfo(tzTimeZoneId, out var windowsTimeZoneId)
                ? windowsTimeZone
                : windowsTimeZoneId;
        }
        /// <summary>
        /// Validate a Timezone 
        /// </summary>
        /// <param name="timezone"></param>
        /// <returns></returns>
        public static bool ValidTimezone(string timezone)
        {
            return TZConvert.TryGetTimeZoneInfo(timezone, out _);
        }
    }
}
