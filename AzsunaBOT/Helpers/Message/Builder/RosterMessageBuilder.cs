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

            await BuildRecaller(list, builder);
            await BuildFullSupportProf(list, builder);
            await BuildDLP(list, builder);
            await BuildDamageBiochemist(list, builder);
            await BuildSupportBiochemist(list, builder);
            await BuildFullSupportClown(list, builder);
            await BuildDevo(list, builder);
            await BuildScreamer(list, builder);
            await BuildSupportGypsy(list, builder);

            return builder;
        }

        public static async Task<StringBuilder> BuildSecondPostAsync(List<AttendanceModel> list)
        {
            var builder = new StringBuilder();

            await BuildHighPriest(list, builder);
            await BuildLinker(list, builder);
            await BuildChamp(list, builder);
            await BuildSlowGrace(list, builder);
            await BuildTarotGypsy(list, builder);
            await BuildTarotClown(list, builder);
            await BuildFrostJokeClown(list, builder);
            await BuildDamageHighWizard(list, builder);

            return builder;
        }


        public static async Task<StringBuilder> BuildThirdPostAsync(List<AttendanceModel> list)
        {
            var builder = new StringBuilder();

            await BuildFullSupportHighWizard(list, builder);
            await BuildDamageStalker(list, builder);
            await BuildDamageSniper(list, builder);
            await BuildFalconSniper(list, builder);
            await BuildStatusSniper(list, builder);
            await BuildMBKRecaller(list, builder);
            await BuildMBKProf(list, builder);
            await BuildMBKDevo(list, builder);
            await BuildSmith(list, builder);
            await BuildWarper(list, builder);
            await BuildReroll(list, builder);
            await BuildGuest(list, builder);

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

        #region BuildFirstBatch
        private static async Task BuildSupportGypsy(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "SPSONG"))
            {
                builder.AppendLine("**SPSONG**");
                foreach (var role in list.Where(x => x.Role == "SPSONG"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }

        }

        private static async Task BuildScreamer(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "SCREAM"))
            {
                builder.AppendLine("**SCREAM**");
                foreach (var role in list.Where(x => x.Role == "SCREAM"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildDevo(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "DEVO"))
            {
                builder.AppendLine("**DEVO**");
                foreach (var role in list.Where(x => x.Role == "DEVO"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - { role.CharName} : { role.Comments}");
            }
        }

        private static async Task BuildFullSupportClown(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "FSCLOWN"))
            {
                builder.AppendLine("**FSCLOWN**");
                foreach (var role in list.Where(x => x.Role == "FSCLOWN"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildSupportBiochemist(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "SPPCHEM"))
            {
                builder.AppendLine("**SPPCHEM**");
                foreach (var role in list.Where(x => x.Role == "SPPCHEM"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildDamageBiochemist(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "DDCHEM"))
            {
                builder.AppendLine("**DDCHEM**");
                foreach (var role in list.Where(x => x.Role == "DDCHEM"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildDLP(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "DLP"))
            {
                builder.AppendLine("**DLP**");
                foreach (var role in list.Where(x => x.Role == "DLP"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildFullSupportProf(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "FSPROF"))
            {
                builder.AppendLine("**FSPROF**");
                foreach (var role in list.Where(x => x.Role == "FSPROF"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildRecaller(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "RECALLER"))
            {
                builder.AppendLine("**RECALLER**");
                foreach (var role in list.Where(x => x.Role == "RECALLER"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }
        #endregion

        #region BuildSecondBatch
        private static async Task BuildDamageHighWizard(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "DDHW"))
            {
                builder.AppendLine("**DDHW**");
                foreach (var role in list.Where(x => x.Role == "DDHW"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }

        }

        private static async Task BuildFrostJokeClown(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "FJCLOWN"))
            {
                builder.AppendLine("**FJCLOWN**");
                foreach (var role in list.Where(x => x.Role == "FJCLOWN"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }

        }

        private static async Task BuildTarotClown(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "TAROTCLOWN"))
            {
                builder.AppendLine("**TAROTCLOWN**");
                foreach (var role in list.Where(x => x.Role == "TAROTCLOWN"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }

        }

        private static async Task BuildTarotGypsy(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "TAROTGYPSY"))
            {
                builder.AppendLine("**TAROTGYPSY**");
                foreach (var role in list.Where(x => x.Role == "TAROTGYPSY"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }

        }

        private static async Task BuildSlowGrace(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "SGGYPSY"))
            {
                builder.AppendLine("**SGGYPSY**");
                foreach (var role in list.Where(x => x.Role == "SGGYPSY"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }

        }

        private static async Task BuildChamp(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "CHAMP"))
            {
                builder.AppendLine("**CHAMP**");
                foreach (var role in list.Where(x => x.Role == "CHAMP"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildLinker(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "LINKER"))
            {
                builder.AppendLine("**LINKER**");
                foreach (var role in list.Where(x => x.Role == "LINKER"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildHighPriest(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "HP"))
            {
                builder.AppendLine("**HP**");
                foreach (var role in list.Where(x => x.Role == "HP"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }
        #endregion

        #region BuildThirdBatch
        private static async Task BuildGuest(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "GUEST"))
            {
                builder.AppendLine("**GUEST**");
                foreach (var role in list.Where(x => x.Role == "GUEST"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildReroll(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "REROLL"))
            {
                builder.AppendLine("**REROLL**");
                foreach (var role in list.Where(x => x.Role == "REROLL"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildWarper(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "WARPER"))
            {
                builder.AppendLine("**WARPER**");
                foreach (var role in list.Where(x => x.Role == "WARPER"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildSmith(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "FSSMITH"))
            {
                builder.AppendLine("**FSSMITH**");
                foreach (var role in list.Where(x => x.Role == "FSSMITH"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildMBKDevo(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "SMITH"))
            {
                builder.AppendLine("**SMITH**");
                foreach (var role in list.Where(x => x.Role == "SMITH"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildMBKProf(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "MBKDEVO"))
            {
                builder.AppendLine("**MBKDEVO**");
                foreach (var role in list.Where(x => x.Role == "MBKDEVO"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildMBKRecaller(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "MBKPROF"))
            {
                builder.AppendLine("**MBKPROF**");
                foreach (var role in list.Where(x => x.Role == "MBKPROF"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildStatusSniper(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "MBKRECALLER"))
            {
                builder.AppendLine("**MBKRECALLER**");
                foreach (var role in list.Where(x => x.Role == "MBKRECALLER"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildFalconSniper(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "STATUSSNIPER"))
            {
                builder.AppendLine("**STATUSSNIPER**");
                foreach (var role in list.Where(x => x.Role == "STATUSSNIPER"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildDamageSniper(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "DDSNIPER"))
            {
                builder.AppendLine("**DDSNIPER**");
                foreach (var role in list.Where(x => x.Role == "DDSNIPER"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildDamageStalker(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "DDSTALKER"))
            {
                builder.AppendLine("**DDSTALKER**");
                foreach (var role in list.Where(x => x.Role == "DDSTALKER"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }

        private static async Task BuildFullSupportHighWizard(List<AttendanceModel> list, StringBuilder builder)
        {
            if (list.Any(x => x.Role.ToUpper() == "FSHW"))
            {
                builder.AppendLine("**FSHW**");
                foreach (var role in list.Where(x => x.Role == "FSHW"))
                    builder.AppendLine($"{await GetSignColorAsync(role)} @{role.DiscordTag} - {role.CharName} : {role.Comments}");
            }
        }
        #endregion
    }
}
