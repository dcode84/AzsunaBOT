using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers.Message
{
    public static class AttendanceMessager
    {
        public static async Task<DiscordMessage> IncorrectCredentialsMessage(CommandContext context)
        {
            return await context.Channel.SendMessageAsync($"Incorrect credentials. It is: **!setrole \"charname\" role sign \"comments\"**. \n" +
                                                          $"If your character name or comments have spaces, make sure to enclose them in double quotes.");
        }

        public static async Task<DiscordMessage> IncorrectRoleMessage(CommandContext context)
        {
            return await context.Channel.SendMessageAsync($"Incorrect role. if unsure, please see the list with valid roles.");
        }

        internal static async Task<DiscordMessage> IncorrectWoeModeMessage(CommandContext context)
        {
            return await context.Channel.SendMessageAsync($"Incorrect mode. if unsure, please see the list with valid modes.");
        }

        public static async Task<DiscordMessage> IncorrectSignMessage(CommandContext context)
        {
            return await context.Channel.SendMessageAsync($"Sign can only be **Yes**, **No** or **Maybe**. Please be aware that Maybe results in a No, if not changed");
        }

        public static async Task<DiscordMessage> AlreadySignedMessage(CommandContext context)
        {
            return await context.Channel.SendMessageAsync($"You have signed up already. Try a different command.");
        }
        public static async Task<DiscordMessage> IncorrectDayMessage(CommandContext context)
        {
            return await context.Channel.SendMessageAsync($"This is not a valid day. Try one of **mon, tue, wed, thu, fri, sat, sun**");
        }

        public static async Task<DiscordMessage> SignUpBlocked(CommandContext context, string day, string mode)
        {
            var cache = DateFormatter.ConvertWeekDay(day).Result;

            return await context.Channel.SendMessageAsync($"Signup is currently closed for **{mode.ToUpper()}** on **{cache}**");
        }

        public static async Task<DiscordMessage> NotFound(CommandContext context, string day)
        {
            var cache = DateFormatter.ConvertWeekDay(day).Result;

            return await context.Channel.SendMessageAsync($"No signup found for **{cache}**");
        }

    }
}
