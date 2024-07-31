using System.Security.Claims;

using Image_Crud_in_Asp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Image_Crud_in_Asp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ImgCrudContext db;

        public ProductController(ImgCrudContext _db)
        {
            this.db = _db;
        }

        public IActionResult Index()
        {
            var products = db.Products.Include(x => x.Cat);
            return View(products.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CatId = new SelectList(db.Categories,"Id","CatName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product prd, IFormFile file)
        {
            string imageName = DateTime.Now.ToString("yymmddhhmmss");
            imageName += Path.GetFileName(file.FileName);
            var imagePath = Path.Combine(HttpContext.Request.PathBase.Value,"wwwroot/uploads");
            var imageValue = Path.Combine(imagePath,imageName);

            using (var stream = new FileStream(imageValue, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var dbimage = Path.Combine("/uploads", imageName);
            prd.Image = dbimage;
            db.Products.Add(prd);
            db.SaveChanges();
            ViewBag.CatId = new SelectList(db.Categories, "Id", "CatName");
            return RedirectToAction("Index");
        }

        // Signup:

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(User user)
        {
            var checkExictingUser = db.Users.FirstOrDefault(x => x.Email == user.Email);
            if (checkExictingUser != null)
            {
                ViewBag.msg = "User Already Exists";
                return View();
            }

            var hasher = new PasswordHasher<string>();
            user.Password = hasher.HashPassword(user.Email, user.Password);
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            bool IsAuthenticated = false;
            string controller = "";
            ClaimsIdentity identity = null;

            var checkUser = db.Users.FirstOrDefault(u1 => u1.Email == user.Email);
            if (checkUser != null)
            {
                var hasher = new PasswordHasher<string>();
                var verifyPass = hasher.VerifyHashedPassword(user.Email, checkUser.Password, user.Password);

                if (verifyPass == PasswordVerificationResult.Success && checkUser.RoleId == 1)
                {
                    identity = new ClaimsIdentity(new[]
                    {
            new Claim(ClaimTypes.Name ,checkUser.Username),
            new Claim(ClaimTypes.Role ,"Admin"),
        }
                   , CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.Session.SetString("email", checkUser.Email);
                    HttpContext.Session.SetString("username", checkUser.Username);

                    IsAuthenticated = true;
                    controller = "Admin";
                }
                else if (verifyPass == PasswordVerificationResult.Success && checkUser.RoleId == 2)
                {
                    IsAuthenticated = true;
                    identity = new ClaimsIdentity(new[]
                   {
            new Claim(ClaimTypes.Name ,checkUser.Username),
            new Claim(ClaimTypes.Role ,"User"),
        }
                   , CookieAuthenticationDefaults.AuthenticationScheme);
                    controller = "Home";
                }
                else
                {
                    IsAuthenticated = false;

                }
                if (IsAuthenticated)
                {
                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", controller);
                }

                else
                {
                    ViewBag.msg = "Invalid Credentials";
                    return View();
                }
            }
            else
            {
                ViewBag.msg = "User not found";
                return View();
            }

        }
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
    }