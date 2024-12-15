namespace BC.Api.Features.Catalog.Command.CreateProduct
{
    public record CreateProductCommand(ProductCommandDto Product)
       : ICommand<CreateProductResult>;

    public record CreateProductResult(ProductQueryDto Product);


    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Product.Company).NotEmpty().WithMessage("Company is required");
            RuleFor(x => x.Product.ItemNo).NotEmpty().WithMessage("ItemNo is required");
            RuleFor(x => x.Product.Description).NotEmpty().WithMessage("Description is required");
        }
    }

    public class CreateProductHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product()
            {
                Company = command.Product.Company,
                ItemNo = command.Product.ItemNo,
                Description = command.Product.Description,
                Category = command.Product.Category,
                UnitPrice = command.Product.UnitPrice,
            };

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.ToProductDto());
        }
    }
}
