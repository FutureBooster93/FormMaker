using FormMaker.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FormMaker.ViewModels
{
    public class FormFieldViewModel
    {
        public FormFieldViewModel()
        {
            fields=new List<Field>();
        }
        public int Id { get; set; }
        public int formId { get; set; }
        public int fieldId { get; set; }
        [ValidateNever]
        public string value { get; set; }
        [ValidateNever]
        public string SqlMessage { get; set; }
        [ValidateNever]
        public List<FormField> formFields { get; set; }
        [ValidateNever]
        public List<Field> fields { get; set; }
    }
}
