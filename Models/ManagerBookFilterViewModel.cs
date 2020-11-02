using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BiblioTechA.Models
{
    public class ManagerBookFilterViewModel
    {
        public List<BookReservationHistory> Books { get; set; }
        public SelectList WhoReserved { get; set; }
        public SelectList WhoReleased { get; set; }
        public SelectList WhoReceivedReturn { get; set; }
        public SelectList ReserveDay { get; set; }
        public SelectList ReserveMonth { get; set; }
        public SelectList ReserveYear { get; set; }
        public string BookWhoReserved { get; set; }
        public string BookWhoReleased { get; set; }
        public string BookWhoReceivedReturn { get; set; }
        public string BookReserveDay { get; set; }
        public string BookReserveMonth { get; set; }
        public string BookReserveYear { get; set; }
        public string SearchString { get; set; }
    }
}
