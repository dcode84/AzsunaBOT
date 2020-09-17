using AzsunaBOT.EventArgs;
using DSharpPlus.CommandsNext;
using System;
using System.ComponentModel;
using System.Threading;

namespace AzsunaBOT.Helpers
{
    public class ThreadedTimer : INotifyPropertyChanged
    {
        private string _message;

        private readonly CommandContext _context;
        private string _name;
        private TimeSpan _minVarianceTimer;
        private TimeSpan _maxVarianceTimer;
        private TimeSpan _currentTime;
        private DateTime _nextWakeUp;
        private DateTime _endOfVariance;
        private DateTime _timerStart;
        private DateTime _earlyReminder;
        private Thread _thread;


        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public TimeSpan MinVarianceTimer
        {
            get => _minVarianceTimer;
            set => _minVarianceTimer = value;
        }

        public TimeSpan MaxVarianceTimer
        {
            get => _maxVarianceTimer;
            set => _maxVarianceTimer = value;
        }

        public TimeSpan CurrentTimer
        {
            get => _currentTime;
            set
            {
                var cache = _timerStart.Subtract(DateTime.UtcNow);
                _currentTime = _minVarianceTimer - cache;
            }
        }
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

        public delegate void TimerEventHandler(object sender, TimerEventArgs args);
        public event TimerEventHandler TimeReached;
        public event PropertyChangedEventHandler PropertyChanged;

        public ThreadedTimer(CommandContext context, string name, TimeSpan minVarianceTime, TimeSpan maxVarianceTime)
        {
            this._context = context;
            this._name = name;
            this._minVarianceTimer = minVarianceTime;
            this._maxVarianceTimer = maxVarianceTime;

            _thread = new Thread(TimerThread)
            {
                IsBackground = true,
                Name = _name.ToUpper()
            };
        }

        public void OnTimeReached(string message)
        {
            TimeReached?.Invoke(this, new TimerEventArgs(message));
            Message = message;
        }

        protected virtual void OnMessageChanged(string message)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(message));
        }

        public void Start()
        {
            try
            {
                OnTimeReached($"**{_name.ToUpper()} **: Timer has started. ");
                _thread.Start();
            }
            catch { }
        }

        public void Abort()
        {
            try
            {
                _thread.Interrupt();
                OnTimeReached($"**{_name.ToUpper()} **: Timer has aborted/stopped. ");
            }
            catch { }
        }

        public void GetCurrentTime(string name)
        {
            if (name == _name)
            {
                OnTimeReached($"**{_name.ToUpper()} **: Variance starts in {_currentTime.Hours} hours and {_currentTime.Minutes} minutes");
            }
        }

        private void TimerThread()
        {
            _timerStart = DateTime.UtcNow;
            _nextWakeUp = DateTime.UtcNow + _minVarianceTimer;
            _endOfVariance = DateTime.UtcNow + _maxVarianceTimer;
            _earlyReminder = _nextWakeUp.AddMinutes(-1);

            var earlyReminderTs = _earlyReminder.Subtract(DateTime.UtcNow);
            var fiveMinuteReminderTs = _minVarianceTimer - earlyReminderTs;
            var maxVarianceReminderTs = _maxVarianceTimer - _minVarianceTimer;

            while (true)
            {
                Thread.Sleep((int)earlyReminderTs.TotalMilliseconds);

                if (_earlyReminder < DateTime.UtcNow)
                {
                    try
                    {
                        OnTimeReached($"**{_name} **: Only five minutes until variance starts.");
                    }
                    catch { }
                    Thread.Sleep((int)fiveMinuteReminderTs.TotalMilliseconds);
                }
                if (_nextWakeUp < DateTime.UtcNow)
                {
                    try
                    {
                        OnTimeReached($"**{_name} **: Variance has started.");
                    }
                    catch { }
                    Thread.Sleep((int)maxVarianceReminderTs.TotalMilliseconds);

                }
                if (_endOfVariance < DateTime.UtcNow)
                {
                    try
                    {
                        OnTimeReached($"**{_name} **: Variance has ended and the MvP has spawned for sure. Check the map again");
                        _thread.Interrupt();

                    }
                    catch { }
                }
                break;
            }
        }

        //private async Task EarlyReminderMessage(CommandContext context)
        //{
        //    await context.Channel.SendMessageAsync($"**{_name} **: Only five minutes until variance starts.");

        //    OnTimeReached($"**{_name} **: Only five minutes until variance starts.");
        //}


        //private async Task StartMessage(CommandContext context = null)
        //{
        //    await context.Channel.SendMessageAsync($"**{_name.ToUpper()} **: Timer has started. ");

        //    OnTimeReached($"**{_name.ToUpper()} **: Timer has started. ");
        //}
    }
}
