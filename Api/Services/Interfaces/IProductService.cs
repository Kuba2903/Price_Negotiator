using Api.DTO_s;
using Data.Entities;

namespace Api.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> AddProduct(CreateProductDTO productDto);
        Task<Product> GetProduct(int id);
        Task<IEnumerable<Product>> GetAllProducts();
    }
}
