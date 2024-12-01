using System.Text.Json;

namespace BC.Api.Features.Catalog.CreateProduct
{
    public record CreateProductCommand(ProductDto ProductDto)
       : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.ProductDto.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.ProductDto.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ProductDto.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.ProductDto.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    public class CreateProductHandler(IHttpClientFactory clientFactory) : 
        ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {

            var product = new Product()
            {
                Name = command.ProductDto.Name,
                Description = command.ProductDto.Description,
                Category = command.ProductDto.Category,
                ImageFile = command.ProductDto.ImageFile,
                Price = command.ProductDto.Price,
            };

            var httpClient = clientFactory.CreateClient("MSD365BC");
            var httpResponseMessage = await httpClient.GetAsync(
                $"discounts/{product.Id}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                 var content = await httpResponseMessage.Content.ReadAsStringAsync();
                 int discount = JsonSerializer.Deserialize<int>(content);

                product.Price = product.Price - (product.Price * discount / 100);
            }

            return new CreateProductResult(product.Id);
        }
    }
}
