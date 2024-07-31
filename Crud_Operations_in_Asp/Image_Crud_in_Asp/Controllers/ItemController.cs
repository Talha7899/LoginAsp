using Image_Crud_in_Asp.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Image_Crud_in_Asp.Controllers
{
    public class ItemController : Controller
    {
        //private readonly ImgCrudContext db;

        //public ItemController(ImgCrudContext _db)
        //{
        //    this.db = _db;
        //}

        //public IActionResult Index()
        //{
        //    var itemData = db.Items.Include(x => x.Cat);
        //    return View(itemData.ToList());
        //}

        //[HttpGet]

        //public IActionResult Create()
        //{
        //    ViewBag.CatId = new SelectList(db.Categories,"Catid","CatName");
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Item item, IFormFile file)
        //{
        //    string imageName = DateTime.Now.ToString("yymmddhhmmss");
        //    imageName += Path.GetFileName(file.FileName);
        //    var imagePath = Path.Combine(HttpContext.Request.PathBase.Value,"wwwroot/uploads");
        //    var imageValue = Path.Combine(imagePath,imageName);

        //    using(var stream = new FileStream(imageValue,FileMode.Create))
        //    {
        //        file.CopyTo(stream);
        //    }

        //    var dbimage = Path.Combine("/uploads", imageName);
        //    item.Image = dbimage;
        //    db.Items.Add(item);
        //    db.SaveChanges();
        //    ViewBag.CatId = new SelectList(db.Categories, "Catid", "CatName");
        //    return RedirectToAction("Index");
        //}
    }
}
