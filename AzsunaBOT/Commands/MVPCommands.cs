using AzsunaBOT.Data;
using AzsunaBOT.EventArgs;
using AzsunaBOT.Helpers;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzsunaBOT.Commands
{
    public class MVPCommands : BaseCommandModule
    {
        private readonly List<MVPData> _timers = new List<MVPData>();
        private ThreadedTimer _timer;

        public void OnTimerReaded(CommandContext context, object sender, TimerEventArgs e)
        {

        }


        [Command("mvp")]
        [Description("Sets or resets a certain MvP timer. \n**-r *MvPName*** to reset at once. \n**-t *MvPName number*** in minutes passed.")]
        public async Task ResetMvp(CommandContext context, [Description("-r, -t or -v")] string parameter, [Description("MvP")] string name, [Description("Time")] int minutes = 0)
        {
            if (parameter == "-r")
            {
                await StartTimer(context, name);
                //await StartTimerMessage(context, name, minutes).ConfigureAwait(false);
            }
            else if (parameter == "-t")
            {
                //await StartTimerMessage(context, name, minutes).ConfigureAwait(false);
            }
            else if (parameter == "-v")
            {
            }
        }

        public async Task LoadJsonDataAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead(@"C:\Repos\AzsunaBOT\AzsunaBOT\Data\MVPData.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var mvpObject = JsonConvert.DeserializeObject<List<MVPData>>(json);

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

        public async Task StartTimer(CommandContext context, string name)
        {
            var requestedTimer = await GetMvpTimer(name.ToUpper());

            _timer = new ThreadedTimer(context,
                                       requestedTimer.Name,
                                       TimeSpan.FromMinutes((double)requestedTimer.MinTime),
                                       TimeSpan.FromMinutes((double)requestedTimer.MaxTime));
            _timer.Start();


        }

        public async Task<MVPData> GetMvpTimer(string name)
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
            else
            {
                return null;
            }
        }

    }
}
