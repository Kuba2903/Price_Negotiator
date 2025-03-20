using Api.DTO_s;
using Api.Services.Interfaces;
using Data.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IValidator<CreateProductDTO> _validator;
        public ProductController(IProductService productService, IValidator<CreateProductDTO> validator)
        {
            _productService = productService;
            _validator = validator;
        }

        [HttpPost]
        //[Authorize(Roles = "Employee")]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDTO productDto)
        {
            ValidationResult result = _validator.Validate(productDto);
            if (result.IsValid)
            {
                var product = await _productService.AddProduct(productDto);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            else
            {
                return BadRequest(result.Errors);
            }
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
