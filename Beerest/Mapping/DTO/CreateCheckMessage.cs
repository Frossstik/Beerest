namespace Beerest.Mapping.DTO
{
    public class CreateCheckMessage
    {
        public string OrderId { get; set; }
        public List<CheckItemDto> Items { get; set; }
    }
}
