﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BiblioTechA.Models
{
    public class BookAuthorGenreForView
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Autor")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Gênero")]
        public string Genre { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "O número deve estar entre 1 e 10000")]
        [Display(Name = "Páginas")]
        public int PageNumbers { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Quem reservou")]
        public string WhoReserved { get; set; }

        [Display(Name = "Retirada")]
        [DataType(DataType.DateTime)]
        public DateTime ReservationDateRelease { get; set; }
    }
}
