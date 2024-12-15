using BC.Domain.Dtos.Catalog;

namespace BC.Api.Extensions
{
    public static class ProductExtensions
    {
        public static IEnumerable<ProductQueryDto> ToProductDtoList(this IEnumerable<Product> products)
        {
            return products.Select(product => DtoFromProduct(product));
        }

        public static ProductQueryDto ToProductDto(this Product product)
        {
            return DtoFromProduct(product);
        }

        private static ProductQueryDto DtoFromProduct(Product product)
        {
            return new ProductQueryDto(
                         Id: product.Id,
                Company: product.Company,
                ItemNo: product.ItemNo,
                Description: product.Description,
                Category: product.Category,
                UnitPrice: product.UnitPrice);
        }
    }
}
