using BiblioTechA.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiblioTechA.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            UserBooks = new List<ApplicationUserBook>();
            UsersBookHistory = new List<ApplicationUserBookHistory>();
        }
        [PersonalData]
        [Column(TypeName ="nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [PersonalData]
        public DateTime ReservationDate { get; set; }

        [PersonalData]
        public bool AskedReservation { get; set; }
        public List<ApplicationUserBookHistory> UsersBookHistory { get; set; }

        public List<ApplicationUserBook> UserBooks { get; set; }
    }
}
