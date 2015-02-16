namespace SafeMapper.Tests.Performance.Util
{
    using System;
    using System.Diagnostics;

    public class Profiler
    {
        public static long Profile(int iterations, Action func)
        {
            try
            {
                // warm up 
                func();

                var watch = new Stopwatch();

                // clean up
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                watch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    func();
                }

                watch.Stop();
                return watch.Elapsed.Ticks;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}
