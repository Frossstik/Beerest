namespace Beerest.GraphQL.Types
{
    public class PersonsType
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Age { get; set; }

        public BarsType? Bar { get; set; }
    }
}
