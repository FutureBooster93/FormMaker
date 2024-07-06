using FormMaker.Models;
using FormMaker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FormMaker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext db;

        public HomeController(ILogger<HomeController> logger, AppDbContext _db)
        {
            _logger = logger;
            db = _db;
        }

        public IActionResult Index()
        {
            FieldViewModel model = new FieldViewModel()
            {
                dataTypes = db.DataTypes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            model.Fields = db.Fields.Include(x => x.dataType).ToList();
            return View(model);
        }
        public IActionResult CreateField(FieldViewModel model)
        {
            if (ModelState.IsValid)
            {
                Field field = new Field()
                {
                    Name = model.Name,
                    Description = model.Description,
                    dataTypeId = model.dataTypeId
                };
                db.Fields.Add(field);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        public IActionResult Delete(int Id)
        {
            if (!db.FormFields.Any(x => x.fieldId == Id))
            {
                var field = db.Fields.Find(Id);
                if (field != null)
                {
                    db.Fields.Remove(field);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }else
            {
                FieldViewModel model = new FieldViewModel()
                {
                    dataTypes = db.DataTypes.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    })
                };
                model.Fields = db.Fields.Include(x => x.dataType).ToList();
                model.message = $"Field {db.Fields.FirstOrDefault(x => x.Id == Id).Name} is in use";
                return View("Index",model);
            }
        }

    }
}
