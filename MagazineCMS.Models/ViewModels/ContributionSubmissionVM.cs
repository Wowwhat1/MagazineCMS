using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineCMS.Models.ViewModels
{
    public class ContributionSubmissionVM
    {
        [Required(ErrorMessage = "Please select at least one file.")]
        [Display(Name = "Files")]
        public List<IFormFile> Files { get; set; }
    }
}
