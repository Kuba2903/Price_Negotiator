using Api.DTO_s;
using Api.Services.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly Dictionary<int, Product> _products = new();
        private int _nextId = 1;

        public Task<Product> AddProduct(CreateProductDTO productDto)
        {
            var product = new Product
            {
                Id = _nextId++,
                Name = productDto.Name,
                BasePrice = productDto.BasePrice,
                Description = productDto.Description
            };

            _products[product.Id] = product;
            return Task.FromResult(product);
        }

        public Task<Product> GetProduct(int id)
        {
            var product = _products.Values.FirstOrDefault(x => x.Id == id);
            if (product != null)
                return Task.FromResult(product);
            else
                throw new Exception("Product not found");
        }
       
        public Task<IEnumerable<Product>> GetAllProducts()
        {
            return Task.FromResult(_products.Values.AsEnumerable());
        }
    }
}
