using AzsunaBOT.Enums;
using AzsunaBOT.Helpers.Message;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers
{
    public static class AttendanceHelper
    {
        public static async Task<bool> IsValueOfEnum(CommandContext context, string day, string mode, string role, string sign)
        {
            return await Task.Run(async () =>
            {
                if (!Enum.IsDefined(typeof(WoedaysShort), day.ToUpper()))
                {
                    await AttendanceMessager.IncorrectDayMessage(context);
                    return false;
                }
                else if (!Enum.IsDefined(typeof(WoeMode), mode.ToUpper()))
                {
                    await AttendanceMessager.IncorrectWoeModeMessage(context);
                    return false;
                }
                else if (!Enum.IsDefined(typeof(RolesAll), role.ToUpper()))
                {
                    await AttendanceMessager.IncorrectRoleMessage(context);
                    return false;
                }
                else if (!Enum.IsDefined(typeof(Signs), sign.ToUpper()))
                {
                    await AttendanceMessager.IncorrectSignMessage(context);
                    return false;
                }
                else
                    return true;
            });
        }
    }
}
