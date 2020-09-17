using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers.Messages
{
    public class MessageListener
    {

        public static async Task OnMessageSent(MessageCreateEventArgs e)
        {
            if (string.Equals(e.Message.Content, "uwu"))
            {
                var uwu = DiscordEmoji.FromName(e.Client, ":uwu:");
                await e.Message.RespondAsync(uwu);
            }

            if (string.Equals(e.Message.Content, "shroom"))
            {
                var shroom = DiscordEmoji.FromName(e.Client, ":shroom:");
                await e.Message.RespondAsync(shroom);
            }
        }
    }
}
