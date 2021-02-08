using AzsunaBOT.Data;
using AzsunaBOT.Helpers.Processes;
using DataLibrary.Models;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers.Message
{
    public static class RosterMessager
    {
        private static StringBuilder builtString;

        public static async Task<string> FirstRoleMessage(List<AttendanceModel> list)
        {
            builtString = await RosterMessageBuilder.BuildFirstPostAsync(list);

            return builtString.ToString();
        }

        public static async Task<string> SecondRoleMessage(List<AttendanceModel> list)
        {
            builtString = await RosterMessageBuilder.BuildSecondPostAsync(list);

            return builtString.ToString();
        }

        public static async Task<string> ThirdRoleMessage(List<AttendanceModel> list)
        {
            builtString = await RosterMessageBuilder.BuildThirdPostAsync(list);

            return builtString.ToString();
        }

        public static async Task BuildRosterMessage(DiscordClient client, List<AttendanceModel> list, string day)
        {
            await RoleProcessor.SortAllAsync(list);

            var channelId = await ChannelHelper.GetChannelId(day);

            await client.SendMessageAsync(client.GetChannelAsync(channelId).Result, FirstRoleMessage(RoleLists.FirstBatch).Result);
            await client.SendMessageAsync(client.GetChannelAsync(channelId).Result, SecondRoleMessage(RoleLists.SecondBatch).Result);
            await client.SendMessageAsync(client.GetChannelAsync(channelId).Result, ThirdRoleMessage(RoleLists.ThirdBatch).Result);
        }

        public static async Task<DiscordMessage> SetWoeMessage(CommandContext context, string day, DateTime? date = null)
        {
            string woeDay = await DateFormatter.ConvertWeekDay(day);

            if (date == null)
            {
                var nextWeekDay = DateFormatter.GetNextWeekday(DateTime.UtcNow, DateFormatter.GetDayOfWeekAsync(woeDay).Result).AddDays(-1);
                return await context.Channel.SendMessageAsync($"Attendance is now open for **{woeDay} {nextWeekDay:MM/dd/yyyy}**");
            }
            else
            {
                return await context.Channel.SendMessageAsync($"Attendance is now open for **{woeDay},** " + string.Format($"**{date:MM/dd/yyyy}**"));
            }
        }

        public static async Task<DiscordMessage> IncorrectCredentialsMessage(CommandContext context)
        {
            return await context.Channel.SendMessageAsync($"Incorrect credentials, try again");
        }

        public static async Task<DiscordMessage> WoeClosedMessage(CommandContext context, string day)
        {

            return await context.Channel.SendMessageAsync($"Signup has been closed for **{DateFormatter.GetDayOfWeekAsync(day).Result}**");
        }
    }
}
