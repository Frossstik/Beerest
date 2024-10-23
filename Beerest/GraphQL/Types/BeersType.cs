using System;

namespace Beerest.GraphQL.Types
{
    public class BeersType
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Country { get; set; }

        public int Volume { get; set; }

        public int VolumeAlc { get; set; }
    }
}
