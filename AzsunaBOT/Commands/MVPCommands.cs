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
        private readonly List<MVPData> _mvpDataList = new List<MVPData>();
        private readonly List<ThreadedTimer> _timerList = new List<ThreadedTimer>();
        private ThreadedTimer _timer;


        [Command("mvp")]
        [Description("Sets or resets a certain MvP timer. \n**-r *MvPName*** to reset or start. \n**-v *MvPName** to show time until variance*")]
        public async Task ResetMvp(CommandContext context, [Description("-r or -v")] string parameter, [Description("MvP")] string name, [Description("Time")] int minutes = 0)
        {
            if (parameter == "-r")
            {
                await StartTimer(context, name.ToUpper(), parameter);
                
            }
            else if (parameter == "-v")
            {
                if (_timer != null)
                {
                    _timer = _timerList.SingleOrDefault(t => t.Name.ToUpper() == name.ToUpper());
                    _timer.GetTimeUntilVariance(context);
                    
                }
                else
                    await context.Channel.SendMessageAsync($"Please start a timer before you use this parameter. Thx bruh.");
            }
            else if (parameter == "-s")
            {
                _timer = _timerList.SingleOrDefault(t => t.Name.ToUpper() == name.ToUpper());
                //_timer = _timerList.Find(t => t.Name.ToUpper() == name.ToUpper());

                if (_timer == null)
                {
                    await context.Channel.SendMessageAsync($"There is no running timer for {name.ToUpper()}");
                    return;
                }
                else
                {
                    _timerList.Remove(_timer);
                    _timer.Abort();
                    //_timer = null;
                }

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
                _mvpDataList.Add(obj);
            }
        }

        public async Task StartTimer(CommandContext context, string name, string parameter)
        {
            var requestedTimer = await GetMvpTimer(context, name);
            bool containsTimer = _timerList.Any(item => item.Name.ToUpper() == name);

            if (requestedTimer != null && containsTimer == false)
            {

                _timer = new ThreadedTimer(context,
                                           requestedTimer.Name,
                                           TimeSpan.FromMinutes((double)requestedTimer.MinTime),
                                           TimeSpan.FromMinutes((double)requestedTimer.MaxTime));
                _timer.Start();

                _timerList.Add(_timer);
            }
            else if (containsTimer)
            {
                if(parameter == "-r")
                {
                    _timerList.RemoveAll(n => n.Name.ToUpper() == name);
                    await context.Channel.SendMessageAsync($"Timer for **{name}** has been reset.");
                    await StartTimer(context, name, parameter);
                }
            }                
            else
                await context.Channel.SendMessageAsync($"Something went wrong. NULL value found.");
        }

        public async Task<MVPData> GetMvpTimer(CommandContext context, string name)
        {
            if (_mvpDataList.Count == 0)
            {
                await LoadJsonDataAsync();
            }

            var match = _mvpDataList.FirstOrDefault(n => n.Name.ToUpper() == name);

            if (match != null)
                return match;
            else
                await context.Channel.SendMessageAsync($"*{name}* does not exist in the list currently. Contact a dev for it.");

            return null;            
        }
    }
}
