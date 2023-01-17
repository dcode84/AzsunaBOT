using DataLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers.Message
{
    public static class RosterMessageBuilder
    {
        public static async Task<StringBuilder> BuildFirstPostAsync(List<AttendanceModel> list)
        {
            var builder = new StringBuilder();

            await BuildRosterStrings(list, builder);

            return builder;
        }

        public static async Task<StringBuilder> BuildSecondPostAsync(List<AttendanceModel> list)
        {
            var builder = new StringBuilder();

            await BuildRosterStrings(list, builder);

            return builder;
        }

        public static async Task<StringBuilder> BuildThirdPostAsync(List<AttendanceModel> list)
        {
            var builder = new StringBuilder();

            await BuildRosterStrings(list, builder);

            return builder;
        }

        private static async Task<string> GetSignColorAsync(AttendanceModel entry)
        {
            string cache = "";

            await Task.Run(() =>
             {
                 if (entry.Sign.ToUpper() == "YES")
                     cache = ":green_circle:";

                 if (entry.Sign.ToUpper() == "NO")
                     cache = ":red_circle:";

                 if (entry.Sign.ToUpper() == "MAYBE")
                     cache = ":orange_circle:";
             });

            return cache;
        }

        private static async Task BuildRosterStrings(List<AttendanceModel> list, StringBuilder builder)
        {
            string role = list.First().Role.ToUpper();

            builder.AppendLine($"**{role}**");

            foreach (var entry in list)
            {
                if (entry.Role.ToUpper() != role)
                {
                    builder.AppendLine($"**{entry.Role.ToUpper()}**");
                    role = entry.Role.ToUpper();
                }

                builder.AppendLine($"{await GetSignColorAsync(entry)} {entry.DiscordTag} - {entry.CharName}     {entry.Comments}");
            }
        }
    }
}
