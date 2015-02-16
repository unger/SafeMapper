namespace SafeMapper.Tests.Performance.Util
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    public class Clock
    {
        private interface IStopwatch
        {
            bool IsRunning { get; }

            TimeSpan Elapsed { get; }

            void Start();

            void Stop();

            void Reset();
        }

        public static double BenchmarkTime(Action action, int iterations = 10000)
        {
            return Benchmark<TimeWatch>(action, iterations);
        }

        public static double BenchmarkCpu(Action action, int iterations = 10000)
        {
            return Benchmark<CpuWatch>(action, iterations);
        }

        private static double Benchmark<T>(Action action, int iterations) where T : IStopwatch, new()
        {
            // clean Garbage
            GC.Collect();

            // wait for the finalizer queue to empty
            GC.WaitForPendingFinalizers();

            // clean Garbage
            GC.Collect();

            // warm up
            action();

            var stopwatch = new T();
            var timings = new double[5];
            for (int i = 0; i < timings.Length; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                for (int j = 0; j < iterations; j++)
                {
                    action();
                }

                stopwatch.Stop();
                timings[i] = stopwatch.Elapsed.Ticks;
            }

            return NormalizedMean(timings);
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

        private class TimeWatch : IStopwatch
        {
            private Stopwatch stopwatch = new Stopwatch();

            public TimeWatch()
            {
                if (!Stopwatch.IsHighResolution)
                {
                    throw new NotSupportedException("Your hardware doesn't support high resolution counter");
                }

                // prevent the JIT Compiler from optimizing Fkt calls away
                long seed = Environment.TickCount;

                // use the second Core/Processor for the test
                Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2);

                // prevent "Normal" Processes from interrupting Threads
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

                // prevent "Normal" Threads from interrupting this thread
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
            }

            public TimeSpan Elapsed
            {
                get
                {
                    return this.stopwatch.Elapsed;
                }
            }

            public bool IsRunning
            {
                get
                {
                    return this.stopwatch.IsRunning;
                }
            }

            public void Start()
            {
                this.stopwatch.Start();
            }

            public void Stop()
            {
                this.stopwatch.Stop();
            }

            public void Reset()
            {
                this.stopwatch.Reset();
            }
        }

        private class CpuWatch : IStopwatch
        {
            private TimeSpan startTime;

            private TimeSpan endTime;

            private bool isRunning;

            public TimeSpan Elapsed
            {
                get
                {
                    if (this.IsRunning)
                    {
                        throw new NotImplementedException(
                            "Getting elapsed span while watch is running is not implemented");
                    }

                    return this.endTime - this.startTime;
                }
            }

            public bool IsRunning
            {
                get
                {
                    return this.isRunning;
                }
            }

            public void Start()
            {
                this.startTime = Process.GetCurrentProcess().TotalProcessorTime;
                this.isRunning = true;
            }

            public void Stop()
            {
                this.endTime = Process.GetCurrentProcess().TotalProcessorTime;
                this.isRunning = false;
            }

            public void Reset()
            {
                this.startTime = TimeSpan.Zero;
                this.endTime = TimeSpan.Zero;
            }
        }
    }
}
