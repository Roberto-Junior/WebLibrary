using System.ComponentModel.DataAnnotations;

namespace BiblioTechA.Models
{
    public class BookAuthor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Autor")]
        public string Author { get; set; }
        public int BookId { get; set; }
    }
}
