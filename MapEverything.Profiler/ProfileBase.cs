namespace MapEverything.Profiler
{
    using System;
    using System.Diagnostics;

    public abstract class ProfileBase
    {
        public void Execute()
        {
            this.ExecuteWrapper(100);
            this.ExecuteWrapper(1000);
            this.ExecuteWrapper(10000);
            this.ExecuteWrapper(100000);
            this.ExecuteWrapper(1000000);
        }

        protected virtual void ExecuteWrapper(int iterations)
        {
            Console.WriteLine("Iterations {0}", iterations);
            Console.WriteLine("========================================================");
            this.Execute(iterations);
            Console.WriteLine();
        }

        protected abstract void Execute(int iterations);

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
