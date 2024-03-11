using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineCMS.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string? Firstname { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Lastname { get; set; }

        [MaxLength(255)] // Adjust the maximum length as needed
        public string? AvatarUrl { get; set; } // Add the AvatarUrl 

        public int FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        public Faculty Faculty { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}
