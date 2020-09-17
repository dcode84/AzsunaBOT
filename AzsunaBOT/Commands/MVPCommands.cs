using AzsunaBOT.Data;
using AzsunaBOT.Helpers;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AzsunaBOT.Commands
{
    class MVPCommands : BaseCommandModule
    {
        //private readonly List<MVPData> _timers = new List<MVPData>();
        private readonly List<MVPTimer> _timers = new List<MVPTimer>();
        private readonly List<MVPData> _jsonData = new List<MVPData>();


        [Command("mvp")]
        [Description("Sets or resets a certain MvP timer. \n**-r *MvPName*** to reset at once. \n**-t *MvPName number*** in minutes passed.")]
        public async Task ResetMvp(CommandContext context, [Description("-r, -t or -s")]string parameter, [Description("MvP")]string name, [Description("Time")]int minutes = 0)
        {
            if (parameter == "-r")
            {           
                await StartTimer(name);
                await StartTimerMessage(context, name, minutes).ConfigureAwait(false);
            }
            else if (parameter == "-t")
            {
                await StartTimerMessage(context, name, minutes).ConfigureAwait(false);
            }
            else if (parameter == "-s")
            {
                await ShowTimeUntilVarianceMessage(context, name).ConfigureAwait(false);
            }
        }

        public async Task LoadJsonDataAsync()
        {
            var json = string.Empty;

            using(var fs = File.OpenRead(@"E:\programpls\Projects\AzsunaBOT\AzsunaBOT\Data\MVPData.json"))
            using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var mvpObject = JsonConvert.DeserializeObject<List<MVPTimer>>(json);

            foreach (var obj in mvpObject)
            {
                _timers.Add(obj);
            }
        }

        public async Task StartTimerMessage(CommandContext context, string name, int minutes)
        {
            if (minutes == 0)
            {
                await context.Channel.SendMessageAsync($"Starting timer for **{name.ToUpper()}**").ConfigureAwait(false);
            }
            else
            {
                await context.Channel.SendMessageAsync($"Starting timer for **{name.ToUpper()}** from {minutes} minutes ago.").ConfigureAwait(false);
            }
        }

        public async Task StartTimer(string name)
        {
            var requestedTimer = await GetMvpTimer(name.ToUpper());

            requestedTimer.Timer.Start();
        }

        public async Task<MVPTimer> GetMvpTimer(string name)
        {
            if (_timers.Count == 0)
            {
                await LoadJsonDataAsync();
            }

            var match = _timers.FirstOrDefault(n => n.Name.ToUpper() == name.ToUpper());

            if (match != null)
            {
                return match;
            }
            return null;
        }

        private async Task<int> CalculateTime(string name)
        {
            var requestedTimer = await GetMvpTimer(name.ToUpper());

            var remainingTime = RemainingTimeCalculator.CalculateRemainingMinTime(requestedTimer.MinTime, (int)requestedTimer.Timer.ElapsedMilliseconds);

            return remainingTime;
        }

        private async Task ShowTimeUntilVarianceMessage(CommandContext context, string name)
        {
            var requestedTimer = await GetMvpTimer(name.ToUpper());
            var isRunning = requestedTimer.Timer.IsRunning;
            var time = requestedTimer.Timer.ElapsedMilliseconds;
            var remainingTime = await CalculateTime(name);

            if (isRunning == false && time == 0)
            {
                await context.Channel.SendMessageAsync($"There is no running timer for **{name.ToUpper()}**");
            }
            else
            {
                await context.Channel.SendMessageAsync($"**{name.ToUpper()}** respawns in {remainingTime} minutes.");
            }
        }

    }
}
