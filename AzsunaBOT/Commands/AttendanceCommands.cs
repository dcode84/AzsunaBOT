using AzsunaBOT.Data;
using AzsunaBOT.Enums;
using AzsunaBOT.Helpers.Message;
using AzsunaBOT.Helpers.Processes;
using DataLibrary.DataAccess;
using DataLibrary.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzsunaBOT.Commands
{
    public class AttendanceCommands : BaseCommandModule
    {
        private readonly IDataAccess _data;
        private readonly IConfiguration _config;

        private List<Roles> _roles;
        private List<Signs> _signs;
        private List<AttendanceModel> _attendanceList;

        private string _woeDate = "12101992";

        public AttendanceCommands(IDataAccess data, IConfiguration config)
        {
            _data = data;
            _config = config;
            _roles = new List<Roles>();
            _signs = new List<Signs>();
            _attendanceList = new List<AttendanceModel>();
        }


        [Command("setrole")]
        public async Task Setrole(CommandContext context, string charName, string role, string sign, string comments = "")
        {
            // TODO woeDate needs to be static and always set to the new upcoming woe day.

            if (!string.IsNullOrEmpty(charName)
                && !string.IsNullOrEmpty(role)
                && !string.IsNullOrEmpty(sign))
            {
                if (Enum.IsDefined(typeof(Roles), role.ToUpper()) && Enum.IsDefined(typeof(Signs), sign.ToUpper()))
                {
                    try
                    {
                        // TODO
                        // Needs correct responses

                        var signUpModel = new SignUpModel()
                        {
                            WoeDate = _woeDate,
                            DiscordTag = context.User.Username + "#" + context.User.Discriminator,
                            CharName = charName,
                            Role = role,
                            Sign = sign,
                            Comments = comments
                        };
                        RefreshRoster(context);
                        //await _data.SaveData<SignUpModel>("sp_userWoeSignUp", signUpModel, _config.GetConnectionString("AzsunaBOT"));
                    }
                    catch (Exception e)
                    {
                        await AttendanceMessager.AlreadySignedMessage(context);
                        Console.WriteLine(e.Message);
                    }
                }
                else if (!(Enum.IsDefined(typeof(Roles), role.ToUpper())))
                    await AttendanceMessager.IncorrectRoleMessage(context);

                else if (!Enum.IsDefined(typeof(Signs), sign.ToUpper()))
                    await AttendanceMessager.IncorrectSignMessage(context);
                else
                    await AttendanceMessager.IncorrectCredentialsMessage(context);


            }
        }

        public async void RefreshRoster(CommandContext context)
        {
            try
            {
                await RoleLists.ClearLists();

                _attendanceList = _data.LoadData<AttendanceModel, dynamic>("sp_adminLookUp", new { @tblName = _woeDate }, _config.GetConnectionString("AzsunaBOT")).Result;

                var first = await RoleProcessor.SortFirstBatchAsync(_attendanceList);
                var second = await RoleProcessor.SortSecondBatchAsync(_attendanceList);
                var third = await RoleProcessor.SortThirdBatchAsync(_attendanceList);

                await RosterMessager.FirstRoleMessage(context, first);
                await RosterMessager.SecondRoleMessage(context, second);
                await RosterMessager.ThirdRoleMessage(context, third);

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }


    }
}
