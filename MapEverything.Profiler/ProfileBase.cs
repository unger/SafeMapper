namespace MapEverything.Profiler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public abstract class ProfileBase
    {
        private List<Tuple<string, double>> results = new List<Tuple<string, double>>();

        public void Execute()
        {
            this.ExecuteWrapper(100);
            this.ExecuteWrapper(1000);
            this.ExecuteWrapper(10000);
            this.ExecuteWrapper(100000);
            //this.ExecuteWrapper(1000000);
        }

        protected virtual void ExecuteWrapper(int iterations)
        {
            this.results.Clear();

            Console.WriteLine("Iterations {0}", iterations);
            Console.WriteLine("========================================================");
            this.Execute(iterations);
            this.results.Sort((t1, t2) => t1.Item2.CompareTo(t2.Item2));

            foreach (var result in this.results)
            {
                Console.WriteLine(result.Item1);
            }

            Console.WriteLine();
        }

        protected abstract void Execute(int iterations);

        protected void AddResult(Tuple<string, double> result)
        {
            this.results.Add(result);
        }

        protected Tuple<string, double> Profile(string description, int iterations, Action<int> func)
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
                return new Tuple<string, double>(string.Format("{0,-40} throws exception", description, e.Message), double.MaxValue);
            }
        }
    }
}
