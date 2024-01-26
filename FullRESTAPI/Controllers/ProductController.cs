using FullRESTAPI.Interfaces;
using FullRESTAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI;
using System.Reflection.Metadata.Ecma335;

namespace FullRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        // GET: api/<ProductController>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Product>))]
        public IActionResult  GetProducts()
        {
            var products = productRepository.GetProducts();
            return Ok(products);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200,Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProduct(int id)
        {
            var product = productRepository.GetProduct(id);
            if (product is null) return NotFound();

            return Ok(product);
        }

        // POST api/<ProductController>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize]
        [ServiceFilter(typeof(ValidationFilter<Product>))]
        public IActionResult CreateProduct([FromBody] Product Product)
        {
            productRepository.CreateProduct(Product);
            return Ok(Product);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize]
        [ServiceFilter(typeof(ValidationFilter<Product>))]
        public IActionResult UpdateProduct(int id, [FromBody] Product Product)
        {
            if (!productRepository.ProductExists(id)) return NotFound();

            Product _product = productRepository.GetProduct(id);

            _product.Name = Product.Name;
            _product.Price = Product.Price;
            _product.Stock = Product.Stock;

            productRepository.UpdateProduct(_product);
            return Ok(_product);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [Authorize]
        public IActionResult Delete(int id)
        {
            if (!productRepository.ProductExists(id)) return NotFound();

            productRepository.DeleteProduct(id);

            return Ok();
        }
    }
}
