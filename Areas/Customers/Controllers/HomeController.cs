using BookShop.Data.Repository.IRepository;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookShop.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");


            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details(int id)
        {

            ShoppingCart cartObj = new ShoppingCart
            {
                Count = 1,
                Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id, "Category,CoverType"),
               
            };
         
           

            return View(cartObj);
        }


    }
}