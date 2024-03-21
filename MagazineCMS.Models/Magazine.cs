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
    public class Magazine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Closure Date")]
        public DateTime EndDate { get; set; }

        public int FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        [ValidateNever]
        public Faculty Faculty { get; set; }

        public int SemesterId { get; set; }
        [ForeignKey("SemesterId")]
        [ValidateNever]
        public Semester Semester { get; set; }
        public ICollection<Contribution> Contributions { get; set; }

    }
}
