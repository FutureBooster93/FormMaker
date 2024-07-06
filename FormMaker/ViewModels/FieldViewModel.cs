using FormMaker.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FormMaker.ViewModels
{
    public class FieldViewModel
    {
        public FieldViewModel()
        {
            Fields=new List<Field>();
        }
        [Required(ErrorMessage ="پر کردن این فیلد اجباریست")]
        public string Name { get; set; }
        [Required(ErrorMessage ="پر کردن این فیلد اجباریست")]
        public string Description { get; set; }
        [Required(ErrorMessage ="پر کردن این فیلد اجباریست")]
        public int dataTypeId { get; set; }

        [ValidateNever]
        public List<Field> Fields { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> dataTypes { get; set; }
        [ValidateNever]
        public string message { get; set; }
    }
}
