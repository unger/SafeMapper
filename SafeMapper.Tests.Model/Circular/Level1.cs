namespace SafeMapper.Tests.Model.Circular
{
    using System.Collections.Generic;

    public class Level1
    {
        public string Property1 { get; set; }

        public int Property2 { get; set; }

        public IEnumerable<Level2> Level2 { get; set; }
    }
}