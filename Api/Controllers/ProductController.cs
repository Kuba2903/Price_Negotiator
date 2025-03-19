using Api.DTO_s;
using Api.Services.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        //[Authorize(Roles = "Employee")]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDTO productDto)
        {
            var product = await _productService.AddProduct(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProduct(id);

            if(product != null)
                return Ok(product);
            else
                return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return Ok(await _productService.GetAllProducts());
        }
    }
}
