using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MagazineCMS.Models
{
    public class Contribution
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Status { get; set; }
        [Required]
        public DateTime SubmissionDate { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }

        public int MagazineId { get; set; }
        [ForeignKey("MagazineId")]
        [ValidateNever]
        public Magazine Magazine { get; set; }

        [ValidateNever]
        public List<Document> Documents { get; set; }
        public List<Feedback> Feedbacks { get; set; }
    }
}
