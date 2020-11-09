using DataLibrary.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers.Message
{
    public static class RosterMessager
    {

        public static async Task<DiscordMessage> FirstRoleMessage(CommandContext context, List<AttendanceModel> list)
        {
            var builtString = await RosterMessageBuilder.BuildFirstPostAsync(list);

            return await context.Channel.SendMessageAsync($"{builtString}");

        }

        public static async Task<DiscordMessage> SecondRoleMessage(CommandContext context, List<AttendanceModel> list)
        {
            var builtString = await RosterMessageBuilder.BuildSecondPostAsync(list);

            return await context.Channel.SendMessageAsync($"{builtString}");

        }

        public static async Task<DiscordMessage> ThirdRoleMessage(CommandContext context, List<AttendanceModel> list)
        {
            var builtString = await RosterMessageBuilder.BuildThirdPostAsync(list);

            return await context.Channel.SendMessageAsync($"{builtString}");

        }
    }
}
