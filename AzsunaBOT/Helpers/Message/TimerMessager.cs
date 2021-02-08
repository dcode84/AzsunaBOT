
using AzsunaBOT.Data;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers.Message
{
    public static class TimerMessager
    {

        public static async Task<DiscordMessage> TimerNotFoundMessage(CommandContext context, string name)
        {
            return await context.Channel.SendMessageAsync($"**{name}** : Currently there is no known reference to this MvP. " +
                                                          $"Contact a dev for the timer implementation.");
        }

        public static async Task<DiscordMessage> TimerNotRunningMessage(CommandContext context, string name)
        {
            return await context.Channel.SendMessageAsync($"**{name}** : Currently there is no running timer for this MvP.");
        }

        public static async Task<DiscordMessage> SomethingWentWrongMessage(CommandContext context)
        {
            return await context.Channel.SendMessageAsync($"Something went wrong. Contact a dev pls");
        }

        public static async Task<DiscordMessage> ShowTimeUntilVarianceMessage(CommandContext context, string name, TimeSpan cache)
        {
            return await context.Channel.SendMessageAsync($"**{name}** : Variance starts in {cache.Hours} hours and {cache.Minutes} minutes.");
        }

        public static async Task<DiscordMessage> TimerIsRunningMessage(CommandContext context, string name)
        {
            return await context.Channel.SendMessageAsync($"**{name}** : Timer is currently running. Try another parameter.");
        }

        public static async Task<DiscordMessage> UnknownCredentialsMessage(CommandContext context)
        {
            return await context.Channel.SendMessageAsync($"Idk what you want me to do bruh. A valid parameter and mvp must be passed to this command.");

        }
        public static async Task<DiscordMessage> ResetTimerMessage(CommandContext context, string name)
        {
            return await context.Channel.SendMessageAsync($"**{name}** : Resetting timer.");

        }

        public static async Task<DiscordMessage> ExactTimerMessage(CommandContext context, string name, DateTime parsedDate)
        {
            return await context.Channel.SendMessageAsync($"**{name}** : Timer starts from {parsedDate.Hour:00.##}:" +
                                                          $"{parsedDate.Minute:00.##}:" +
                                                          $"{parsedDate.Second:00.##}");
        }

        public static async Task DisplayRunningTimersMessage(CommandContext context)
        {
            foreach (var mvp in MVPTimerList._list)
            {
                await context.Channel.SendMessageAsync($"**{mvp.Name}** : \n" +
                                                       $"      **Killed** : {mvp.KillTime.Value:HH:mm:ss} \n" +
                                                       $"      **Variance** : {mvp.VarianceStart:HH:mm:ss} \n" +
                                                       $"      **InVariance** : {mvp.IsInVariance}");
            }
        }

        public static async Task<DiscordMessage> DisplayVarianceInfo(CommandContext context, MVPTimer timer)
        {
            if (timer.IsInVariance == false)
            {
                return await context.Channel.SendMessageAsync($"**{timer.Name}** : \n" +
                                                              $"       **Variance time** : {timer.VarianceStart:HH:mm:ss} \n" +
                                                              $"       **Time left** : {timer.TimeUntilVariance.Hours}:{timer.TimeUntilVariance.Minutes}:{timer.TimeUntilVariance.Seconds}");
            }
            else
            {
                return await context.Channel.SendMessageAsync($"**{timer.Name}** : MVP is in variance already!");
            }


        }
    }
}
