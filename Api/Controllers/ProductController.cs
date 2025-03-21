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


        /// <summary>
        /// Creates a new product (requires Employee role)
        /// </summary>
        /// <param name="productDto">Product data</param>
        /// <returns>Newly created product</returns>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">If the product data is invalid</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="403">If user is not authorized as Employee</response>
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDTO productDto)
        {
            ValidationResult result = await _validator.ValidateAsync(productDto);
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


        /// <summary>
        /// Retrieves a specific product by id
        /// </summary>
        /// <param name="id">The ID of the product</param>
        /// <returns>The requested product</returns>
        /// <response code="200">Returns the requested product</response>
        /// <response code="404">If the product is not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProduct(id);

            if(product != null)
                return Ok(product);
            else
                return NotFound();
        }


        /// <summary>
        /// Retrieves all products
        /// </summary>
        /// <returns>List of all products</returns>
        /// <response code="200">Returns all products</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return Ok(await _productService.GetAllProducts());
        }
    }
}
