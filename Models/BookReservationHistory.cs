using System;
using System.ComponentModel.DataAnnotations;

namespace BiblioTechA.Models
{
    public class BookReservationHistory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Autor")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Quem reservou")]
        public string WhoReserved { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Liberou reserva")]
        public string WhoReleased { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Recebeu devolução")]
        public string WhoReceivedReturn { get; set; }

        [Required]
        [Display(Name = "Retirada")]
        [DataType(DataType.DateTime)]
        public DateTime ReservationDateRelease { get; set; }

        [Required]
        [Display(Name = "Devolução")]
        [DataType(DataType.DateTime)]
        public DateTime ReservationDateReturn { get; set; }

    }
}
