using FormMaker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FormMaker.ViewModels
{
    public class FormViewModel
    {
        public FormViewModel()
        {
            forms = new List<Form>();
        }
        
        public string name { get; set; }
        [ValidateNever]
        public List<Form> forms { get; set; }
        [ValidateNever]
        public string sqlResponse { get; set; }
    }
}
