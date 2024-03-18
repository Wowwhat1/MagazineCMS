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
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string DocumentUrl { get; set; }

        public int ContributionId { get; set; }
        [ForeignKey("ContributionId")]
        [ValidateNever]
        public Contribution Contribution { get; set; }
    }
}
