using BC.Domain.Models.Base;

namespace BC.Domain.Models.Catalog
{
    public class Product : Entity<Guid>
    {

        public string ItemNo { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string Category { get; set; } = default!;

        public decimal UnitPrice { get; set; } = default!;

        public static Product Create(string company, string itemNo, string description, string category,  decimal unitPrice)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(company);
            ArgumentOutOfRangeException.ThrowIfNullOrWhiteSpace(itemNo);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Company = company,
                ItemNo = itemNo,
                Description = description,
                Category = category,
                UnitPrice = unitPrice
            };

            return product;
        }

    }
}
