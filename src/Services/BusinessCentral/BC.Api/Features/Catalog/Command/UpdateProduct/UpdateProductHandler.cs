namespace BC.Api.Features.Catalog.Command.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, ProductCommandDto Product)
    : ICommand<UpdateProductResult>;

    public record UpdateProductResult(ProductQueryDto Product);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Product.Company).NotEmpty().WithMessage("Company is required");
            RuleFor(x => x.Product.ItemNo).NotEmpty().WithMessage("ItemNo is required");
            RuleFor(x => x.Product.Description).NotEmpty().WithMessage("Description is required");
        }
    }

    public class UpdateProductHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {

            var productId = command.Id;
            var product = await dbContext.Products
                .FindAsync([productId], cancellationToken: cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product", productId.ToString());
            }

            product.ItemNo = command.Product.ItemNo;
            product.Description = command.Product.Description;
            product.Company = command.Product.Company;
            product.UnitPrice = command.Product.UnitPrice;

            dbContext.Products.Update(product);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(product.ToProductDto());
        }
    }
}
