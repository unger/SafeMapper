namespace MapEverything.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public abstract class ProfileBase
    {
        protected int[] iterations = { 100, 1000, 10000, 100000 };

        protected int MaxIterations 
        {
            get
            {
                return this.iterations.Last();
            }
        }

        public abstract void Execute();

        protected void AddResult(string description, Action<int> func)
        {
            Console.Write("{0, -30}", description);
            foreach (var iteration in this.iterations)
            {
                var result = this.Profile(description, iteration, func);
                Console.Write("{0, 12}", result.Item2);
            }

            Console.WriteLine();
        }

        protected void WriteHeader(string headline = "")
        {
            Console.WriteLine();
            Console.WriteLine(headline);
            Console.Write("{0, -30}", string.Empty);
            foreach (var iteration in this.iterations)
            {
                Console.Write("{0, 12}", iteration);
            }

            Console.WriteLine();
        }

        private Tuple<string, double> Profile(string description, int iterations, Action<int> func)
        {
            try
            {
                // warm up 
                func(0);

                var watch = new Stopwatch();

                // clean up
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                watch.Start();
                for (int i = 0; i < iterations; i++)
                {
                    func(i);
                }

                watch.Stop();
                return new Tuple<string, double>(string.Format("{0,-40}{1} ms", description, watch.Elapsed.TotalMilliseconds), watch.Elapsed.TotalMilliseconds);
            }
            catch (Exception e)
            {
                return new Tuple<string, double>(string.Format("{0,-40} throws exception {1}", description, e.Message), -1);
            }
        }
    }
}
