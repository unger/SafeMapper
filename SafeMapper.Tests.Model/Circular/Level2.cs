namespace SafeMapper.Tests.Model.Circular
{
    using System.Collections.Generic;

    public class Level2
    {
        public Dictionary<string, Level3> Level3 { get; set; }
    }
}