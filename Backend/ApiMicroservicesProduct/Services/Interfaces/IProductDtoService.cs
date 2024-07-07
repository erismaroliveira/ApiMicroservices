using ApiMicroservicesProduct.Dtos;

namespace ApiMicroservicesProduct.Services.Interfaces;

public interface IProductDtoService : IGenericService<ProductDto>
{
    Task<IEnumerable<ProductDto>> GetSearchProductsDtoAsync(string keyword);
    Task<IEnumerable<ProductDto>> GetProductsByCategoriesDtoAsync(string categoryStr);
}
