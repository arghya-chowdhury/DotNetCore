using System.ComponentModel.DataAnnotations;

namespace InventoryManagementWebApi.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(40)]
        public string Name { get; set; }

        [Required, DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
        public int Stock { get; set; }
    }
}
