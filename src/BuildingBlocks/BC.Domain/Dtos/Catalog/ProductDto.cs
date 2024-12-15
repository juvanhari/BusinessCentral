namespace BC.Domain.Dtos.Catalog
{
    public record ProductCommandDto(string Company, string ItemNo, string Description, string Category, decimal UnitPrice);

    public record ProductQueryDto : ProductCommandDto
    {
        public Guid Id { get; set; }

        public ProductQueryDto(Guid Id, string Company, string ItemNo, string Description, string Category, decimal UnitPrice) 
            : base(Company, ItemNo, Description, Category, UnitPrice)
        {
            this.Id = Id;
        }

    } 
}
