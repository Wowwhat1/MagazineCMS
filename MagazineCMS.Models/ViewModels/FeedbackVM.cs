using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineCMS.Models.ViewModels
{
    public class FeedbackVM
    {
        [Required(ErrorMessage = "Please provide feedback comment.")]
        public string Comment { get; set; }
        public int ContributionId { get; set; }
    }
}
