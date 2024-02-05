using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ppUI.Controllers
{
    public class BasketController : Controller
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IAdvertService _advertService;
        private readonly IUserService _userService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public BasketController(ILogger<BasketController> logger, IAdvertService advertService, IUserService userService, IBasketService basketService, IOrderService orderService)
        {
            _logger = logger;
            _advertService = advertService;
            _userService = userService;
            _basketService = basketService;
            _orderService = orderService;
        }

        [Authorize]
        public IActionResult ViewBasket()
        {
            try
            {
                var user = _userService.GetAll().FirstOrDefault(x => x.Email == HttpContext.User.Identity.Name);

                if (user != null)
                {
                    var query = from userBaskets in _basketService.GetAll().Where(x => x.UserID == user.ID)
                                join advert in _advertService.GetAll().ToList() on userBaskets.AdvertId equals advert.ID
                                select new BasketItemViewModel
                                {
                                    AdvertId = advert.ID,
                                    ImagePath = advert.ImagePath,
                                    AdvertCreateDate = advert.CreateDate,
                                    Description = advert.Description,
                                    Price = advert.Price,
                                    AnimalType = advert.AnimalType,
                                    CategoryId = advert.CategoryID,
                                    AnimalName = advert.AnimalName,
                                    CreateDate = advert.CreateDate,
                                    BasketId = userBaskets.ID
                                };
                    
                    return View(query.ToList());
                }

                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("MyAccount");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sepetinizdeki ürünleri görüntülerken bir hata oluştu: " + ex.Message;
                return RedirectToAction("MyAccount");
            }
        }
        [Authorize]

        [HttpPost]
        public IActionResult ConfirmOrder(Order order)
        {
            try
            {
                var user = _userService.GetAll().FirstOrDefault(x => x.Email == HttpContext.User.Identity.Name);
                order.UserID = user.ID;
                order.BasketOrderID = _basketService.GetAll().LastOrDefault(x => x.UserID == user.ID)?.ID ?? 0;
                var orders = from userBaskets in _basketService.GetAll().Where(x => x.UserID == user.ID)
                             join advert in _advertService.GetAll().ToList() on userBaskets.AdvertId equals advert.ID
                             select new BasketItemViewModel
                             {
                                 AdvertId = advert.ID,
                                 ImagePath = advert.ImagePath,
                                 AdvertCreateDate = advert.CreateDate,
                                 Description = advert.Description,
                                 Price = advert.Price,
                                 AnimalType = advert.AnimalType,
                                 CategoryId = advert.CategoryID,
                                 AnimalName = advert.AnimalName,
                                 CreateDate = advert.CreateDate,
                                 BasketId = userBaskets.ID
                             };
                foreach (var item in orders.ToList())
                {
                    order.AdvertID = item.AdvertId;
                    order.ID = 0;
                    _orderService.Insert(order);
                    _basketService.Delete(_basketService.GetAll().Where(x=>x.ID==item.BasketId).FirstOrDefault());
                }
                TempData["Message"] = "Siparişiniz alındı. ";



                return RedirectToAction("ViewBasket", TempData["Message"]);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sipariş onaylanırken bir hata oluştu: " + ex.Message;
                return RedirectToAction("ViewBasket");
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult ConfirmOrder()
        {
            return ViewBasket();
            //try
            //{
            //    var user = _userService.GetAll().FirstOrDefault(x => x.Email == HttpContext.User.Identity.Name);

            //    order.UserID = user.ID;
            //    order.BasketOrderID = _basketService.GetAll().FirstOrDefault(x => x.UserID == user.ID)?.ID ?? 0;

            //    // Perform any additional order processing logic here

            //    return View("OrderConfirmation", order);
            //}
            //catch (Exception ex)
            //{
            //    TempData["ErrorMessage"] = "Sipariş onaylanırken bir hata oluştu: " + ex.Message;
            //    return RedirectToAction("ViewBasket");
            //}
        }

        [HttpPost]
        [Authorize]
        public IActionResult RemoveFromBasket(int basketId)
        {
            try
            {
                var user = _userService.GetAll().FirstOrDefault(x => x.Email == HttpContext.User.Identity.Name);
                var userBasket = _basketService.GetAll().FirstOrDefault(x => x.ID == basketId && x.UserID == user.ID);

                if (userBasket != null)
                {
                    _basketService.Delete(userBasket);
                    TempData["SuccessMessage"] = "Ürün sepetten çıkarıldı.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Ürün sepetten çıkarılırken bir hata oluştu.";
                }

                return RedirectToAction("ViewBasket");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ürün sepetten çıkarılırken bir hata oluştu: " + ex.Message;
                return RedirectToAction("ViewBasket");
            }
        }
    }
}
