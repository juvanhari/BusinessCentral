namespace BC.Domain.Dtos.Catalog
{
    public record ProductDto(string Name, string Description, List<string> Category, string ImageFile, decimal Price);
}
