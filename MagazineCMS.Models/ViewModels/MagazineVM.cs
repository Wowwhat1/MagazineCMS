using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagazineCMS.Models.ViewModels
{
    public class MagazineVM
    {
        public Magazine Magazine { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> FacultyList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SemesterList { get; set; }
    }
}
