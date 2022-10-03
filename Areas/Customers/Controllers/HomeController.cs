using BookShop.Data.Repository.IRepository;
using BookShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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

        public IActionResult Details(int productId)
        {

            Cart cartObj = new Cart
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == productId, "Category,CoverType"),
               
            };
         
           

            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(Cart cart)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
          // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            cart.ApplicationUserId = claims.Value;

            Cart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.ApplicationUserId == claims.Value && c.ProductId == cart.ProductId);

            if(cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(cart);

            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, cart.Count);

            }
            _unitOfWork.Save();
            TempData["success"] = "Item added to cart successfully!";
    
          

            return RedirectToAction(nameof(Index));
        }


    }
}