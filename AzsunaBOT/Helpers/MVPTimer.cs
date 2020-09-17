using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AzsunaBOT.Helpers
{
    public class MVPTimer
    {
        public string Name { get; set; }
        public int MinTime { get; set; }
        public int MaxTime { get; set; }
        public Stopwatch Timer { get; set; } = new Stopwatch();
    }
}
