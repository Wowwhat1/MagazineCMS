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
    public class Feedback
    {
        [Key] 
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime FeedbackDate { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User CoordinatorUser { get; set; }

        public int ContributionId { get; set; }
        [ForeignKey("ContributionId")]
        [ValidateNever]
        public Contribution Contribution { get; set; }
    }
}
