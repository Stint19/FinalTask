using System.ComponentModel.DataAnnotations;

namespace FinalTask.Domain.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
    }
}
