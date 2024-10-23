using NuGet.Protocol;

namespace Beerest.GraphQL.Types
{
    public class BarsType
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public List<BeersType> Beers { get; set; } = new List<BeersType>();
    }
}
