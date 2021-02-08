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
        public async Task Setrole(CommandContext context, string day, string charName, string role, string sign, string comments = "")
        {
            if (!string.IsNullOrEmpty(day) && !string.IsNullOrEmpty(charName) && !string.IsNullOrEmpty(role) && !string.IsNullOrEmpty(sign))
            {
                if (Enum.IsDefined(typeof(WoedaysShort), day.ToUpper()) && Enum.IsDefined(typeof(RolesAll), role.ToUpper()) && Enum.IsDefined(typeof(Signs), sign.ToUpper()))
                {
                    try
                    {
                        var woeList = _data.LoadData<WoeModel, dynamic>("sp_adminLookUp", new { @tblName = "woedays" }, _config.GetConnectionString("AzsunaBOT")).Result;

                        if (woeList != null && woeList.Any(x => x.Day.ToUpper() == DateFormatter.ConvertWeekDay(day).Result.ToUpper()))
                        {
                            try
                            {
                                // Correct table needs to be selected depending on which day was passed in!


                                var signUpModel = new SignUpModel()
                                {
                                    WoeDate = DateFormatter.FormatDate(day).Result,
                                    DiscordTag = context.Member.Mention,
                                    CharName = charName,
                                    Role = role,
                                    Sign = sign,
                                    Comments = comments
                                };
                                //await _data.SaveData<SignUpModel>("sp_userWoeSignUp", signUpModel, _config.GetConnectionString("AzsunaBOT"));
                                RosterHelper.RefreshRostersAsync(_data, _config);
                            }
                            catch (Exception e)
                            {
                                await AttendanceMessager.AlreadySignedMessage(context);
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                    catch (Exception e) { Console.WriteLine(e.Message); }
                }
                else if (!Enum.IsDefined(typeof(WoedaysShort), day.ToUpper()))
                    await AttendanceMessager.IncorrectDayMessage(context);
                else if (!Enum.IsDefined(typeof(RolesAll), role.ToUpper()))
                    await AttendanceMessager.IncorrectRoleMessage(context);
                else if (!Enum.IsDefined(typeof(Signs), sign.ToUpper()))
                    await AttendanceMessager.IncorrectSignMessage(context);
                else
                    await AttendanceMessager.IncorrectCredentialsMessage(context);
            }
        }
    }
}
