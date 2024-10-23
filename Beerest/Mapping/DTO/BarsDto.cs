namespace Beerest.Mapping.DTO
{
    public class BarsDto
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public List<int> BeerIds { get; set; } = new List<int>();
    }
}
