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
            var timer = new CountedStopwatch(TestName("foreach", list.GetType()));

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
            var timer = new CountedStopwatch(TestName("for", list.GetType()));

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
            var timer = new CountedStopwatch(TestName("foreach", list.GetType()));

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
            var timer = new CountedStopwatch(TestName("for", list.GetType()));

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

        static CountedStopwatch Time_Foreach_RefEnumerator(ReadOnlyCollectionRefEnumerator<string> list)
        {
            var timer = new CountedStopwatch(TestName("foreach", list.GetType()));

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

        static CountedStopwatch Time_For_RefEnumerator(ReadOnlyCollectionRefEnumerator<string> list)
        {
            var timer = new CountedStopwatch(TestName("for", list.GetType()));

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

        static CountedStopwatch Time_Foreach_ValEnumerator(ReadOnlyCollectionValEnumerator<string> list)
        {
            var timer = new CountedStopwatch(TestName("foreach", list.GetType()));

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

        static CountedStopwatch Time_For_ValEnumerator(ReadOnlyCollectionValEnumerator<string> list)
        {
            var timer = new CountedStopwatch(TestName("for", list.GetType()));

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

        static CountedStopwatch Time_Foreach_YieldEnumerator(ReadOnlyCollectionYieldEnumerator<string> list)
        {
            var timer = new CountedStopwatch(TestName("foreach", list.GetType()));

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

        static CountedStopwatch Time_For_YieldEnumerator(ReadOnlyCollectionYieldEnumerator<string> list)
        {
            var timer = new CountedStopwatch(TestName("for", list.GetType()));

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
            var timer = new CountedStopwatch(TestName("foreach", list.GetType(), typeof(IList<string>)));

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
            var timer = new CountedStopwatch(TestName("for", list.GetType(), typeof(IList<string>)));

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


        static CountedStopwatch TimeForeach<T>(T list)
            where T : IList<string>
        {
            var timer = new CountedStopwatch(TestName("Generic caller - foreach", list.GetType(), typeof(T)));

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

        static CountedStopwatch TimeFor<T>(T list)
            where T : IList<string>
        {
            var timer = new CountedStopwatch(TestName("Generic caller - for", list.GetType(), typeof(T)));

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

        static string TestName(string testName, Type actualType, Type paramType = null)
        {
            if (paramType == null || actualType == paramType)
            {
                return $"{testName} ({TypeName(actualType)})";
            }
            else
            {
                return $"{testName} ({TypeName(actualType)} as {TypeName(paramType)})";
            }
        }

        static string TypeName(Type t)
        {
            if (t.IsArray)
            {
                return TypeName(t.GetElementType()) + "[]";
            }
            else if (t.IsGenericType)
            {
                var sb = new StringBuilder(t.Name);

                // strip the generic type list
                while (sb[sb.Length - 1] != '`')
                {
                    sb.Length--;
                }
                sb.Length--;

                sb.Append('<');
                foreach (var typeArg in t.GetGenericArguments())
                {
                    sb.Append(TypeName(typeArg));
                }
                sb.Append('>');

                return sb.ToString();
            }
            else
            {
                return t.Name;
            }
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
            var refList = new ReadOnlyCollectionRefEnumerator<string>(list);
            var valList = new ReadOnlyCollectionValEnumerator<string>(list);
            var yieldList = new ReadOnlyCollectionYieldEnumerator<string>(list);


            // warm up QPC.  I've noticed the first use can have a small delay, so
            // make sure we don't get that while gathering measurements.
            {
                var warmup = new System.Diagnostics.Stopwatch();
                warmup.Start();
                warmup.Restart();
                warmup.Restart();
            }


            // test cases:
            Func<CountedStopwatch>[] tests =
            {
                // array tests:
                () => Time_Foreach_Array(array),
                () => Time_For_Array(array),
                () => Time_Foreach_IList(array),
                () => Time_For_IList(array),

                // List<T> tests:
                () => Time_Foreach_List(list),
                () => Time_For_List(list),
                () => Time_Foreach_IList(list),
                () => Time_For_IList(list),

                // Ref enumerator tests:
                () => Time_Foreach_RefEnumerator(refList),
                () => Time_For_RefEnumerator(refList),
                () => Time_Foreach_IList(refList),
                () => Time_For_IList(refList),

                // Val enumerator tests:
                () => Time_Foreach_ValEnumerator(valList),
                () => Time_For_ValEnumerator(valList),
                () => Time_Foreach_IList(valList),
                () => Time_For_IList(valList),

                // Yield enumerator tests:
                () => Time_Foreach_YieldEnumerator(yieldList),
                () => Time_For_YieldEnumerator(yieldList),
                () => Time_Foreach_IList(yieldList),
                () => Time_For_IList(yieldList),

                #region Generic caller
                // While trying to clean up the tests (less duplicate code and such) I made
                // these generic TimeForeach and TimeFor functions.  However, I found that
                // they has *significantly* worse time-performance than the non-generic
                // equivalents, and in turn, make for interesting, though unique, test cases.

                // array tests:
                () => TimeForeach(array),
                () => TimeFor(array),

                // List<T> tests:
                () => TimeForeach(list),
                () => TimeFor(list),

                // Ref enumerator tests:
                () => TimeForeach(refList),
                () => TimeFor(refList),

                // Val enumerator tests:
                () => TimeForeach(valList),
                () => TimeFor(valList),

                // Yield enumerator tests:
                () => TimeForeach(yieldList),
                () => TimeFor(yieldList),

                #endregion
            };


            // test + output:
            string sepLine = new string('=', 80);
            foreach (var testCase in tests)
            {
                // try to avoid running anything GC-related while timing things.
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // run the test
                CountedStopwatch result = testCase();

                // print the test result
                Console.WriteLine(sepLine);
                Console.WriteLine("Test: " + result.TestName);
                Console.WriteLine();
                Console.WriteLine("# Intervals:     " + result.Intervals);
                Console.WriteLine("Total Time:      " + result.Elapsed.TotalMilliseconds + " ms");
                Console.WriteLine("Average Time:    " + result.AverageElapsed.TotalMilliseconds + " ms");
                Console.WriteLine();
                Console.WriteLine();
            }

            // pause if we're not writing to a file
            if (!Console.IsOutputRedirected)
            {
                Console.WriteLine("Press any key to continue . . .");
                Console.ReadKey(intercept: true);
            }
        }
    }
}
