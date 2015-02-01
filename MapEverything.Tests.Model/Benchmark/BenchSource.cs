namespace MapEverything.Tests.Model.Benchmark
{
    public class BenchSource
    {
        public class Int1
        {
            public string str1 = "1";
            public string str2 = null;
            public int i = 10;
        }

        public class Int2
        {
            public Int1 i1 = new Int1();
            public Int1 i2 = new Int1();
            public Int1 i3 = new Int1();
            public Int1 i4 = new Int1();
            public Int1 i5 = new Int1();
            public Int1 i6 = new Int1();
            public Int1 i7 = new Int1();
        }

        public Int2 i1 = new Int2();
        public Int2 i2 = new Int2();
        public Int2 i3 = new Int2();
        public Int2 i4 = new Int2();
        public Int2 i5 = new Int2();
        public Int2 i6 = new Int2();
        public Int2 i7 = new Int2();
        public Int2 i8 = new Int2();

        public int n2;
        public long n3;
        public byte n4;
        public short n5;
        public uint n6;
        public int n7;
        public int n8;
        public int n9;

        public string s1 = "1";
        public string s2 = "2";
        public string s3 = "3";
        public string s4 = "4";
        public string s5 = "5";
        public string s6 = "6";
        public string s7 = "7";

    }
}