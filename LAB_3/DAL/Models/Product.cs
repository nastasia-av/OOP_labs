using System.ComponentModel.DataAnnotations;

namespace LAB_3.Models
{
    public class Product
    {
        [Key]
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;
    }


}
