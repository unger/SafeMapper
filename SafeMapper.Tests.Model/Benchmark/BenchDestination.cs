namespace SafeMapper.Tests.Model.Benchmark
{
    public class BenchDestination
    {
        public class Int1
        {
            public string str1;
            public string str2;
            public int i;
        }

        public class Int2
        {
            public Int1 i1;
            public Int1 i2;
            public Int1 i3;
            public Int1 i4;
            public Int1 i5;
            public Int1 i6;
            public Int1 i7;
        }

        public Int2 i1 { get; set; }
        public Int2 i2 { get; set; }
        public Int2 i3 { get; set; }
        public Int2 i4 { get; set; }
        public Int2 i5 { get; set; }
        public Int2 i6 { get; set; }
        public Int2 i7 { get; set; }
        public Int2 i8 { get; set; }

        public long n2 = 2;
        public long n3 = 3;
        public long n4 = 4;
        public long n5 = 5;
        public long n6 = 6;
        public long n7 = 7;
        public long n8 = 8;
        public long n9 = 9;

        public string s1;
        public string s2;
        public string s3;
        public string s4;
        public string s5;
        public string s6;
        public string s7;
    }
}
