using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfTesting
{
    class CountedStopwatch
    {
        private readonly Stopwatch _timer = new Stopwatch();

        public string TestName { get; }
        public int Intervals { get; private set; }


        public bool IsRunning
        {
            get { return _timer.IsRunning; }
        }

        public TimeSpan Elapsed
        {
            get { return _timer.Elapsed; }
        }

        public long ElapsedMilliseconds
        {
            get { return _timer.ElapsedMilliseconds; }
        }

        public long ElapsedTicks
        {
            get { return _timer.ElapsedTicks; }
        }

        public TimeSpan AverageElapsed
        {
            get
            {
                long ticks = Elapsed.Ticks / Intervals;
                return TimeSpan.FromTicks(ticks);
            }
        }


        public CountedStopwatch(string testName)
        {
            TestName = testName;
        }


        public void Reset()
        {
            Intervals = 0;
            _timer.Reset();
        }

        public void Restart()
        {
            Reset();
            Start();
        }

        public void Start()
        {
            Intervals++;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
