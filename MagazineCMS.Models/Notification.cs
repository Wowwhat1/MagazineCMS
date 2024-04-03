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
    public class Notification
    {
        [Key] 
        public int Id { get; set; }
        // UserId who receives the notification
        public string RecipientUserId { get; set; }
        [ForeignKey("RecipientUserId")]
        [ValidateNever]
        public User RecipientUser { get; set; }

        // UserId of the user who trigger the notification

        public string SenderUserName { get; set; }
        public List<string> UserIds { get; set; }

        public string Content { get; set; }
        public string Type { get; set; }

        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
