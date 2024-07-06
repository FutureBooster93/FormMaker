using System.ComponentModel.DataAnnotations.Schema;

namespace FormMaker.Models
{
    public class FormField
    {
        public int Id { get; set; }
        public int formId { get; set; }
        [ForeignKey("formId")]
        public Form form { get; set; }
        public int fieldId { get; set; }
        [ForeignKey("fieldId")]
        public Field field { get; set; }
    }
}
