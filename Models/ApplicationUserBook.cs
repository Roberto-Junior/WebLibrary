using BiblioTechA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioTechA.Models
{
    public class ApplicationUserBook
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
