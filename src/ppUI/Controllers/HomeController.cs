using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ppUI.Models;
using System.Diagnostics;

namespace ppUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdvertService _advertService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBasketService _basketService;

        public HomeController(ILogger<HomeController> logger, IAdvertService advertService, IUserService userService, IWebHostEnvironment webHostEnvironment, IBasketService basketService)
        {
            _logger = logger;
            _advertService = advertService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _basketService = basketService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Hakkimizda()
        {
            return View();
        }
        [HttpGet("Home/Category/{filter}")]
        public IActionResult Category(string filter)
        {
            var tumHayvanlar = _advertService.GetAll();
            if (filter == "Evcil-Hayvanlar")
            {
                tumHayvanlar = tumHayvanlar.Where(x => x.CategoryID == 3).ToList();
            }
            else if (filter == "Buyukbas-Hayvanlar")
            {
                tumHayvanlar = tumHayvanlar.Where(x => x.CategoryID == 4).ToList();
            }
            else if (filter == "Kucukbas-Hayvanlar")
            {
                tumHayvanlar = tumHayvanlar.Where(x => x.CategoryID == 5).ToList();
            }

            return View(tumHayvanlar);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> MyAccount()
        {
            var userId = _userService.GetAll().Where(x => x.Email == HttpContext.User.Identity.Name).SingleOrDefault().ID;

            // Kullanýcýnýn ilanlarýný çek
            var userAdverts = _advertService.GetAll().Where(x => x.UserID == userId).ToList();

            return View(userAdverts);
        }
        [HttpGet]
        public IActionResult AddAdvert()
        {
            ViewBag.Categories = GetCategorySelectListItems();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAdvert(Advert model)
        {
            try
            {
                var user = _userService.GetAll().FirstOrDefault(x => x.Email == HttpContext.User.Identity.Name);

                if (user != null)
                {
                    model.CreateDate = DateTime.Now;
                    model.UserID = user.ID;

                    // Eðer image varsa iþlemleri gerçekleþtir
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        // Resmi wwwroot/images altýndaki klasöre kaydet
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                        // Resmin adýný dosya adýyla birleþtir
                        var fileName = Path.GetFileName(model.ImageFile.FileName);

                        // Eðer ayný isimde bir dosya varsa, dosya adýnýn sonuna bir GUID ekleyin
                        var counter = 1;
                        while (System.IO.File.Exists(Path.Combine(imagePath, fileName)))
                        {
                            fileName = $"{Path.GetFileNameWithoutExtension(model.ImageFile.FileName)}_{counter++}{Path.GetExtension(model.ImageFile.FileName)}";
                        }

                        var filePath = Path.Combine(imagePath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        // Modelin ImagePath özelliðini dosyanýn adýyla güncelle
                        model.ImagePath = fileName;
                    }

                    _advertService.Insert(model);
                    return RedirectToAction("MyAccount");
                }

                ViewBag.Categories = GetCategorySelectListItems();
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Categories = GetCategorySelectListItems();
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdvert(Advert model)
        {
            try
            {
                var user = _userService.GetAll().FirstOrDefault(x => x.Email == HttpContext.User.Identity.Name);

                if (user != null)
                {
                    model.UserID = user.ID;

                    // Eðer image varsa iþlemleri gerçekleþtir
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        // Resmi wwwroot/images altýndaki klasöre kaydet
                        var userImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                        // Resmin adýný kullanýcýnýn ID'si ve dosya adýyla birleþtir
                        var fileName = $"{user.ID}_{Path.GetFileName(model.ImageFile.FileName)}";
                        var filePath = Path.Combine(userImagePath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        // Modelin ImagePath özelliðini dosyanýn adýyla güncelle
                        model.ImagePath = fileName;
                    }
                    else if (string.IsNullOrEmpty(model.ImagePath))
                    {
                        // Eðer yeni bir fotoðraf eklenmemiþse ve mevcut fotoðraf da yoksa,
                        // varsayýlan bir fotoðraf veya boþ bir deðer atanabilir.
                        // Örneðin: model.ImagePath = "default.jpg"; veya model.ImagePath = "";
                    }

                    _advertService.Update(model);
                    return RedirectToAction("MyAccount");
                }

                ViewBag.Categories = GetCategorySelectListItems();
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Categories = GetCategorySelectListItems();
                return View(model);
            }
        }

        public IActionResult UpdateAdvert(int id)
        {
            var advert = _advertService.GetById(id);
            if (advert == null)
            {
                return NotFound();
            }

            ViewBag.Categories = GetCategorySelectListItems();
            return View(advert);
        }



        private SelectList GetCategorySelectListItems()
        {
            var categories = new[]
            {
                new { Value = 3, Text = "Evcil-Hayvanlar" },
                new { Value = 4, Text = "Buyukbas-Hayvanlar" },
                new { Value = 5, Text = "Kucukbas-Hayvanlar" }
                // Diðer kategoriler eklenmeli
            };

            return new SelectList(categories, "Value", "Text");
        }
        [HttpGet]
        public IActionResult DetailsAdvert(int id)
        {
            var advert = _advertService.GetById(id);
            if (advert == null)
            {
                return NotFound();
            }
            return View(advert);
        }

        [HttpGet]
        public IActionResult DeleteAdvert(int id)
        {
            var advert = _advertService.GetById(id);
            if (advert == null)
            {
                return NotFound();
            }
            return View(advert);
        }

        [HttpPost, ActionName("ConfirmDeleteAdvert")]
        public IActionResult ConfirmDeleteAdvert(int id)
        {
            var advert = _advertService.GetById(id);
            if (advert == null)
            {
                return NotFound();
            }

            // Resmi sil
            DeleteImage(advert.ImagePath);

            _advertService.Delete(advert);
            return RedirectToAction("MyAccount");
        }

        private void DeleteImage(string imagePath)
        {
            // Resmin dosya yolunu oluþtur
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", imagePath);

            // Dosya varsa sil
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        [Authorize]
        [HttpGet("/Home/AddBasket/{AdvertID}")]
        public string AddBasket(int AdvertID)
        {
            try
            {
                string returnMessage = "";
                var user = _userService.GetAll().FirstOrDefault(x => x.Email == HttpContext.User.Identity.Name);
                var mybasket = _basketService.GetAll().Where(x => x.AdvertId == AdvertID && x.UserID==user.ID);
                var allbasket = _basketService.GetAll().Where(x => x.AdvertId == AdvertID);
                if (user.ID != null && allbasket.FirstOrDefault() == null)
                {                  
                    var basketItem = new Basket
                    {
                        UserID= user.ID,    
                        AdvertId = AdvertID,
                        CreateDate = DateTime.Now
                    };
                    _basketService.Insert(basketItem);

                    returnMessage = "Ürün baþarýyla sepete eklendi.";
                }
                else if (user.ID != null && mybasket.FirstOrDefault() != null)
                {
                    returnMessage = "Ürün zaten sepette.";
                }
                else
                {
                    returnMessage = "Ürün baþka sepette.";
                }                
                return returnMessage;
            }
            catch (Exception ex)
            {
                return "Ürün sepete eklenirken bir hata oluþtu: " + ex.Message;
            }
        }
        [HttpPost]
        public int GetInfoBasket()
        {
            int mybasketCounter = 0;
            if (HttpContext.User.Identity.Name!=null)
            {
                var user = _userService.GetAll().FirstOrDefault(x => x.Email == HttpContext.User.Identity.Name);
                mybasketCounter = _basketService.GetAll().Where(x=>x.UserID == user.ID).ToList().Count();
            }
            return mybasketCounter;
        }
    }
}

