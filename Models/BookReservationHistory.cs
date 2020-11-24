using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiblioTechA.Models
{
    public class BookReservationHistory
    {
        public BookReservationHistory()
        {
            UsersBookHistory = new List<ApplicationUserBookHistory>();
        }
        public int Id { get; set; }
        public string Title { get; set; }

        [Display(Name = "Quem reservou")]
        public string WhoReserved { get; set; }

        [Display(Name = "Liberou reserva")]
        public string WhoReleased { get; set; }

        [Display(Name = "Recebeu devolução")]
        public string WhoReceivedReturn { get; set; }

        [Display(Name = "Retirada")]
        [DataType(DataType.DateTime)]
        public DateTime ReservationDateRelease { get; set; }

        [Display(Name = "Devolução")]
        [DataType(DataType.DateTime)]
        public DateTime ReservationDateReturn { get; set; }
        public Book Book { get; set; }
        public List<ApplicationUserBookHistory> UsersBookHistory { get; set; }
    }
}
