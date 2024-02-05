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

            // Kullan�c�n�n ilanlar�n� �ek
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

                    // E�er image varsa i�lemleri ger�ekle�tir
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        // Resmi wwwroot/images alt�ndaki klas�re kaydet
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                        // Resmin ad�n� dosya ad�yla birle�tir
                        var fileName = Path.GetFileName(model.ImageFile.FileName);

                        // E�er ayn� isimde bir dosya varsa, dosya ad�n�n sonuna bir GUID ekleyin
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

                        // Modelin ImagePath �zelli�ini dosyan�n ad�yla g�ncelle
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

                    // E�er image varsa i�lemleri ger�ekle�tir
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        // Resmi wwwroot/images alt�ndaki klas�re kaydet
                        var userImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                        // Resmin ad�n� kullan�c�n�n ID'si ve dosya ad�yla birle�tir
                        var fileName = $"{user.ID}_{Path.GetFileName(model.ImageFile.FileName)}";
                        var filePath = Path.Combine(userImagePath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        // Modelin ImagePath �zelli�ini dosyan�n ad�yla g�ncelle
                        model.ImagePath = fileName;
                    }
                    else if (string.IsNullOrEmpty(model.ImagePath))
                    {
                        // E�er yeni bir foto�raf eklenmemi�se ve mevcut foto�raf da yoksa,
                        // varsay�lan bir foto�raf veya bo� bir de�er atanabilir.
                        // �rne�in: model.ImagePath = "default.jpg"; veya model.ImagePath = "";
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
                // Di�er kategoriler eklenmeli
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
            // Resmin dosya yolunu olu�tur
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

                    returnMessage = "�r�n ba�ar�yla sepete eklendi.";
                }
                else if (user.ID != null && mybasket.FirstOrDefault() != null)
                {
                    returnMessage = "�r�n zaten sepette.";
                }
                else
                {
                    returnMessage = "�r�n ba�ka sepette.";
                }                
                return returnMessage;
            }
            catch (Exception ex)
            {
                return "�r�n sepete eklenirken bir hata olu�tu: " + ex.Message;
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

