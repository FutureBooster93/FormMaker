using FormMaker.Application.Services.Interfaces;
using FormMaker.Models;
using FormMaker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FormMaker.Controllers
{
    public class FormController : Controller
    {
        private readonly AppDbContext db;
        private readonly ITableService tableService;

        public FormController(AppDbContext _db, ITableService _tableService)
        {
            db = _db;
            tableService = _tableService;
        }
        public IActionResult Index()
        {
            FormViewModel model = new FormViewModel() { forms = db.Forms.ToList() };

            return View(model);


        }
        //[HttpPost]
        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }
            else
            {
                var forms = db.Forms.Where(x => x.Name.Contains(searchTerm)).ToList();
                FormViewModel model = new FormViewModel() { forms = forms };
                return View("Index", model);
            }
        }

        public IActionResult FormCreate(FormViewModel model)
        {
            if (!db.Forms.Any(x => x.Name == model.name))
            {
                Form form = new Form() { Name = model.name };
                Random rnd = new Random();
                form.tableName = $"Tbl_{rnd.Next(0, 1001)}_AutoGen";
                db.Forms.Add(form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "این فرم وجود دارد");
                model.forms = db.Forms.ToList();
                return View("Index", model);
            }
        }
        public IActionResult Delete(int Id)
        {
            var form = db.Forms.Find(Id);
            var response = tableService.DropTable("dbo", form.tableName);
            if (response == 1)
            {
                if (form != null)
                {
                    db.Forms.Remove(form);
                    db.SaveChanges();
                }
                FormViewModel model = new FormViewModel() { forms = db.Forms.ToList() };
                model.sqlResponse = "جدول مربوط به فرم پاک شد";
                return View("Index", model);
            }
            else
            {
                if (form != null)
                {
                    db.Forms.Remove(form);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
        }
    }
}
