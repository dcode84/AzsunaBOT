using AzsunaBOT.Enums;
using System;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers
{
    public static class DateFormatter
    {
        private static DateTime? _date;
        private static string _formattedDate;
        private static DayOfWeek dayOfWeek;

        public static async Task<string> FormatDate(string day, DateTime? passedDate = null)
        {
            var woeday = day;

            if (Enum.IsDefined(typeof(WoedaysShort), woeday.ToUpper()))
            {
                woeday = ConvertWeekDay(day).Result;
            }


            return await Task.Run(() =>
            {

                if (passedDate == null)
                {
                    switch (woeday.ToLower())
                    {
                        case "monday":
                            _date = GetNextWeekday(DateTime.UtcNow, DayOfWeek.Monday);
                            break;
                        case "tuesday":
                            _date = GetNextWeekday(DateTime.UtcNow, DayOfWeek.Tuesday);
                            break;
                        case "wednesday":
                            _date = GetNextWeekday(DateTime.UtcNow, DayOfWeek.Wednesday);
                            break;
                        case "thursday":
                            _date = GetNextWeekday(DateTime.UtcNow, DayOfWeek.Thursday);
                            break;
                        case "friday":
                            _date = GetNextWeekday(DateTime.UtcNow, DayOfWeek.Friday);
                            break;
                        case "saturday":
                            _date = GetNextWeekday(DateTime.UtcNow, DayOfWeek.Saturday);
                            break;
                        case "sunday":
                            _date = GetNextWeekday(DateTime.UtcNow, DayOfWeek.Sunday);
                            break;

                        default:
                            _date = null;
                            break;
                    }
                }
                else
                {
                    _date = passedDate;
                }

                if (_date != null)
                {
                    _formattedDate = $"{woeday.ToLower()}_" + string.Format($"{_date:ddMMyyyy}");
                }

                return _formattedDate;
            });
        }

        public static async Task<string> ConvertWeekDay(string day)
        {
            string convertedDay = "";

            return await Task.Run(() =>
            {
                switch (day)
                {
                    case "mon":
                        convertedDay = "Monday";
                        break;
                    case "tue":
                        convertedDay = "Tuesday";
                        break;
                    case "wed":
                        convertedDay = "Wednesday";
                        break;
                    case "thu":
                        convertedDay = "Thursday";
                        break;
                    case "fri":
                        convertedDay = "Friday";
                        break;
                    case "sat":
                        convertedDay = "Saturday";
                        break;
                    case "sun":
                        convertedDay = "Sunday";
                        break;

                    default:
                        break;
                }

                return convertedDay;
            });

        }

        public static DateTime GetNextWeekday(DateTime currentDay, DayOfWeek aimedDay)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)aimedDay - (int)currentDay.DayOfWeek + 7) % 7;

            return currentDay.AddDays(daysToAdd);
        }

        public static async Task<DayOfWeek> GetDayOfWeekAsync(string day)
        {
            day = ConvertWeekDay(day).Result;

            return await Task.Run(() =>
            {
                switch (day.ToLower())
                {
                    case "monday":
                        dayOfWeek = DayOfWeek.Monday;
                        break;
                    case "tuesday":
                        dayOfWeek = DayOfWeek.Tuesday;
                        break;
                    case "wednesday":
                        dayOfWeek = DayOfWeek.Wednesday;
                        break;
                    case "thursday":
                        dayOfWeek = DayOfWeek.Thursday;
                        break;
                    case "friday":
                        dayOfWeek = DayOfWeek.Friday;
                        break;
                    case "saturday":
                        dayOfWeek = DayOfWeek.Saturday;
                        break;
                    case "sunday":
                        dayOfWeek = DayOfWeek.Sunday;
                        break;

                    default:
                        break;
                }

                return dayOfWeek;
            });

        }
    }
}
