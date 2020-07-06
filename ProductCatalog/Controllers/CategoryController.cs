using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.Repositoreis;

namespace ProductCatalog.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _context;

        public CategoryController(CategoryRepository context)
        {
            _context = context;
        }

        [Route("v1/categories")]
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 10)]
        public IEnumerable<Category> Get()
        {
            return _context.Get();
        }

        [Route("v1/categories/{id}")]
        [HttpGet]
        public Category Get(int id)
        {
            return _context.Get(id);
        }

        //[Route("v1/categories/{id}/products")]
        //[HttpGet]
        //public IEnumerable<Product> GetProducts(int id)
        //{
        //    return _context.Products.AsNoTracking()
        //        .Where(x => x.CategoryId == id)
        //        .ToList();
        //}

        [Route("v1/categories")]
        [HttpPost]
        public Category Post([FromBody] Category category)
        {
            _context.Save(category);

            return category;
        }

        [Route("v1/categories")]
        [HttpPut]
        public Category Put([FromBody] Category category)
        {
            _context.Update(category);

            return category;
        }

        [Route("v1/categories")]
        [HttpDelete]
        public Category Delete([FromBody] Category category)
        {
            _context.Delete(category);

            return category;
        }
    }
}
