using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BiblioTechA.Models
{
    public class BookFilterViewModel
    {
        public List<Book> Books { get; set; }
        public SelectList Genres { get; set; }
        public SelectList Authors { get; set; }
        public string BookGenre { get; set; }
        public string BookAuthor { get; set; }
        public string SearchString { get; set; }
    }
}
