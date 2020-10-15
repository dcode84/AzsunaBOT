using AzsunaBOT.Data;
using AzsunaBOT.Helpers;
using AzsunaBOT.Helpers.Message;
using AzsunaBOT.Helpers.Processes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AzsunaBOT.Commands
{
    public class MVPCommands : BaseCommandModule
    {
        private List<MVPData> _mvpDataList = new List<MVPData>();
        private List<MVPTimer> _timerList = new List<MVPTimer>();

        private MVPTimer _timer;

        private readonly string[] _validParametersArray = { "-s", "-r", "-i", "-v", "-t", "-tr", "-l" };

        private bool _containsTimer = false;
        private string _name;

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
                parameter = param.Trim(charToTrim);
                _containsTimer = _timerList.Any(t => t.Name.ToUpper() == _name);
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
                    // Variance check method call here
                    break;

                case "t":
                case "tr":
                    await StartExactTimerAsync(context, _name, parameter, time);
                    break;

                case "l":
                    // Listing timer method call here
                    break;

                default:
                    await TimerMessager.SomethingWentWrongMessage(context);
                    break;
            }
        }

        private async Task StartTimerAsync(CommandContext context, string name, string parameter, DateTime? killTime = null)
        {
            var requestedTimer = await GetMvpTimerValuesAsync(context, name);

            if (requestedTimer != null && _containsTimer == false)
            {
                // start a System.Timer here
                _timer = new MVPTimer(context, name,
                                      TimeSpan.FromMinutes((double)requestedTimer.MinTime),
                                      TimeSpan.FromMinutes((double)requestedTimer.MaxTime),
                                      killTime);
                _timerList.Add(_timer);
                await _timer.Start();

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

        private Task InterruptTimerAsync(CommandContext context, string name, string parameter)
        {
            // Timer interruption logic here
            GetTimerObject(name);
            _timer.Stop();
            _timerList.Remove(_timer);

            //await TimerMessager.TimerNotRunningMessage(context, name);
            return Task.CompletedTask;
        }

        private async Task ResetTimerAsync(CommandContext context, string name, string parameter)
        {
            // Timer reset logic here
            await InterruptTimerAsync(context, name, parameter);

            await TimerMessager.ResetTimerMessage(context, name);

            await StartTimerAsync(context, name, parameter);

            //await TimerMessager.TimerNotRunningMessage(context, name);
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

        private MVPTimer GetTimerObject(string name) => _timer = _timerList.SingleOrDefault(t => t.Name.ToUpper() == name);
        //{


        //    return Task.CompletedTask;
        //}
    }
}
