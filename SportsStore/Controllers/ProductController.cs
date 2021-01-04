using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _repo;
        public int PageSize = 4;
        public ProductController(IProductRepository repo)
        {
            _repo = repo;
        }
        public ViewResult List(string category, int productPage = 1) =>
            View(new ProductsListViewModel
            {
                Products = _repo.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null
                    ? _repo.Products.Count()
                    : _repo.Products.Where(x => x.Category == category).Count()
                },
                CurrentCategory = category
            });

    }
}
