using System;

namespace Newbe.Claptrap.IGrain
{
    public class SkuStateData : IStateData
    {
        public string Id { get; set; }
        public SkuStatus Status { get; set; }
    }
}