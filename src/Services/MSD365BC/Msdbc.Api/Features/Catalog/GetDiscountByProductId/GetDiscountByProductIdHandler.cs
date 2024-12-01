namespace Msdbc.Api.Features.Catalog.GetDiscount
{
    public record GetDiscounttByProductIdQuery(Guid Id) : IQuery<GetDiscountByProductIdResult>;

    public record GetDiscountByProductIdResult(int Discount);
    public class GetDiscountByProductIdHandler : IQueryHandler<GetDiscounttByProductIdQuery, GetDiscountByProductIdResult>
    {
        public async Task<GetDiscountByProductIdResult> Handle(GetDiscounttByProductIdQuery query, CancellationToken cancellationToken)
        {

            //var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

            //if (product is null)
            //{
            //    throw new ProductNotFoundException(query.Id.ToString());
            //}

            return new GetDiscountByProductIdResult(20);
        }
    }
}
