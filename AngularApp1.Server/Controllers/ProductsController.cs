using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularApp1.Server.Models;
using AngularApp1.Server.Service;
using AngularApp1.Server.CustomExceptions;

namespace AngularApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _service;

        public ProductsController(IProductsService service)
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
            catch(ApiException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetProducts>> GetProducts(int id)
        {
            try
            {
                bool isExists=await _service.productExists(id);
                if (isExists)
                {
                    var products =await _service.GetProductById(id);

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
            catch (ApiException ex) {
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

        [HttpPost]
        public async Task<ActionResult<Products>> PostProducts(Products products)
        {
            try
            {
                await _service.createProduct(products);

                return CreatedAtAction("GetProducts", products);
            }
            catch(ApiException ex)
            {
                return StatusCode(ex.StatusCode,ex.Message);
            }
             
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            try
            {
                bool isExists = await _service.productExists(id);
                if (isExists)
                {
                    bool isDeleted =await _service.deleteProduct(id);

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
            catch (ApiException ex) {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

    }
}
