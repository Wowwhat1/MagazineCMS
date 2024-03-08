using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineCMS.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(100)]
        public string Lastname { get; set; }

        [MaxLength(255)] // Adjust the maximum length as needed
        public string AvatarUrl { get; set; } // Add the AvatarUrl property

        // Foreign keys
        public int Role_Id { get; set; }
        public int Faculty_Id { get; set; }

        // Navigation properties
        public Role Role { get; set; }
        public Faculty Faculty { get; set; }
    }
}
