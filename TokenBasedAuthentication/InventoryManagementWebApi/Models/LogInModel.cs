using System.ComponentModel.DataAnnotations;

namespace InventoryManagementWebApi.Models
{
    public class LoginModel
    {
        [Required, MinLength(3), MaxLength(15)]
        public string Username { get; set; }

        [Required, DataType(DataType.Password), MinLength(6), MaxLength(20)]
        public string Password { get; set; }
    }
}
