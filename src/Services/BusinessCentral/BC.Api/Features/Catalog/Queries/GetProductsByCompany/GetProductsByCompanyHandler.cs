namespace BC.Api.Features.Catalog.Queries.GetProductByCompany
{
    public record GetProductsByCompanyQuery(string Company, PaginationRequest PaginationRequest)
   : IQuery<GetProductsResult>;

    public record GetProductsResult(PaginatedResult<ProductQueryDto> Products);
    public class GetProductsByCompanyHandler(IApplicationDbContext dbContext) : IQueryHandler<GetProductsByCompanyQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsByCompanyQuery query, CancellationToken cancellationToken)
        {

            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await dbContext.Products.Where(p => p.Company == query.Company).LongCountAsync(cancellationToken);

            var products = await dbContext.Products
                           .Where(p => p.Company == query.Company)
                           .OrderBy(o => o.Category)
                           .Skip(pageSize * pageIndex)
                           .Take(pageSize)
                           .ToListAsync(cancellationToken);

            return new GetProductsResult(
                new PaginatedResult<ProductQueryDto>(
                    pageIndex,
                    pageSize,
                    totalCount,
                    products.ToProductDtoList()));
        }
    }
}
