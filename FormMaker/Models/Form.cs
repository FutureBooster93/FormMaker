namespace FormMaker.Models
{
    public class Form
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string tableName { get; set; }
        public List<FormField> formFields { get; set; }
    }
}
