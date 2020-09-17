using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace AzsunaBOT.Commands
{
    public class UWUCommands : BaseCommandModule
    {

        [Command("uwu")]
        [Description("UwU's the tagged person")]
        public async Task UwUTag(CommandContext context, [Description("Person to be tagged.")] string name)
        {
            var uwu = DiscordEmoji.FromName(context.Client, ":uwu:");
            await context.Channel.SendMessageAsync(/*{context.User.Mention}*/$"{uwu} {name}").ConfigureAwait(false);
        }
    }
}
