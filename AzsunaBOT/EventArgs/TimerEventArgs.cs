using System;
using System.Collections.Generic;
using System.Text;

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
