using BC.Domain.Models.Catalog;
using BC.Domain.Models.Enums;
using BC.Domain.Service;
using BuildingBlocks.BusinessCentralApi;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BC.Api.Features.Catalog.Queries.GetProductByCompany
{
    public record GetProductsByCompanyQuery(string Company, PaginationRequest PaginationRequest)
   : IQuery<GetProductsResult>;

    public record GetProductsResult(PaginatedResult<ProductQueryDto> Products);
    public class GetProductsByCompanyHandler(IApplicationDbContext dbContext,BusinessCentralHeader header, 
        IMSDToken msdCentralToken,IHttpClientFactory clientFactory) : IQueryHandler<GetProductsByCompanyQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsByCompanyQuery query, CancellationToken cancellationToken)
        {
            RequestSource source = Enum.Parse<RequestSource>(header.Value);

            return source == RequestSource.Database ? await GetProductsByCompanyFromDatabase(dbContext, query, cancellationToken)
                : await GetProductsByCompanyFromApi(dbContext, query, cancellationToken);
        }

        private async Task<GetProductsResult> GetProductsByCompanyFromDatabase(IApplicationDbContext dbContext, GetProductsByCompanyQuery query, CancellationToken cancellationToken)
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

        private async Task<GetProductsResult> GetProductsByCompanyFromApi(IApplicationDbContext dbContext, GetProductsByCompanyQuery query, CancellationToken cancellationToken)
        {
            List<ProductQueryDto> products = new();
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var token = await msdCentralToken.GetAuthToken();

            var httpClient = clientFactory.CreateClient("MSD365BC");
            // Add the Authorization header with the bearer token
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponseMessage = await httpClient.GetAsync(
                $"ECOM_ProcessRequest?company={query.Company}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                var odataResponse = JsonSerializer.Deserialize<ODataResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Step 2: Deserialize the "value" property into the Item object
                if (odataResponse != null && !string.IsNullOrEmpty(odataResponse.Value))
                {
                    var product  = JsonSerializer.Deserialize<ProductQueryDto>(odataResponse.Value);
                    products.Add(product!);
                }
            }

            return new GetProductsResult(
               new PaginatedResult<ProductQueryDto>(
                   pageIndex,
                   pageSize,
                   1,
                   products));
        }
    }
}
