using AzsunaBOT.EventArgs;
using DSharpPlus.CommandsNext;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace AzsunaBOT.Helpers
{
    public class ThreadedTimer : INotifyPropertyChanged
    {
        public const int EarlyReminderInMinutes = 1;

        private string _message;
        private TimeSpan _minVarianceTimer;
        private TimeSpan _maxVarianceTimer;
        private DateTime _nextWakeUp;
        private DateTime _endOfVariance;
        private DateTime _earlyReminder;
        private string _name;
        private bool _running;
        private readonly CommandContext _context;
        private readonly Thread _thread;


        public string Name { get => _name; set => _name = value; }

        //public TimeSpan MinVarianceTimer
        //{
        //    get => _minVarianceTimer;
        //    set => _minVarianceTimer = value;
        //}

        //public TimeSpan MaxVarianceTimer
        //{
        //    get => _maxVarianceTimer;
        //    set => _maxVarianceTimer = value;
        //}

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
            this._name = name.ToUpper();
            this._minVarianceTimer = minVarianceTime;
            this._maxVarianceTimer = maxVarianceTime;

            _thread = new Thread(TimerThread)
            {
                IsBackground = true,
                Name = _name.ToUpper()
            };
        }

        protected virtual void OnTimeReached(string message)
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
                OnTimeReached($"**{_name} **: Timer has started. ");
                _thread.Start();
            }
            catch { }
        }

        public void Abort()
        {
            try
            {
                OnTimeReached($"**{_name} **: Timer has aborted/stopped. ");
                _running = false;
                _thread.Interrupt();
            }
            catch { }
        }

        public async void GetTimeUntilVariance(CommandContext context)
        {
            var cache = _nextWakeUp.Subtract(DateTime.UtcNow);

            await context.Channel.SendMessageAsync($"**{_name}** : Variance starts in {cache.Hours} hours and {cache.Minutes} minutes.");

        }

        private void TimerThread()
        {
            _nextWakeUp = DateTime.UtcNow + _minVarianceTimer;
            _endOfVariance = DateTime.UtcNow + _maxVarianceTimer;
            _earlyReminder = _nextWakeUp.AddMinutes(-EarlyReminderInMinutes);

            var earlyReminderTs = _earlyReminder.Subtract(DateTime.UtcNow);
            var fiveMinutesUntilVariance = _minVarianceTimer - earlyReminderTs;
            var maxVarianceReminderTs = _maxVarianceTimer - _minVarianceTimer;

            while (_running)
            {
                Thread.Sleep((int)earlyReminderTs.TotalMilliseconds);

                if (_earlyReminder < DateTime.UtcNow)
                {
                    try
                    {
                        OnTimeReached($"**{_name} **: Only five minutes until variance starts.");
                    }
                    catch { }
                    Thread.Sleep((int)fiveMinutesUntilVariance.TotalMilliseconds);
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
    }
}
