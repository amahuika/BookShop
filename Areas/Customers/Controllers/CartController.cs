using BookShop.Data.Repository.IRepository;
using BookShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShop.Areas.Customers.Controllers
{
    [Area("Customers")]
    [Authorize]
    public class CartController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM shoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public IActionResult Index()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            shoppingCartVM = new ShoppingCartVM()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, "Product")

            };
            foreach (var item in shoppingCartVM.CartList)
            {
                item.Price = GetPriceBasedOnQuantity(item.Count, item.Product.Price, item.Product.Price50, item.Product.Price100);
                shoppingCartVM.TotalPrice += (item.Price * item.Count);
            }

            return View(shoppingCartVM);
        }


        public IActionResult Summary()
        {

      /*      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            shoppingCartVM = new ShoppingCartVM()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, "Product")

            };
            foreach (var item in shoppingCartVM.CartList)
            {
                item.Price = GetPriceBasedOnQuantity(item.Count, item.Product.Price, item.Product.Price50, item.Product.Price100);
                shoppingCartVM.TotalPrice += (item.Price * item.Count);
            }

            return View(shoppingCartVM);*/
            return View();

        }


        public IActionResult Plus(int cartId) 
        {
             var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(i => i.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            

            return RedirectToAction(nameof(Index));

       
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(i => i.Id == cartId);
            if(cart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(i => i.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();


            return RedirectToAction(nameof(Index));


        }






        public double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100) 
        {
        if(quantity <= 50) 
            {
                return price;
            } else
            {
                if(quantity <= 99)
                {
                    return price50;
                }
                else
                {
                    return price100;
                }
            }




        }

    }
}
