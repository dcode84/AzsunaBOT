using AzsunaBOT.Enums;
using AzsunaBOT.Helpers;
using AzsunaBOT.Helpers.Message;
using DataLibrary.DataAccess;
using DataLibrary.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AzsunaBOT.Commands
{
    public class UserAttendanceCommands : BaseCommandModule
    {
        private readonly IDataAccess _data;
        private readonly IConfiguration _config;

        public UserAttendanceCommands(IDataAccess data, IConfiguration config)
        {
            _data = data;
            _config = config;
        }

        // Needs to be restricted to a channel
        [RequireRoles(RoleCheckMode.Any, "woe", "guest")]
        [Command("setrole")]
        public async Task Setrole(CommandContext context, string day, string mode, string charName, string role, string sign, string comments = "")
        {
            if (string.IsNullOrEmpty(day) || string.IsNullOrEmpty(mode) || string.IsNullOrEmpty(charName) || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(sign))
            {
                await AttendanceMessager.IncorrectCredentialsMessage(context);
                return;
            }

            if (!await AttendanceHelper.IsValueOfEnum(context, day, mode, role, sign))
                return;

            try
            {
                var woeDay = RosterHelper.LoadWoeDay(context, day, mode, _data, _config);

                if (woeDay.IsClosed == true)
                {
                    await AttendanceMessager.SignUpBlocked(context, day, mode);
                    return;
                }

                try
                {
                    var signUpModel = await RosterHelper.FillSignUpModel(context, day, mode, charName, role, sign, comments);
                    //await _data.SaveData<SignUpModel>("sp_userWoeSignUp", signUpModel, _config.GetConnectionString("AzsunaBOT"));
                    RosterHelper.RefreshRostersAsync(context, _data, _config);
                }
                catch (Exception e)
                {
                    await AttendanceMessager.AlreadySignedMessage(context);
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                await AttendanceMessager.NotFound(context, day);
                Console.WriteLine(e.Message);
            }
        }
    }
}
