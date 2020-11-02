using System.Collections.Generic;

namespace BiblioTechA.Models
{
    public class UserReservedAndHistoricalBooks
    {
        public IEnumerable<Book> actualReservedBook;
        public IEnumerable<BookReservationHistory> historicalReservedBook;

        public UserReservedAndHistoricalBooks(IEnumerable<Book> actualReservedBook, IEnumerable<BookReservationHistory> historicalReservedBook)
        {
            this.actualReservedBook = actualReservedBook;
            this.historicalReservedBook = historicalReservedBook;
        }
    }
}
