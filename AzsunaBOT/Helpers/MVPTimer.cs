
using AzsunaBOT.EventArgs;
using DSharpPlus.CommandsNext;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;

namespace AzsunaBOT.Helpers
{
    public class MVPTimer : INotifyPropertyChanged
    {
        /// <summary>
        /// Timer needs to be removed from a static list when it finished itself
        /// </summary>


        public const int EarlyReminderInMinutes = 1;

        private static Timer _mvpTimer;
        private CommandContext _context;

        private string _name;
        private string _message;

        private readonly TimeSpan _minVarianceTime;
        private readonly TimeSpan _maxVarianceTime;

        private bool _earlyReminderFlag = false;
        private bool _varianceFlag = false;
        private bool _maxTimeFlag = false;

        private DateTime? _killTime;
        private DateTime _nextWakeUp;
        private DateTime _endOfVariance;
        private DateTime _earlyReminder;

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
        public string Name { get => _name; set => _name = value; }
        public bool IsRunning { get; private set; }

        public delegate void TimerEventHandler(object sender, TimerEventArgs args);
        public event TimerEventHandler TimeReached;
        public event PropertyChangedEventHandler PropertyChanged;

        public MVPTimer(CommandContext context, string name, TimeSpan minVarianceTime, TimeSpan maxVarianceTime, DateTime? killTime)
        {
            _context = context;
            _name = name;
            _minVarianceTime = minVarianceTime;
            _maxVarianceTime = maxVarianceTime;
            _killTime = killTime;

            if (_killTime == null)
            {
                _killTime = DateTime.UtcNow;
            }

            _nextWakeUp = (DateTime)_killTime + _minVarianceTime;
            _endOfVariance = (DateTime)_killTime + _maxVarianceTime;
            _earlyReminder = _nextWakeUp.AddMinutes(-EarlyReminderInMinutes);
        }

        public Task Start()
        {
            OnTimeReached($"**{_name}** : Timer has started.");

            _mvpTimer = new Timer(1000);
            _mvpTimer.Elapsed += OnTimedEvent;
            _mvpTimer.AutoReset = true;
            _mvpTimer.Enabled = true;

            IsRunning = true;

            return Task.CompletedTask;
        }

        public Task Stop()
        {
            OnTimeReached($"**{_name}** : Timer has stopped.");

            IsRunning = false;
            _mvpTimer.Stop();
            _mvpTimer.Dispose();

            return Task.CompletedTask;
        }

        protected virtual Task OnMessageChanged(string message)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(message));

            return Task.CompletedTask;
        }

        protected virtual Task OnTimeReached(string message)
        {
            TimeReached?.Invoke(this, new TimerEventArgs(message));
            Message = message;

            return Task.CompletedTask;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (_earlyReminder < DateTime.UtcNow && _earlyReminderFlag == false)
            {
                OnTimeReached($"**{_name} **: Only five minutes until variance starts.");
                _earlyReminderFlag = true;
            }
            if (_nextWakeUp < DateTime.UtcNow && _varianceFlag == false)
            {
                OnTimeReached($"**{_name} **: Variance has started.");
                _varianceFlag = true;
            }
            if (_endOfVariance < DateTime.UtcNow && _maxTimeFlag == false)
            {
                OnTimeReached($"**{_name} **: Variance has ended and the MvP has spawned for sure. Check the map again");
                _maxTimeFlag = true;
            }
        }
    }
}
