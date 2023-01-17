using AzsunaBOT.Data;
using AzsunaBOT.Enums;
using AzsunaBOT.Helpers.Message;
using DataLibrary.DataAccess;
using DataLibrary.Models;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers
{
    public static class RosterHelper
    {
        public static DiscordClient DiscordClient { get; set; }

        public static WoeDaysModel LoadWoeDay(CommandContext context, string day, string mode, IDataAccess data, IConfiguration config)
        {
            var tblName = BuildDaysTableNameString(context).Result;
            var dataTableName = BuildDataTableNameString(context, day, mode).Result;

            return data.LoadData<WoeDaysModel, dynamic>("sp_adminGetWoeDay",
                new 
                { 
                    @tblName = tblName, 
                    @dataTableName = dataTableName,
                    @mode = mode 
                },
                config.GetConnectionString("AzsunaBOT")).Result.First();
        }

        public static async void RefreshRostersAsync(CommandContext context, IDataAccess _data, IConfiguration _config)
        {
            try
            {
                await RoleLists.ClearLists();

                await RefreshWoeDaysAsync(context, DiscordClient, _data, _config);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        public static async Task<SignUpModel> FillSignUpModel(CommandContext context, string day, string mode, string charName, string role, string sign, string comments)
        {
            return await Task.Run(() =>
            {
                var model = new SignUpModel()
                {
                    Info = BuildDataTableNameString(context, day, mode).Result,
                    DiscordTag = context.Member.Mention,
                    CharName = charName,
                    Role = role,
                    Sign = sign,
                    Comments = comments
                };
                return model;
            });
        }

        private static async Task RefreshWoeDaysAsync(CommandContext context, DiscordClient client, IDataAccess _data, IConfiguration _config)
        {
            var woeDates = new List<WoeDaysModel>();

            try
            {
                woeDates = _data.LoadData<WoeDaysModel, dynamic>("sp_adminLookUp", 
                    new { @tblName = BuildDaysTableNameString(context).Result}, 
                    _config.GetConnectionString("AzsunaBOT")).Result;
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            foreach (var woeDay in woeDates)
            {
                try
                {
                    var attendanceList = new List<AttendanceModel>();

                    attendanceList =
                        _data.LoadData<AttendanceModel, dynamic>("sp_adminLookUp", 
                            new { @tblName = woeDay.DataTableName }, 
                            _config.GetConnectionString("AzsunaBOT")).Result;

                    await RosterMessager.BuildRosterMessage(client, attendanceList, woeDay.Day, woeDay.ChannelId);
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }

        private static async Task<string> BuildDaysTableNameString(CommandContext context)
        {
            return await Task.Run(() =>
            {
                return $"{context.Guild.Name.ToLower()}_woedays";
            });
        }

        private static async Task<string> BuildDataTableNameString(CommandContext context, string day, string mode)
        {
            return await Task.Run(() => 
            {
                if (!Enum.IsDefined(typeof(WoeMode), mode.ToUpper()))
                    return null;

                var cache = $"{context.Guild.Name.ToLower()}_{mode.ToLower()}_{DateFormatter.FormatDate(day).Result}";

                return cache;
            });
        }

        private static Task<WoeDaysModel> PrepareWoeModel(string woeDay)
        {
            return Task.Run(() =>
            {
                var model = new WoeDaysModel()
                {
                    Day = DateFormatter.ConvertWeekDay(woeDay).Result,
                    DataTableName = DateFormatter.FormatDate(woeDay).Result
                };

                return model;
            });
        }

        public static async Task SetWoeDateAsync(string woeDay, IDataAccess _data, IConfiguration _config)
        {
            await Task.Run(async () =>
            {
                var model = PrepareWoeModel(woeDay).Result;

                try
                {
                    await _data.SaveData<WoeDaysModel>("sp_adminAddWoeDate", model, _config.GetConnectionString("AzsunaBOT"));
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            });
        }

        public static async Task DeleteWoeDateAsync(string woeDay, IDataAccess _data, IConfiguration _config)
        {
            await Task.Run(async () =>
            {
                var model = PrepareWoeModel(woeDay).Result;

                try
                {
                    await _data.SaveData<WoeDaysModel>("sp_adminDeleteWoeDate", model, _config.GetConnectionString("AzsunaBOT"));
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
                    var tblName = new TableNameModel() { TblName = woeDay };

                    await _data.SaveData<TableNameModel>("sp_adminCreateAttendance", 
                        tblName, _config.GetConnectionString("AzsunaBOT"));
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            });
        }
    }
}
