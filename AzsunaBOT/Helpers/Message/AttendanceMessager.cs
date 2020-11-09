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
            return await context.Channel.SendMessageAsync($"Incorrect role. if unsure, please see the the list with valid roles.");
        }

        public static async Task<DiscordMessage> IncorrectSignMessage(CommandContext context)
        {
            return await context.Channel.SendMessageAsync($"Sign can only be **Yes**, **No** or **Maybe**. Please be aware that Maybe results in a No, if not changed");
        }

        public static async Task<DiscordMessage> AlreadySignedMessage(CommandContext context)
        {
            return await context.Channel.SendMessageAsync($"You have signed up already. Try a different command.");
        }
    }
}
