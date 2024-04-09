using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineCMS.Utility
{
    public static class SD
    {
        public const string Role_Student = "Student";
        public const string Role_Coordinator = "Coordinator";
        public const string Role_Manager = "Manager";
        public const string Role_Admin = "Admin";

        public const string Default_Avatar = "/image/avatar/default-avatar.png";

        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_Rejected = "Rejected";
        public const string Status_Submitted = "Submitted";
        public const string Status_Public = "Public";

        public const string Document_Type_Word = "Word";
        public const string Document_Type_Image = "Image";
        
        // a student submits 1 contribution
        public const string Noti_Type_SubmitSingle = "Submit";
        // a student submits n contributions
        public const string Noti_Type_SubmitMultipleTime = "SubmitMultipleTime";
       
        public const string Noti_Type_Approval = "Approved";
        public const string Noti_Type_Rejection = "Rejected";
        public const string Noti_Type_Comment = "Comment";
        public const string Noti_Type_Feedback = "Feedback";
        public const string Noti_Type_ContributionReminder = "ContributionReminder";
    }
}
