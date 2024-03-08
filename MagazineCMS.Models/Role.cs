using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineCMS.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        // Navigation property for Users with this role
        public ICollection<User> UsersWithThisRole { get; set; }
    }
}
