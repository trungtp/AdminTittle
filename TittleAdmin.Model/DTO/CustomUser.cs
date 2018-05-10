using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TittleAdmin.Model.DTO
{
    public class CustomUser
    {
        public CustomUser(string Firstname, string Lastname, string country, string phone, string email, string fbid, string os,
            string status, string userType, DateTime lastActiveDate, DateTime registrationDate, string addon)
        {
            FirstName = Firstname;
            LastName = Lastname;
            Country = country;
            Phone = phone;
            Email = email;
            FacebookID = fbid;
            OS = os;
            AccountStatus = status;
            UserType = userType;
            LastActiveDate = lastActiveDate.ToShortDateString();
            RegistrationDate = registrationDate.ToShortDateString();
            AddonPurchases = addon;
        }

        public CustomUser() { }

        public long id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FacebookID { get; set; }
        public string OS { get; set; }
        public string AccountStatus { get; set; }
        public string UserType { get; set; }
        public string LastActiveDate { get; set; }
        public string RegistrationDate { get; set; }
        public string AddonPurchases { get; set; }
    }
}
