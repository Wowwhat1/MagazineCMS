using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineCMS.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime PostedAt { get; set; }

        [Required]
        public int ContributionId { get; set; }

        public Contribution Contribution { get; set; }

        // Renamed the navigation property to avoid the naming conflict
        public Comment Parent { get; set; }

        // Renamed the foreign key property to match the navigation property
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        public ICollection<Comment> Replies { get; set; }
    }
}
