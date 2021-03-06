﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiblioTechA.Models
{
    public class Book
    {
        public Book()
        {
            UserBooks = new List<ApplicationUserBook>();
            BooksReservationHistory = new List<BookReservationHistory>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "O número deve estar entre 1 e 10000")]
        [Display(Name = "Páginas")]
        public int PageNumbers { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Reservado")]
        public string Reserved { get; set; }

        [Display(Name = "Retirada")]
        [DataType(DataType.DateTime)]
        public DateTime ReservationDateRelease { get; set; }

        public List<ApplicationUserBook> UserBooks { get; set; }
        public List<BookReservationHistory> BooksReservationHistory { get; set; }
        public BookGenre BookGenre { get; set; }
        public BookAuthor BookAuthor { get; set; }
        
    }
}
