namespace Api.DTO_s
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }

        public string? Description { get; set; }
    }
}
