using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels.ProductViewModes;
using ProductCatalog.ViewModels;

namespace ProductCatalog.Controllers
{
    public class ProductController : Controller
    {
        private readonly StoreDataContext _context;

        public ProductController(StoreDataContext context)
        {
            _context = context;
        }

        [Route("v1/products")]
        [HttpGet]
        public IEnumerable<ListProductViewModel> Get()
        {
            return _context.Products
                .Include(x => x.Category)
                .Select(x => new ListProductViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    Category = x.Category.Title,
                    CategoryId = x.Category.Id
                })
                .AsNoTracking()
                .ToList();
        }

        [Route("v1/products/{id}")]
        [HttpGet]
        public Product Get(int id)
        {
            return _context.Products.Where(x => x.Id == id).FirstOrDefault();
        }        

        [Route("v1/products")]
        [HttpPost]
        public ResultViewModel Post([FromBody] EditorProdutcViewModel model)
        {
            if (model == null)
            {
                return new ResultViewModel
                {
                    Sucess = false,
                    Message = "nada enviado",
                    Data = null
                };
            }

            model.Validate();

            if (model.Invalid)
            {
                return new ResultViewModel
                {
                    Sucess = false,
                    Message = "Não foi possível cadastrar o produto",
                    Data = model.Notifications
                };
            }

            var product = new Product();
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.DateCreate = DateTime.Now;
            product.Description = model.Description;
            product.Image = model.Image;
            product.LastUpdateDate = DateTime.Now;
            product.Price = model.Price;
            product.Quantity = model.Quatity;

            _context.Products.Add(product);
            _context.SaveChanges();

            return new ResultViewModel
            {
                Sucess = true,
                Message = "Produto cadastrado com sucesso",
                Data = product
            };
        }

        [Route("v1/products")]
        [HttpPut]
        public ResultViewModel Put([FromBody] EditorProdutcViewModel model)
        {
            if (model == null)
            {
                return new ResultViewModel
                {
                    Sucess = false,
                    Message = "nada enviado",
                    Data = null
                };
            }

            model.Validate();

            if (model.Invalid)
            {
                return new ResultViewModel
                {
                    Sucess = false,
                    Message = "Não foi possível alterar o produto",
                    Data = model.Notifications
                };
            }

            var product = _context.Products.Find(model.Id);
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.Description = model.Description;
            product.Image = model.Image;
            product.LastUpdateDate = DateTime.Now;
            product.Price = model.Price;
            product.Quantity = model.Quatity;

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            return new ResultViewModel
            {
                Sucess = true,
                Message = "Produto alterado com sucesso",
                Data = product
            };
        }


    }
}
