namespace SafeMapper.Tests.Model.Circular
{
    using System.Collections.Generic;

    public class Level1
    {
        public IEnumerable<Level2> Level2 { get; set; }
    }
}