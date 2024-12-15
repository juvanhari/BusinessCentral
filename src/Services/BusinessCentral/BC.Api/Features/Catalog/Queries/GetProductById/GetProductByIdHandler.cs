namespace BC.Api.Features.Catalog.Queries.GetProductById
{
    public record GetProductByIdQuery(Guid Id)
   : IQuery<GetProductByIdResult>;

    public record GetProductByIdResult(ProductQueryDto Product);

    public class GetProductByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {

            var productId = query.Id;
            var product = await dbContext.Products
                .FindAsync([productId], cancellationToken: cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product", productId.ToString());
            }

            return new GetProductByIdResult(
                    product.ToProductDto());
        }
    }
}
