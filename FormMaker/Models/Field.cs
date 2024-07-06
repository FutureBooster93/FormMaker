using System.ComponentModel.DataAnnotations.Schema;

namespace FormMaker.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int dataTypeId { get; set; }
        [ForeignKey("dataTypeId")]
        public DataType dataType { get; set; }
    }
}
