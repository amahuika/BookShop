using BookShop.Data.Repository.IRepository;
using BookShop.Models;
using BookShop.Models.ViewModels;
using BookShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }



        public IActionResult Details(int orderId)
        {

            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderId == orderId, "Product")
            };

            return View(OrderVM);
        }

        [ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsPayNow()
        {

            OrderVM.OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, "ApplicationUser");

            OrderVM.OrderDetails = _unitOfWork.OrderDetails.GetAll(u => u.Id == OrderVM.OrderHeader.Id, "Product");

            // strip settings
            var domain = "https://localhost:44333/";
            var options = new SessionCreateOptions
            {

                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={OrderVM.OrderHeader.Id}",
                CancelUrl = domain + $"admin/order/Details?orderid={OrderVM.OrderHeader.Id}",
            };

            foreach (var item in OrderVM.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long?)(item.Price * 100),
                        Currency = "NZD",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title,
                        },

                    },
                    Quantity = item.Count,
                };

                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.OrderHeader.UpdatePaymentId(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);

            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }



        public IActionResult PaymentConfirmation(int orderHeaderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderHeaderId);

            if (orderHeader.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment)
            {

                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                // check if been paid
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    //update status in orderheader table
                    _unitOfWork.OrderHeader.UpdatePaymentId(orderHeaderId, orderHeader.SessionId, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, StaticDetails.PaymentStatusApproved);
                    _unitOfWork.Save();
                }



            }


            return View(orderHeaderId);
        }




        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails()
        {
            var orderHeaderDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderDb.City = OrderVM.OrderHeader.City;
            orderHeaderDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            if(orderHeaderDb.TrackingNumber != null)
            {
                orderHeaderDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;

            }
            if(orderHeaderDb.Carrier != null)
            {
                orderHeaderDb.TrackingNumber = OrderVM.OrderHeader.Carrier;
            }

            _unitOfWork.OrderHeader.Update(orderHeaderDb);
            _unitOfWork.Save();
            TempData["success"] = "Order Details Updated Successfully";
            return RedirectToAction("Details", "Order", new {orderId = orderHeaderDb.Id});
        }


        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcessing()
        {
      
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, StaticDetails.StatusInProcess); 
            _unitOfWork.Save();
            TempData["success"] = "Order Status Updated Successfully";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder()
        {
            var orderHeaderDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeaderDb.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeaderDb.OrderStatus = StaticDetails.StatusShipped;
            orderHeaderDb.ShippingDate = DateTime.Now;

            if(orderHeaderDb.OrderStatus == StaticDetails.PaymentStatusDelayedPayment)
            {
                orderHeaderDb.PaymentDueDate = DateTime.Now.AddDays(30);
            }


            _unitOfWork.OrderHeader.Update(orderHeaderDb);
            _unitOfWork.Save();
            TempData["success"] = "Order Shipped Successfully";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }


        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var orderHeaderDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id);
            // creating a refund with stripe
            if(orderHeaderDb.PaymentStatus == StaticDetails.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeaderDb.PaymentIntentId,
                   
                    
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderDb.Id, StaticDetails.StatusCancelled, StaticDetails.StatusRefunded);
            } else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderDb.Id, StaticDetails.StatusCancelled, StaticDetails.StatusCancelled);
            }
            _unitOfWork.Save();

          
            TempData["success"] = "Order Cancelled Successfully";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
        }








        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeader;

            // get orderheader details from db based on user
            if (User.IsInRole(StaticDetails.Role_Admin) || User.IsInRole(StaticDetails.Role_Employee))
            {
               
                orderHeader = _unitOfWork.OrderHeader.GetAll(null, "ApplicationUser");
            } else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                orderHeader = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId, "ApplicationUser");
            }



            
            // filter based on the status
            switch (status)
            {
                case "pending":
                orderHeader = orderHeader.Where(u => u.PaymentStatus == StaticDetails.PaymentStatusPending);
                    break;
                case "inprocess":
              orderHeader = orderHeader.Where(u => u.OrderStatus == StaticDetails.StatusInProcess);
                    break;
                case "completed":
              orderHeader = orderHeader.Where(u => u.OrderStatus == StaticDetails.StatusShipped);
                    break;
                case "approved":
                    orderHeader = orderHeader.Where(u => u.OrderStatus == StaticDetails.StatusApproved);
                    break;
                default:
                    break;
            }






            return Json(new { data = orderHeader });
        }

        #endregion
    }
}
