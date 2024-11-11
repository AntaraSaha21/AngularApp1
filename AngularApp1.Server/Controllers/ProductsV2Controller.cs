using AngularApp1.Server.CustomExceptions;
using AngularApp1.Server.Models;
using AngularApp1.Server.Service;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularApp1.Server.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProductsV2Controller : ControllerBase
    {
        private readonly IProductsService _service;
        public ProductsV2Controller(IProductsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProducts>>> GetProducts()
        {
            try
            {
                return Ok(await _service.GetAll());
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetProducts>> GetProducts(int id)
        {
            try
            {
                bool isExists = await _service.productExists(id);
                if (isExists)
                {
                    var products = await _service.GetProductById(id);

                    if (products == null)
                    {
                        return NotFound();
                    }

                    return Ok(products);
                }
                else
                {
                    return BadRequest("Product not exists!");
                }
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Products>> PostProducts(Products products)
        {
            try
            {
                await _service.createProduct(products);

                return CreatedAtAction(nameof(GetProducts), new { version = "2.0", id = products.Id }, products);
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(int id, Products products)
        {
            try
            {
                bool isExists = await _service.productExists(id);
                if (isExists)
                {
                    await _service.updateProduct(products, id);
                }
                else
                {
                    return BadRequest("Product not exists!");
                }
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            try
            {
                bool isExists = await _service.productExists(id);
                if (isExists)
                {
                    bool isDeleted = await _service.deleteProduct(id);

                    if (isDeleted)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest("Product not exists!");
                }
            }
            catch (ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

    }
}
