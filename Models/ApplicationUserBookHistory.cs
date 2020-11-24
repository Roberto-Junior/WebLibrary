using BiblioTechA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioTechA.Models
{
    public class ApplicationUserBookHistory
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int BookReservationHistoryId { get; set; }
        public BookReservationHistory BookReservationHistory { get; set; }
    }
}
