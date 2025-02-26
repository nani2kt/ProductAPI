using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // In-memory product store for demonstration.
        private static List<Product> products = new List<Product>
    {
        new Product { Id = 1, Name = "Laptop", Description = "High performance laptop", Price = 1200 },
        new Product { Id = 2, Name = "Smartphone", Description = "Latest smartphone", Price = 800 }
    };

        [HttpGet]
        [Authorize]
        public IActionResult GetProducts() => Ok(products);

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        //[Authorize]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            product.Id = products.Max(p => p.Id) + 1;
            products.Add(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
       // [Authorize]
        public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize]
        public IActionResult DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            products.Remove(product);
            return NoContent();
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
