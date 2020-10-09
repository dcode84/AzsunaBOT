using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers.Message
{
    public class MessageListener
    {
        public static async Task OnMessageSent(DiscordClient client, MessageCreateEventArgs e)
        {
            if (string.Equals(e.Message.Content, "uwu"))
            {
                var uwu = DiscordEmoji.FromName(client, ":uwu:");
                await e.Message.RespondAsync(uwu);
            }

            if (string.Equals(e.Message.Content, "shroom"))
            {
                var shroom = DiscordEmoji.FromName(client, ":shroom:");
                await e.Message.RespondAsync(shroom);
            }

            if (string.Equals(e.Message.Content, "jojo"))
            {
                var jojo = DiscordEmoji.FromName(client, ":face_vomiting:");
                await e.Message.RespondAsync(jojo);
            }
        }
    }
}
