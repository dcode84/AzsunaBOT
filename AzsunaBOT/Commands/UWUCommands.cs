using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace AzsunaBOT.Commands
{
    public class UWUCommands : BaseCommandModule
    {

        public static async Task OnUwUSend(MessageCreateEventArgs e)
        {
            if (string.Equals(e.Message.Content, "uwu"))
            {
                await e.Message.RespondAsync("UwU");
            }
        }

        [Command("uwu")]
        [Description("UwU's the tagged person")]
        public async Task UwUTag(CommandContext context, [Description("Person to be tagged.")]string name)
        {
            await context.Channel.SendMessageAsync($"{context.User.Mention} uwu {name}").ConfigureAwait(false);
        }
    }
}
