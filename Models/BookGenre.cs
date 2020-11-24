using System.ComponentModel.DataAnnotations;

namespace BiblioTechA.Models
{
    public class BookGenre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Display(Name = "Gênero")]
        public string Genre { get; set; }
        public int BookId { get; set; }
    }
}
