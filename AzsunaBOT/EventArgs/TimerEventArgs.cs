namespace AzsunaBOT.EventArgs
{
    public class TimerEventArgs
    {
        public string Message { get; }

        public TimerEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
