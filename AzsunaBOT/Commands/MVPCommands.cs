using AzsunaBOT.Data;
using AzsunaBOT.Helpers;
using AzsunaBOT.Helpers.Message;
using AzsunaBOT.Helpers.Processes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzsunaBOT.Commands
{
    public class MVPCommands : BaseCommandModule
    {
        private List<MVPData> _mvpDataList = new List<MVPData>();
        private readonly List<ThreadedTimer> _threadList = new List<ThreadedTimer>();
        private readonly List<ThreadedTimer> _deadList = new List<ThreadedTimer>();
        
        private readonly string[] _validParametersArray = { "-s", "-r", "-i", "-v", "-t", "-tr", "-l" };
        
        private bool _containsTimer;
        private string _name;
        private ThreadedTimer _timer;
        
        public MVPCommands()
        {
            try { _mvpDataList = JsonDataProcessor.DeserializeMvpDataAsync("MVPData.json").Result; }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [Command("mvp")]
        [Description("soon")]
        public async Task Mvp(CommandContext context, [Description("Parameter")] string param = null, [Description("MvP")] string mvpname = null, [Description("Time")] string time = null)
        {
            var parameter = string.Empty;

            if (_validParametersArray.Contains(param) 
                && _mvpDataList.Any(mvp => mvp.Name.ToUpper() == mvpname.ToUpper()))
            {
                char[] charToTrim = { '-' };
                _name = mvpname.ToUpper();
                _timer = _threadList.SingleOrDefault(t => t.Name.ToUpper() == _name);
                _containsTimer = _threadList.Any(item => item.Name.ToUpper() == _name);
                parameter = param.Trim(charToTrim);
            }
            else
            {
                await TimerMessager.UnknownCredentialsMessage(context);
                return;
            }

            switch (parameter)
            {
                case "s":
                    await StartTimerAsync(context, _name, parameter);
                    break;

                case "r":
                    await ResetTimerAsync(context, _name, parameter);
                    break;

                case "i":
                    await InterruptTimerAsync(context, _name, parameter);
                    break;

                case "v":
                    try { _timer.GetTimeUntilVariance(context); }
                    catch (Exception e)
                    {
                        await TimerMessager.SomethingWentWrongMessage(context);
                        Console.WriteLine(e.Message);
                    }
                    break;

                case "t":
                case "tr":
                    await StartExactTimerAsync(context, _name, parameter, time);
                    break;

                case "l":

                    break;

                default:
                    await TimerMessager.SomethingWentWrongMessage(context);
                    break;
            }
        }

        //public Task AddThreadToList(string name)
        //{
        //    _containsTimer = _threadList.Any(item => item.Name.ToUpper() == _name);

        //    if (_containsTimer == false)
        //        _threadList.Add(_timer);

        //    return Task.CompletedTask;
        //}        
        
        //public Task RemoveThreadFromList(string name)
        //{
        //    _timer = _threadList.SingleOrDefault(t => t.Name.ToUpper() == _name);
        //    _containsTimer = _threadList.Any(item => item.Name.ToUpper() == _name);

        //    if (_containsTimer == true)
        //        _threadList.Remove(_timer);

        //    return Task.CompletedTask;
        //}

        private async Task StartTimerAsync(CommandContext context, string name, string parameter, DateTime? killTime = null)
        {
            var requestedTimer = await GetMvpTimerValuesAsync(context, name);

            if (requestedTimer != null && _containsTimer == false)
            {

                _timer = new ThreadedTimer(context,
                                           requestedTimer.Name,
                                           TimeSpan.FromMinutes((double)requestedTimer.MinTime),
                                           TimeSpan.FromMinutes((double)requestedTimer.MaxTime),
                                           killTime);
                await _timer.Start();
                _timer.IsRunning = true;
                _threadList.Add(_timer);

                return;
            }
            else if (_timer.IsRunning)
            {
                await TimerMessager.TimerIsRunningMessage(context, name);
                return;
            }

            await TimerMessager.SomethingWentWrongMessage(context);
        }

        private async Task StartExactTimerAsync(CommandContext context, string name, string parameter, string time)
        {
            DateTime parsedDate;
            string pattern = "HH:mm:ss";

            DateTime.TryParseExact(time, pattern, null, DateTimeStyles.None, out parsedDate);

            if (_containsTimer == false && parameter == "t")
            {
                await TimerMessager.ExactTimerMessage(context, name, parsedDate);
                await StartTimerAsync(context, _name, parameter, parsedDate);
            }
            else if (parameter == "tr")
            {
                if (_containsTimer)
                {
                    await InterruptTimerAsync(context, name, parameter);
                    await StartTimerAsync(context, _name, parameter, parsedDate);

                    return;
                }
                await TimerMessager.TimerNotRunningMessage(context, name);
            }
        }

        private async Task InterruptTimerAsync(CommandContext context, string name, string parameter)
        {
            if (_timer != null)
            {
                await _timer.Abort();
                _timer.IsRunning = false;
                _deadList.Add(_timer);
                _threadList.Remove(_timer);
            }
            else
                await TimerMessager.TimerNotRunningMessage(context, name);
        }

        private async Task ResetTimerAsync(CommandContext context, string name, string parameter)
        {
            if (_containsTimer)
            {
                await InterruptTimerAsync(context, name, parameter);
                await StartTimerAsync(context, name, parameter);

                return;
            }

            await TimerMessager.TimerNotRunningMessage(context, name);
        }

        private async Task<MVPData> GetMvpTimerValuesAsync(CommandContext context, string name)
        {
            var match = _mvpDataList.FirstOrDefault(n => n.Name.ToUpper() == name);

            if (match != null)
                return match;
            else
                await TimerMessager.TimerNotFoundMessage(context, name);

            return null;
        }

        //private async Task<Task> ListTimersAsync(CommandContext context)
        //{
        //    return Task.CompletedTask;
        //}
    }
}
