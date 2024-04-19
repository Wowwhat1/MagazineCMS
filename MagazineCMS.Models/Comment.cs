using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace MagazineCMS.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [AllowNull]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } // Navigation property to access user details

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime PostedAt { get; set; }

        public bool IsAnonymous { get; set; }

        [Required]
        public int ContributionId { get; set; }

        public Contribution Contribution { get; set; }
    }
}
