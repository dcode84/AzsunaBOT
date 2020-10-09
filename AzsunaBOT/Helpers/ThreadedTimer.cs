using AzsunaBOT.EventArgs;
using AzsunaBOT.Helpers.Message;
using AzsunaBOT.Commands;
using DSharpPlus.CommandsNext;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace AzsunaBOT.Helpers
{
    public class ThreadedTimer : INotifyPropertyChanged
    {
        public const int EarlyReminderInMinutes = 1;

        private string _message;
        private string _name;

        private readonly TimeSpan _minVarianceTimer;
        private readonly TimeSpan _maxVarianceTimer;

        private DateTime _nextWakeUp;
        private DateTime _endOfVariance;
        private DateTime _earlyReminder;

        private DateTime? _killTime { get; set; }

        private bool _running;

        private readonly CommandContext _context;
        private readonly Thread _thread;

        public string Name { get => _name; set => _name = value; }

        public string Message
        {
            get { return _message; }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    OnMessageChanged(_message);

                    _context.Channel.SendMessageAsync(_message);
                }
            }
        }

        public bool IsRunning { get => _running; set => _running = value; }

        public ThreadedTimer(CommandContext context, string name, TimeSpan minVarianceTime, TimeSpan maxVarianceTime, DateTime? killTime)
        {
            _context = context;
            _name = name.ToUpper();
            _minVarianceTimer = minVarianceTime;
            _maxVarianceTimer = maxVarianceTime;
            _killTime = killTime;

            if (_killTime == null)
            {
                _killTime = DateTime.UtcNow;
            }

            _thread = new Thread(TimerThread)
            {
                IsBackground = true,
                Name = _name.ToUpper()
            };
        }

        public delegate void TimerEventHandler(object sender, TimerEventArgs args);
        public event TimerEventHandler TimeReached;
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual Task OnTimeReached(string message)
        {
            TimeReached?.Invoke(this, new TimerEventArgs(message));
            Message = message;

            return Task.CompletedTask;
        }

        protected virtual Task OnMessageChanged(string message)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(message));

            return Task.CompletedTask;
        }

        public Task Start()
        {
            try 
            {
                _thread.Start();
                _running = true;
            }
            catch { }
            finally { OnTimeReached($"**{_name} **: Timer has started. "); }
         
            return Task.CompletedTask;
        }

        public Task Abort()
        {
            try
            {
                
                _thread.Abort();
                _running = false;
            }
            catch { }
            finally { OnTimeReached($"**{_name} **: Timer has aborted/stopped. "); }

            return Task.CompletedTask;
        }

        public async void GetTimeUntilVariance(CommandContext context)
        {
            var cache = _nextWakeUp.Subtract(DateTime.UtcNow);

            await TimerMessager.ShowTimeUntilVarianceMessage(context, _name, cache);
        }

        private void TimerThread()
        {
            _nextWakeUp = (DateTime)_killTime + _minVarianceTimer;
            _endOfVariance = (DateTime)_killTime + _maxVarianceTimer;
            _earlyReminder = _nextWakeUp.AddMinutes(-EarlyReminderInMinutes);

            var earlyReminderTs = _earlyReminder.Subtract(DateTime.UtcNow);
            var fiveMinutesUntilVariance = _minVarianceTimer - earlyReminderTs;
            var maxVarianceReminderTs = _maxVarianceTimer - _minVarianceTimer;

            while (_running)
            {
                Thread.Sleep((int)earlyReminderTs.TotalMilliseconds);

                if (_earlyReminder < DateTime.UtcNow && _running)
                {
                    try
                    {
                        OnTimeReached($"**{_name} **: Only five minutes until variance starts.");
                    }
                    catch { }

                    Thread.Sleep((int)fiveMinutesUntilVariance.TotalMilliseconds);
                }

                if (_nextWakeUp < DateTime.UtcNow && _running)
                {
                    try
                    {
                        OnTimeReached($"**{_name} **: Variance has started.");
                    }
                    catch { }

                    Thread.Sleep((int)maxVarianceReminderTs.TotalMilliseconds);
                }

                if (_endOfVariance < DateTime.UtcNow && _running)
                {
                    try
                    {
                        _thread.Abort();
                    }
                    catch { }
                    finally { OnTimeReached($"**{_name} **: Variance has ended and the MvP has spawned for sure. Check the map again"); };
                }
                break;
            }
        }
    }
}
