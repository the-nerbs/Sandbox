using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PerfTesting
{
    class Program
    {
        const int TimedIntervals = 100000;
        const int CollectionSize = 100;
        const int StringSize = 5;
        const string StringContents = "abcdefghijklmnopqrstuvwxyz";

        static int stringIndex = 0;


        [MethodImpl(MethodImplOptions.NoInlining)]
        static void DontElide<T>(T value)
        { }


        static CountedStopwatch Time_Foreach_Array(string[] list)
        {
            var timer = new CountedStopwatch("Foreach_Array");

            for (int i = 0; i < TimedIntervals; i++)
            {
                timer.Start();

                foreach (var item in list)
                {
                    DontElide(item);
                }

                timer.Stop();
            }

            return timer;
        }

        static CountedStopwatch Time_For_Array(string[] list)
        {
            var timer = new CountedStopwatch("For_Array");

            for (int i = 0; i < TimedIntervals; i++)
            {
                timer.Start();

                for (int j = 0; j < list.Length; j++)
                {
                    var item = list[j];
                    DontElide(item);
                }

                timer.Stop();
            }

            return timer;
        }

        static CountedStopwatch Time_Foreach_List(List<string> list)
        {
            var timer = new CountedStopwatch("Foreach_List");

            for (int i = 0; i < TimedIntervals; i++)
            {
                timer.Start();

                foreach (var item in list)
                {
                    DontElide(item);
                }

                timer.Stop();
            }

            return timer;
        }

        static CountedStopwatch Time_For_List(List<string> list)
        {
            var timer = new CountedStopwatch("For_List");

            for (int i = 0; i < TimedIntervals; i++)
            {
                timer.Start();

                for (int j = 0; j < list.Count; j++)
                {
                    var item = list[j];
                    DontElide(item);
                }

                timer.Stop();
            }

            return timer;
        }

        static CountedStopwatch Time_Foreach_IList(IList<string> list)
        {
            var timer = new CountedStopwatch($"Foreach_IList ({list.GetType()})");

            for (int i = 0; i < TimedIntervals; i++)
            {
                timer.Start();

                foreach (var item in list)
                {
                    DontElide(item);
                }

                timer.Stop();
            }

            return timer;
        }

        static CountedStopwatch Time_For_IList(IList<string> list)
        {
            var timer = new CountedStopwatch($"For_IList ({list.GetType()})");

            for (int i = 0; i < TimedIntervals; i++)
            {
                timer.Start();

                for (int j = 0; j < list.Count; j++)
                {
                    var item = list[j];
                    DontElide(item);
                }

                timer.Stop();
            }

            return timer;
        }


        static string NextString()
        {
            var chars = new char[StringSize];

            for (int i = 0; i < StringSize; i++)
            {
                chars[i] = StringContents[stringIndex];
                stringIndex = (stringIndex + 1) % StringContents.Length;
            }

            return new string(chars);
        }


        static void Main(string[] args)
        {
            // Data setup:
            var list = new List<string>(capacity: CollectionSize);

            for (int i = 0; i < list.Capacity; i++)
            {
                list.Add(NextString());
            }

            var array = list.ToArray();


            // perf counting:
            CountedStopwatch[] times =
            {
                // array tests:
                Time_Foreach_Array(array),
                Time_For_Array(array),
                Time_Foreach_IList(array),
                Time_For_IList(array),

                // List<T> tests:
                Time_Foreach_List(list),
                Time_For_List(list),
                Time_Foreach_IList(list),
                Time_For_IList(list),
            };


            // output:
            string sepLine = new string('=', 80);
            foreach (var result in times)
            {
                Console.WriteLine(sepLine);
                Console.WriteLine("Test: " + result.TestName);
                Console.WriteLine();
                Console.WriteLine("# Intervals:     " + result.Intervals);
                Console.WriteLine("Total Time:      " + result.Elapsed.TotalMilliseconds + " ms");
                Console.WriteLine("Average Time:    " + result.AverageElapsed.TotalMilliseconds + " ms");
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ReadKey(intercept: true);
        }
    }
}
