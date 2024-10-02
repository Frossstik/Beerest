using Beerest.Models;
using Microsoft.AspNetCore.Components.Routing;
using Newtonsoft.Json;

namespace Beerest.HAL
{
    public class BeerHalResponse
    {
        public List<Beers> Beers { get; set; }

        [JsonProperty("_links")]
        public Dictionary<string, HalLink> Links { get; set; }

        public BeerHalResponse()
        {
            Links = new Dictionary<string, HalLink>();
        }
    }
}
