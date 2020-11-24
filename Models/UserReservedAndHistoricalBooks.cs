using System.Collections.Generic;

namespace BiblioTechA.Models
{
    public class UserReservedAndHistoricalBooks
    {
        public Book actualReservedBook;
        public IEnumerable<BookReservationHistory> historicalReservedBook;

        public UserReservedAndHistoricalBooks(Book actualReservedBook, IEnumerable<BookReservationHistory> historicalReservedBook)
        {
            this.actualReservedBook = actualReservedBook;
            this.historicalReservedBook = historicalReservedBook;
        }
    }
}
