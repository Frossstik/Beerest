namespace Beerest.HAL
{
    public class HalLink
    {
        public string Href { get; set; }

        public HalLink(string href)
        {
            Href = href;
        }
    }
}
