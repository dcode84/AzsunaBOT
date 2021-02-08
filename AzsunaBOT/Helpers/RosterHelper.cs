using AzsunaBOT.Data;
using AzsunaBOT.Helpers.Message;
using DataLibrary.DataAccess;
using DataLibrary.Models;
using DSharpPlus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers
{
    public static class RosterHelper
    {
        public static DiscordClient DiscordClient { get; set; }

        private static string woeDays = "woedays";


        public static async void RefreshRostersAsync(IDataAccess _data, IConfiguration _config)
        {
            // Refresh needs to happen in a specific channel
            try
            {
                await RoleLists.ClearLists();

                await RefreshWoeDaysAsync(DiscordClient, _data, _config);

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        public static async Task SetWoeDateAsync(string woeDay, IDataAccess _data, IConfiguration _config)
        {
            await Task.Run(async () =>
            {
                //var model = new WoeModel()
                //{
                //    Day = DateFormatter.ConvertWeekDay(woeDay).Result,
                //    DateString = DateFormatter.FormatDate(woeDay).Result
                //};
                var model = PrepareWoeModel(woeDay).Result;

                try
                {
                    await _data.SaveData<WoeModel>("sp_adminAddWoeDate", model, _config.GetConnectionString("AzsunaBOT"));

                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            });
        }

        public static async Task DeleteWoeDateAsync(string woeDay, IDataAccess _data, IConfiguration _config)
        {
            await Task.Run(async () =>
            {
                //var model = new WoeModel()
                //{
                //    Day = DateFormatter.ConvertWeekDay(woeDay).Result,
                //    DateString = DateFormatter.FormatDate(woeDay).Result
                //}; 
                var model = PrepareWoeModel(woeDay).Result;

                try
                {
                    await _data.SaveData<WoeModel>("sp_adminDeleteWoeDate", model, _config.GetConnectionString("AzsunaBOT"));
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            });
        }

        public static async Task CreateAttendanceAsync(string woeDay, IDataAccess _data, IConfiguration _config, DateTime? date = null)
        {
            await Task.Run(async () =>
            {
                woeDay = DateFormatter.FormatDate(woeDay, date).Result;

                try
                {
                    var tblName = new TableNameModel()
                    {
                        TblName = woeDay
                    };

                    await _data.SaveData<TableNameModel>("sp_adminCreateAttendance", tblName, _config.GetConnectionString("AzsunaBOT"));
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            });
        }

        private static async Task RefreshWoeDaysAsync(DiscordClient client, IDataAccess _data, IConfiguration _config)
        {
            var woeDates = new List<WoeModel>();

            try
            {
                woeDates = _data.LoadData<WoeModel, dynamic>("sp_adminLookUp", new { @tblName = woeDays }, _config.GetConnectionString("AzsunaBOT")).Result;
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            foreach (var day in woeDates)
            {
                try
                {
                    var attendanceList = new List<AttendanceModel>();
                    attendanceList =
                        _data.LoadData<AttendanceModel, dynamic>("sp_adminLookUp", new { @tblName = day.DateString }, _config.GetConnectionString("AzsunaBOT")).Result;

                    await RosterMessage(client, attendanceList, day.Day);
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }

        private static async Task RosterMessage(DiscordClient client, List<AttendanceModel> list, string day)
        {
            await RosterMessager.BuildRosterMessage(client, list, day);
        }

        private static Task<WoeModel> PrepareWoeModel(string woeDay)
        {
            return Task.Run(() =>
            {
                var model = new WoeModel()
                {
                    Day = DateFormatter.ConvertWeekDay(woeDay).Result,
                    DateString = DateFormatter.FormatDate(woeDay).Result
                };

                return model;
            });
        }
    }
}
