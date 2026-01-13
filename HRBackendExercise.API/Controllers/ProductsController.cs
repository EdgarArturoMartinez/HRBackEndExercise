using HRBackendExercise.API.Models;
using HRBackendExercise.API.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace HRBackendExercise.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productsService;

        public ProductsController(IProductService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var products = _productsService.GetAll();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while retrieving products: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var product = _productsService.GetById(id);
                if (product == null)
                    return NotFound(new { message = $"Product with ID {id} not found." });

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while retrieving the product: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                if (product == null)
                    return BadRequest(new { message = "Product data is required." });

                if (string.IsNullOrWhiteSpace(product.SKU))
                    return BadRequest(new { message = "SKU is required." });

                if (product.Price <= 0)
                    return BadRequest(new { message = "Price must be greater than 0." });

                var createdProduct = _productsService.Create(product);
                return CreatedAtAction(nameof(Get), new { id = createdProduct.Id }, createdProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while creating the product: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            try
            {
                if (product == null)
                    return BadRequest(new { message = "Product data is required." });

                if (string.IsNullOrWhiteSpace(product.SKU))
                    return BadRequest(new { message = "SKU is required." });

                if (product.Price <= 0)
                    return BadRequest(new { message = "Price must be greater than 0." });

                product.Id = id;
                _productsService.Update(product);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while updating the product: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _productsService.GetById(id);
                if (product == null)
                    return NotFound(new { message = $"Product with ID {id} not found." });

                _productsService.Delete(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while deleting the product: {ex.Message}" });
            }
        }
    }
}
