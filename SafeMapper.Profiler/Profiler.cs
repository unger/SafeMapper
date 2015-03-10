namespace SafeMapper.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    public class Profiler
    {
        public static long Profile(Action action, int iterations = 100000, long warmUpTimeMs = 1200)
        {
            var stopwatch = new Stopwatch();

            // Uses the second Core or Processor for the Test
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2);

            // Prevents "Normal" processes from interrupting Threads
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
        
            // Prevents "Normal" Threads from interrupting this thread            
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            // clean Garbage
            GC.Collect();

            // wait for the finalizer queue to empty
            GC.WaitForPendingFinalizers();

            // clean Garbage
            GC.Collect();

            stopwatch.Reset();
            stopwatch.Start();

            while (stopwatch.ElapsedMilliseconds < warmUpTimeMs)
            {
                action();
            }

            stopwatch.Stop();

            var timings = new double[5];
            for (int k = 0; k < timings.Length; k++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    action();
                }

                stopwatch.Stop();

                timings[k] = stopwatch.ElapsedTicks;
            }

            return (long)NormalizedMean(timings);
        }

        private static double NormalizedMean(ICollection<double> values)
        {
            if (values.Count == 0)
            {
                return double.NaN;
            }

            var deviations = Deviations(values).ToArray();
            var meanDeviation = deviations.Sum(t => Math.Abs(t.Item2)) / values.Count;
            return deviations.Where(t => t.Item2 > 0 || Math.Abs(t.Item2) <= meanDeviation).Average(t => t.Item1);
        }

        private static IEnumerable<Tuple<double, double>> Deviations(ICollection<double> values)
        {
            if (values.Count == 0)
            {
                yield break;
            }

            var avg = values.Average();
            foreach (var d in values)
            {
                yield return Tuple.Create(d, avg - d);
            }
        }
    }
}
