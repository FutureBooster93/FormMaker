using Aspose.Cells;
using FormMaker.Application.Services.Implementations;
using FormMaker.Application.Services.Interfaces;
using FormMaker.Models;
using FormMaker.Utilities;
using FormMaker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace FormMaker.Controllers
{
    public class FormFieldController : Controller
    {
        private readonly AppDbContext db;
        private readonly ITableService tableService;
        private readonly IReportService reportService;

        public FormFieldController(AppDbContext _db, ITableService _tableService, IReportService _reportService)
        {
            db = _db;
            tableService = _tableService;
            reportService = _reportService;
        }
        public IActionResult Index(int Id)
        {

            var formFields = db.FormFields.Where(f => f.formId == Id).Include(x => x.form).Include(a => a.field).ThenInclude(b => b.dataType).ToList();
            var fields = db.Fields.ToList();
            FormFieldViewModel model = new FormFieldViewModel() { formFields = formFields, fields = fields, formId = Id };
            return View(model);
        }
        public IActionResult FormFieldCreate(FormFieldViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.FormFields.Any(x => x.fieldId == model.fieldId && x.formId == model.formId))
                {
                    ModelState.AddModelError("formFieldErr", "این فیلد وجود دارد");
                    model.fields = db.Fields.ToList();
                    model.formFields = db.FormFields.Where(f => f.formId == model.formId).Include(x => x.form).Include(a => a.field).ThenInclude(b => b.dataType).ToList();
                    return View("Index", model);
                }
                else
                {
                    FormField formField = new FormField() { formId = model.formId, fieldId = model.fieldId };
                    db.FormFields.Add(formField);
                    db.SaveChanges();
                    var singleField = db.FormFields.Where(c => c.formId == formField.formId).Include(x => x.form).Include(a => a.field).ThenInclude(b => b.dataType).FirstOrDefault(z => z.Id == formField.Id);
                    if (singleField != null)
                    {
                        var typeName = singleField.field.dataType.Name;
                        var columnDefination = singleField.field.Name;
                        switch (typeName.ToLower())
                        {
                            case "int":
                                columnDefination += " " + SD.sqlType_Int;
                                break;
                            case "string":
                                columnDefination += " " + SD.sqlType_Nvarchar;
                                break;
                            case "datetime":
                                columnDefination += " " + SD.sqlType_DateTime;
                                break;
                            case "bool":
                                columnDefination += " " + SD.sqlType_Bool;
                                break;
                            default:
                                break;
                        }
                        var tableName = formField.form.tableName;
                        FormFieldViewModel viewModel = new FormFieldViewModel();
                        viewModel.formFields = db.FormFields.Where(f => f.formId == singleField.formId).Include(x => x.form).Include(a => a.field).ThenInclude(b => b.dataType).ToList();
                        viewModel.fields = db.Fields.ToList();
                        viewModel.formId = singleField.formId;
                        int response = tableService.CheckCreateOrAlterTable("dbo", tableName, columnDefination);
                        switch (response)
                        {
                            case 1:
                                viewModel.SqlMessage = $"Table {tableName} was successfully created with column {columnDefination}";
                                break;
                            case 2:
                                viewModel.SqlMessage = $"column {columnDefination} was added to table {tableName}";
                                break;
                            case 3:
                                viewModel.SqlMessage = $"table {tableName} already has column {columnDefination}";
                                break;

                            default:
                                break;
                        }

                        return View("Index", viewModel);
                    }
                    else
                    {
                        ModelState.AddModelError("err", "این فیلد وجود ندارد");
                        return View("Index", model);
                    }

                }
            }
            else
            {
                ModelState.AddModelError("err", "خطا در ثبت فیلد");
                model.fields = db.Fields.ToList();
                model.formFields = db.FormFields.Where(f => f.formId == model.formId).Include(x => x.form).Include(a => a.field).ThenInclude(b => b.dataType).ToList();
                return View("Index", model);
            }
        }
        public IActionResult Delete(int Id)
        {
            var formField = db.FormFields.Include(x => x.form).Include(b => b.field).ThenInclude(a => a.dataType).FirstOrDefault(d => d.Id == Id);
            //var formFieldCount= db.FormFields.Where(f => f.formId == formField.formId).ToList().Count;
            var columnName = formField.field.Name;
            var tableName = formField.form.tableName;

            FormFieldViewModel model = new FormFieldViewModel();
            int response = tableService.CheckAndDropColumn("dbo", tableName, columnName);
            switch (response)
            {
                case 1:
                    model.SqlMessage = $"Table {tableName} was droped";
                    var field = db.FormFields.Find(Id);
                    db.FormFields.Remove(field);
                    db.SaveChanges();
                    break;
                case 2:
                    model.SqlMessage = $"{columnName} deleted from {tableName} table successfully";
                    var field2 = db.FormFields.Find(Id);
                    db.FormFields.Remove(field2);
                    db.SaveChanges();
                    break;
                case 3:
                    model.SqlMessage = "column doesn't exist";
                    break;
                case 4:
                    model.SqlMessage = "table doesn't exist";
                    break;

                default:
                    break;
            }

            model.formFields = db.FormFields.Where(f => f.formId == formField.formId).Include(x => x.form).Include(a => a.field).ThenInclude(b => b.dataType).ToList();
            model.fields = db.Fields.ToList();
            model.formId = formField.formId;
            return View("Index", model);
        }
        public IActionResult Preview(int Id)
        {
            var formFields = db.FormFields.Where(x => x.formId == Id).Include(x => x.form).Include(x => x.field).ThenInclude(a => a.dataType).ToList();
            if (formFields.Count > 0)
            {
                FormFieldViewModel model = new FormFieldViewModel() { formFields = formFields };
                model.formId = Id;
                return View(model);

            }
            else
            {
                FormFieldViewModel model = new FormFieldViewModel() { formFields = formFields, SqlMessage = "اطلاعاتی در این جدول نیست" };
                model.formId = Id;
                model.fields = db.Fields.ToList();
                return View("Index", model);
            }
        }
        public IActionResult SubmitForm(int formId, List<string> values)
        {
            if (values != null && values.Count > 0)
            {

                string vals = "";
                string fields = "";
                var form = db.Forms.FirstOrDefault(x => x.Id == formId);
                if (form == null)
                {
                    return NotFound();
                }

                var formFields = db.FormFields.Where(x => x.formId == formId).Include(b => b.form).Include(x => x.field).ThenInclude(a => a.dataType).ToList();

                for (int i = 0; i < values.Count; i++)
                {
                    if (i == values.Count - 1)
                    {
                        vals += "N'" + values[i] + "'";
                    }
                    else
                    {
                        vals += "N'" + values[i] + "', ";

                    }
                }
                for (int i = 0; i < formFields.Count; i++)
                {
                    if (i == formFields.Count - 1)
                    {
                        fields += formFields[i].field.Name;
                    }
                    else
                    {
                        fields += formFields[i].field.Name + ", ";

                    }
                }
                var response = tableService.InsertToTableDynamic(form.tableName, fields, vals);
                var message = response == 1 ? "Data inserted successfully" : "Insertion failed";
                FormFieldViewModel model = new FormFieldViewModel() { formFields = formFields, formId = formId, SqlMessage = message };
                return View("Preview", model);
            }
            else
            {
                var formFields = db.FormFields.Where(x => x.formId == formId).ToList();
                FormFieldViewModel model = new FormFieldViewModel() { formFields = formFields, formId = formId, SqlMessage = "تمام فیلدها اجباریست" };
                return View("Preview", model);
            }
        }
        public IActionResult GenerateExcel(int Id)
        {
            var form = db.Forms.FirstOrDefault(x => x.Id == Id);
            var formFields = db.FormFields.Where(x => x.formId == Id).Include(x => x.form).Include(x => x.field).ThenInclude(a => a.dataType).ToList();
            if (formFields.Count > 0)
            {
                DataTable dataTable = reportService.GetDynamicTableData(form.tableName);

                Workbook workbook = new Workbook();
                Worksheet worksheet = workbook.Worksheets[0];

                worksheet.Cells.Merge(0, 0, 1, dataTable.Columns.Count);
                worksheet.Cells[0, 0].PutValue(form.tableName);

                Style style = workbook.CreateStyle();
                style.HorizontalAlignment = TextAlignmentType.Center;
                style.VerticalAlignment = TextAlignmentType.Center;
                style.Font.IsBold = true;
                worksheet.Cells[0, 0].SetStyle(style);

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    worksheet.Cells[1, i].Value = dataTable.Columns[i].ColumnName;
                }

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if (dataTable.Columns[j].ColumnName.ToLower() == "date")
                        {
                            worksheet.Cells[i + 2, j].Value = Convert.ToDateTime(dataTable.Rows[i][j]).ToString();
                        }
                        else
                        {
                            worksheet.Cells[i + 2, j].Value = dataTable.Rows[i][j];
                        }
                    }
                }

                MemoryStream stream = new MemoryStream();

                workbook.Save(stream, SaveFormat.Xlsx);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{form.tableName}.xlsx");
            }
            else
            {
                
                FormFieldViewModel model = new FormFieldViewModel() { formFields = formFields, SqlMessage = "اطلاعاتی در این جدول نیست" };
                model.formId = Id;
                return View("Index", model);
            }

        }
    }
}
