
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace BagShop.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<Bag>? Bags { get; set; }
    }
}
