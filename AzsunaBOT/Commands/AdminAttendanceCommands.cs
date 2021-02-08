using AzsunaBOT.Enums;
using AzsunaBOT.Helpers;
using AzsunaBOT.Helpers.Message;
using DataLibrary.DataAccess;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AzsunaBOT.Commands
{
    public class AdminAttendanceCommands : BaseCommandModule
    {
        private readonly IDataAccess _data;
        private readonly IConfiguration _config;


        public AdminAttendanceCommands(IDataAccess data, IConfiguration config)
        {
            _data = data;
            _config = config;
        }

        [RequireRoles(RoleCheckMode.Any, "lead")]
        [Description("Currently only supports ONE woe date at a time!")]
        [Command("setwoe")]
        public async Task CreateAttendance(CommandContext context, string woeDay, DateTime? date = null)
        {
            if (!string.IsNullOrEmpty(woeDay) && Enum.IsDefined(typeof(WoedaysShort), woeDay.ToUpper()))
            {
                try
                {
                    await RosterHelper.CreateAttendanceAsync(woeDay, _data, _config, date);
                    await RosterHelper.SetWoeDateAsync(woeDay, _data, _config);
                    await RosterMessager.SetWoeMessage(context, woeDay, date);
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            else
                await RosterMessager.IncorrectCredentialsMessage(context);
        }

        [RequireRoles(RoleCheckMode.Any, "lead")]
        [Command("close")]
        public async Task CloseSignUp(CommandContext context, string woeDay)
        {
            if (!string.IsNullOrEmpty(woeDay) && Enum.IsDefined(typeof(WoedaysShort), woeDay.ToUpper()))
            {
                await RosterHelper.DeleteWoeDateAsync(woeDay, _data, _config);
                await RosterMessager.WoeClosedMessage(context, woeDay);
            }
            else
                await RosterMessager.IncorrectCredentialsMessage(context);
        }
    }
}
