namespace LAB_4.DAL.Models
{
    public class Tree
    {
        public int Id { get; set; } 
        public List<Person> Persons { get; set; } = new();
    }
}
