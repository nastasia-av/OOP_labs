using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LAB_3.Models
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreId { get; set; } 
        public string Name { get; set; } = string.Empty; 
        public string Address { get; set; } = string.Empty; 
    }

}
