namespace SafeMapper.Profiler
{
    using System;
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
                Console.Write("{0, 12:0.0000}", result.Item2);
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
                var ticks = Profiler.Profile(() => func(0), iterations, 100);
                var result = ((double)ticks / Stopwatch.Frequency) * 1000;

                return new Tuple<string, double>(string.Format("{0,-40}{1} ms", description, result), result);
            }
            catch (Exception e)
            {
                return new Tuple<string, double>(string.Format("{0,-40} throws exception {1}", description, e.Message), -1);
            }
        }
    }
}
