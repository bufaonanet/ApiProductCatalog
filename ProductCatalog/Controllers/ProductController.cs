using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Models;
using ProductCatalog.ViewModels.ProductViewModes;
using ProductCatalog.ViewModels;
using ProductCatalog.Repositoreis;

namespace ProductCatalog.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _context;

        public ProductController(ProductRepository context)
        {
            _context = context;
        }

        [Route("v1/products")]
        [HttpGet]
        public IEnumerable<ListProductViewModel> Get()
        {
            return _context.Get();
        }

        [Route("v1/products/{id}")]
        [HttpGet]
        public Product Get(int id)
        {
            return _context.Get(id);
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
            product.Quantity = model.Quantity;

            _context.Save(product);

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

            var product = _context.Get(model.Id);
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.Description = model.Description;
            product.Image = model.Image;
            product.LastUpdateDate = DateTime.Now;
            product.Price = model.Price;
            product.Quantity = model.Quantity;

            _context.Update(product);

            return new ResultViewModel
            {
                Sucess = true,
                Message = "Produto alterado com sucesso",
                Data = product
            };
        }

    }
}
