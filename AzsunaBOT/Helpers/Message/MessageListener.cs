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

            if (string.Equals(e.Message.Content, "kevin") || string.Equals(e.Message.Content, "kevione"))
            {
                await e.Message.RespondAsync("Kevione isn kuhler tüp");
            }

            if (string.Equals(e.Message.Content, "fra"))
            {
                var sunflower = DiscordEmoji.FromName(client, ":sunflower:");
                await e.Message.RespondAsync($"Spam {sunflower} this {sunflower} flower {sunflower} to give {sunflower} <@475644926349148172> {sunflower} power {sunflower}");
            }

            if (string.Equals(e.Message.Content, "asca"))
            {
                await e.Message.RespondAsync("Barricade test loading....");
            }

            if (string.Equals(e.Message.Content, "cc"))
            {
                await e.Message.RespondAsync("Nobody: \nskyleo: Time to to setup my radar trap in urban umbala :-)");
            }

            if (string.Equals(e.Message.Content, "kanet"))
            {
                await e.Message.RespondAsync("Bruh I just heard that blunt light up");
            }
        }
    }
}
