using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SafeMapper.Profiler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //new ProfileCreation().Execute();

            //new ProfileArrayCreation().Execute();

            //new ProfilePropertyGetSet().Execute();

            //new ProfileArrayToArray().Execute();

            //new ProfileArrayToList().Execute();

            //new ProfileListToArray().Execute();

            //new ProfileListToList().Execute();

            new ProfileConversion().Execute();

            //new ProfileInvalidConversion().Execute();

            //new ProfileStringConcat().Execute();

            //new ProfileTypeOf().Execute();

            //new ProfileIntParse().Execute();

            //new ProfileCreateConverter().Execute();
        }
    }
}
