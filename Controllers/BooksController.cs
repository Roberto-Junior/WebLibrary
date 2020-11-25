using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiblioTechA.Data;
using BiblioTechA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BiblioTechA.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IAuthorizationService _authorization;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BooksController(ApplicationDbContext context, 
            IAuthorizationService authorization, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _authorization = authorization;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string BookGenre, string BookAuthor,
                                                string searchString)
        {
            await CheckUserReservationTimeLimit();
            var allowed = await _authorization.AuthorizeAsync(User, "userpolicy");
            if (allowed.Succeeded)
                return RedirectToAction("BookList");

            IQueryable<string> genreQuery = _context.Book.Select(g => g.BookGenre.Genre);


            IQueryable<string> authorQuery = _context.Book.Select(g => g.BookAuthor.Author);

            var books = _context.Book
                        .Include(a => a.BookAuthor)
                        .Include(g => g.BookGenre)
                        .AsQueryable();
                        


            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(BookGenre))
            {
                books = books.Where(x => x.BookGenre.Genre == BookGenre);
            }
            if (!string.IsNullOrEmpty(BookAuthor))
            {
                books = books.Where(x => x.BookAuthor.Author == BookAuthor);
            }

            var bookGenreVM = new BookFilterViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Authors = new SelectList(await authorQuery.Distinct().ToListAsync()),
                Books = await books.ToListAsync()
            };

            return View(bookGenreVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                             .Include(a => a.BookAuthor)
                             .Include(g => g.BookGenre)
                             .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var bookForView = new BookAuthorGenreForView
            {
                Title = book.Title,
                Author = book.BookAuthor.Author,
                Genre = book.BookGenre.Genre,
                PageNumbers = book.PageNumbers,
                Description = book.Description
            };

            return View(bookForView);
        }

        [Authorize(Policy = "adminmanagerpolicy")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "adminmanagerpolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,Genre,PageNumbers,Description")] BookAuthorGenreForView ToAddBook)
        {         
            if (ModelState.IsValid)
            {
                Book book = new Book
                {
                    Title = ToAddBook.Title,
                    PageNumbers = ToAddBook.PageNumbers,
                    Description = ToAddBook.Description,
                    Reserved = "Free",
                    ReservationDateRelease = DateTime.Now,
                    BookAuthor = new BookAuthor { Author = ToAddBook.Author},
                    BookGenre = new BookGenre { Genre = ToAddBook.Genre }
                };
                

                IEnumerable<Book> bookNameFind = await _context.Book.ToListAsync();
                foreach (var item in bookNameFind)
                {
                    if (book.Title.ToUpper() == item.Title.ToUpper())
                        return RedirectToAction("DuplicateBook");
                }

                TempData["msg"] = "<script>alert('Livro adicionado com sucesso!');</script>";
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ToAddBook);
        }

        [Authorize(Policy = "adminmanagerpolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                             .Include(a => a.BookAuthor)
                             .Include(g => g.BookGenre)
                             .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var bookForView = new BookAuthorGenreForView
            {
                BookId = book.Id,
                Title = book.Title,
                Author = book.BookAuthor.Author,
                Genre = book.BookGenre.Genre,
                PageNumbers = book.PageNumbers,
                Description = book.Description,
            };

            return View(bookForView);
        }

        [Authorize(Policy = "adminmanagerpolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Author,Genre,PageNumbers,Description")] BookAuthorGenreForView ToAddBook)
        {
            if (id != ToAddBook.BookId) 
            {
                return NotFound();
            }

            var book = await _context.Book
                             .Include(a => a.BookAuthor)
                             .Include(g => g.BookGenre)
                             .FirstOrDefaultAsync(m => m.Id == ToAddBook.BookId);

            if (ModelState.IsValid)
            {
                try
                {
                    book.Title = ToAddBook.Title;
                    book.BookAuthor.Author = ToAddBook.Author;
                    book.BookGenre.Genre = ToAddBook.Genre;
                    book.PageNumbers = ToAddBook.PageNumbers;
                    book.Description = ToAddBook.Description;

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["msg"] = "<script>alert('Livro editado com sucesso!');</script>";
                return RedirectToAction("Index");
            }
            return View(ToAddBook);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                             .Include(a => a.BookAuthor)
                             .Include(g => g.BookGenre)
                             .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var bookForView = new BookAuthorGenreForView
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.BookAuthor.Author,
                Genre = book.BookGenre.Genre,
                PageNumbers = book.PageNumbers,
                Description = book.Description,
            };

            return View(bookForView);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if(book.Reserved == "Free")
            {
                TempData["msg"] = "<script>alert('Livro removido com sucesso!');</script>";
                _context.Book.Remove(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else 
            {
                TempData["msg"] = "<script>alert('Não é possível deletar o livro pois ele está reservado.');</script>";
                return RedirectToAction("Index");
            }
            
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }

        [Authorize(Policy = "userpolicy")]
        public async Task<IActionResult> BookList(string BookGenre, string BookAuthor,
                                                    string SearchString)
        {
            IQueryable<string> genreQuery = _context.Book.Select(g => g.BookGenre.Genre);


            IQueryable<string> authorQuery = _context.Book.Select(g => g.BookAuthor.Author);


            var books = _context.Book
                        .Include(a => a.BookAuthor).Where(x => x.Reserved == "Free")
                        .Include(g => g.BookGenre).Where(y => y.Reserved == "Free");

            if (!string.IsNullOrEmpty(SearchString))
            {
                books = books.Where(s => s.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(BookGenre))
            {
                books = books.Where(x => x.BookGenre.Genre == BookGenre);
            }
            if (!string.IsNullOrEmpty(BookAuthor))
            {
                books = books.Where(x => x.BookAuthor.Author == BookAuthor);
            }

            var bookFilterVM = new BookFilterViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Authors = new SelectList(await authorQuery.Distinct().ToListAsync()),
                Books = await books.ToListAsync()
            };

            return View(bookFilterVM);
        }

        [Authorize(Policy = "userpolicy")]
        public async Task<IActionResult> ConfirmReservation(int? id)
        {
            await CheckUserReservationTimeLimit();
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user.AskedReservation == true)
            {
                TempData["msg"] = "<script>alert('Você já reservou um livro. Cancele a reserva ou devolva o livro e poderá reservar novamente.');</script>";
                return RedirectToAction("BookList");
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [Authorize(Policy = "userpolicy")]
        [HttpPost, ActionName("ConfirmReservation")]
        public async Task<IActionResult> ConfirmTheReservation(int id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            book.ReservationDateRelease = DateTime.Now;
            book.Reserved = "Askeed";
            _context.Update(book);
            await _context.SaveChangesAsync();

            user.ReservationDate = DateTime.Now;
            user.AskedReservation = true;
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            var userbookJoin = new ApplicationUserBook
            {
                ApplicationUser = user,
                Book = book
            };
            _context.Add(userbookJoin);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Policy = "adminmanagerpolicy")]
        public async Task<IActionResult> ReleaseReservation()
        {
            await CheckUserReservationTimeLimit();
            var books = await _context.Book
                             .Include(b => b.BookAuthor)
                             .Include(b => b.UserBooks)
                             .ToListAsync();

            List<BookAuthorGenreForView> booksReserved = new List<BookAuthorGenreForView>();

            foreach (var book in books)
            {
                   
                if (book.Reserved == "Askeed")
                    booksReserved.Add( new BookAuthorGenreForView
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Author = book.BookAuthor.Author,
                        WhoReserved = book.UserBooks[0].ApplicationUser.Email
                    });
            }

            return View(booksReserved);
        }

        [HttpPost, ActionName("ReleaseReservation")]
        [Authorize(Policy = "adminmanagerpolicy")]
        public async Task<IActionResult> ConfirmReleaseReservation(int id)
        {
            await CheckUserReservationTimeLimit();

            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (book.Reserved == "Askeed")
            {
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);

                book.Reserved = "WithUser";
                book.ReservationDateRelease = DateTime.Now;
                _context.Update(book);
                await _context.SaveChangesAsync();

                var userbookJoin1 = new ApplicationUserBook
                {
                    ApplicationUser = user,
                    Book = book
                };
                _context.Add(userbookJoin1);
                await _context.SaveChangesAsync();
            }
            else if(book.Reserved == "Free")
            {
                TempData["msg"] = "<script>alert('Tempo limite da reserva acabou, será necessário que o usuário reserve novamente.');</script>";
            }

            return RedirectToAction("ReleaseReservation");
        }

        [Authorize(Policy = "adminmanagerpolicy")]
        public async Task<IActionResult> ReturnReservation()
        {
            var books = await _context.Book
                             .Include(b => b.UserBooks)
                                .ThenInclude(a => a.ApplicationUser)
                             .ToListAsync();

            List<BookAuthorGenreForView> booksReserved = new List<BookAuthorGenreForView>();

            foreach (var book in books)
            {
                if(book.Reserved != "Askeed" && book.Reserved != "Free")
                {

                    var users = book.UserBooks
                                        .Select(u => u.ApplicationUser)
                                        .ToList();

                    var userEmail = "none";
                    foreach (var user in users)
                    {
                        var userInRole = await _userManager.IsInRoleAsync(user, "Usuário");
                        if (userInRole)
                            userEmail = user.Email;
                    }
                    booksReserved.Add(new BookAuthorGenreForView
                    {
                        Id = book.Id,
                        Title = book.Title,
                        WhoReserved = userEmail,
                        ReservationDateRelease = book.ReservationDateRelease
                    });
                }
                    
            }

            return View(booksReserved);
        }

        [HttpPost, ActionName("ReturnReservation")]
        [Authorize(Policy = "adminmanagerpolicy")]
        public async Task<IActionResult> ConfirmReturnReservation(int id)
        {
            var book = await _context.Book
                                    .Include(b => b.UserBooks)
                                        .ThenInclude(u => u.ApplicationUser)
                                    .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ApplicationUser userRoleUser = null;
            ApplicationUser userRoleAdmin = null;
            foreach (var theUser in book.UserBooks)
            {
                var userInRole = await _userManager.IsInRoleAsync(theUser.ApplicationUser, "Usuário");
                if (userInRole)
                    userRoleUser = theUser.ApplicationUser;   
                else
                    userRoleAdmin = theUser.ApplicationUser;
            }
            var historicalBook = new BookReservationHistory
            {
                Title = book.Title,
                WhoReserved = userRoleUser.Email,
                WhoReleased = userRoleAdmin.Email,
                WhoReceivedReturn = user.Email,
                ReservationDateRelease = book.ReservationDateRelease,
                ReservationDateReturn = DateTime.Now,
                Book = book
            };

            _context.Add(historicalBook);
            await _context.SaveChangesAsync();

            var userbookJoin = new ApplicationUserBookHistory
            {
                ApplicationUser = userRoleUser,
                BookReservationHistory = historicalBook
            };
            _context.Add(userbookJoin);
            await _context.SaveChangesAsync();

            var adminbookJoin = new ApplicationUserBookHistory
            {
                ApplicationUser = userRoleAdmin,
                BookReservationHistory = historicalBook
            };
            _context.Add(adminbookJoin);
            await _context.SaveChangesAsync();

            if(userRoleAdmin.Email != user.Email)
            {
                var adminbookJoin2 = new ApplicationUserBookHistory
                {
                    ApplicationUser = user,
                    BookReservationHistory = historicalBook
                };
                _context.Add(adminbookJoin2);
                await _context.SaveChangesAsync();
            }
            
            user = _userManager.Users.Where(x => x.Email == userRoleUser.Email).FirstOrDefault();
            user.AskedReservation = false;
            await _userManager.UpdateAsync(user);
            user = await _userManager.GetUserAsync(User);

            book.UserBooks.Clear();
            book.Reserved = "Free";
            _context.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction("ReturnReservation");
        }

        [Authorize(Policy = "userpolicy")]
        public async Task<IActionResult> UserReservation()
        {
            await CheckUserReservationTimeLimit();
            var user = await _userManager.GetUserAsync(User);


            var books = await _context.Book
                                .Include(book => book.UserBooks)
                                    .ThenInclude(UsersBooks => UsersBooks.ApplicationUser)
                                .ToListAsync();

            var historicalbooks = await _context.BookReservationHistory
                                                .Include(b => b.Book)
                                                .Include(book => book.UsersBookHistory)
                                                    .ThenInclude(UsersBookHistory => UsersBookHistory.ApplicationUser)
                                                .ToListAsync();

            var bookReserved = books.FirstOrDefault(b => b.UserBooks
                                                .Any(u => u.ApplicationUser.Email == user.Email));

            var bookHistorical = historicalbooks.Where(x => x.UsersBookHistory
                                                 .Any(u => u.ApplicationUser == user));

            var allBooks = new UserReservedAndHistoricalBooks(bookReserved, bookHistorical);

            return View(allBooks);
        }

        [HttpPost]
        [Authorize(Policy = "userpolicy")]
        public async Task<IActionResult> UserReservation(int id)
        {
            await CheckUserReservationTimeLimit();
            var user = await _userManager.GetUserAsync(User);
            var deleteRelationship = _context.UserBooks.FirstOrDefault(x => x.ApplicationUser == user);

            var book = await _context.Book
                             .Include(book => book.UserBooks)
                                    .ThenInclude(UsersBooks => UsersBooks.ApplicationUser)
                             .FirstOrDefaultAsync(b => b.Id == id);

            if (book.Reserved == "WithUser")
            {
                TempData["msg"] = "<script>alert('Faça a devolução do livro e a reserva será cancelada automaticamente.');</script>";
            }
            else if (book.Reserved == "Askeed")
            {
                book.Reserved = "Free";
                await _context.SaveChangesAsync();

                user.AskedReservation = false;
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);

                _context.UserBooks.Remove(deleteRelationship);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserReservation");
        }

        [Authorize(Policy = "managerpolicy")]
        public async Task<IActionResult> AllBookingHistory(string BookWhoReserved, string BookWhoReleased, string BookWhoReceivedReturn,
                                                           string BookReserveMonth, string BookReserveDay, string BookReserveYear,
                                                           string SearchString)
        {
            await CheckUserReservationTimeLimit();

            IQueryable<string> WhoReservedQuery = from m in _context.BookReservationHistory
                                                  orderby m.WhoReserved
                                                  select m.WhoReserved;

            IQueryable<string> WhoReleasedQuery = from m in _context.BookReservationHistory
                                                  orderby m.WhoReleased
                                                  select m.WhoReleased;

            IQueryable<string> WhoReceivedReturnQuery = from m in _context.BookReservationHistory
                                                  orderby m.WhoReceivedReturn
                                                  select m.WhoReceivedReturn;

            IQueryable<string> ReserveMonthQuery = from m in _context.BookReservationHistory
                                                  orderby m.Book.ReservationDateRelease
                                                  select m.Book.ReservationDateRelease.Month.ToString();

            IQueryable<string> ReserveDayQuery = from m in _context.BookReservationHistory
                                                    orderby m.Book.ReservationDateRelease
                                                    select m.Book.ReservationDateRelease.Day.ToString();

            IQueryable<string> ReserveYearQuery = from m in _context.BookReservationHistory
                                                orderby m.Book.ReservationDateRelease
                                                select m.Book.ReservationDateRelease.Year.ToString();

            var books = (from m in _context.UserBooksHistory
                        select m.BookReservationHistory).Distinct();

            if (!string.IsNullOrEmpty(SearchString))
            {
                books = books.Where(s => s.Book.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(BookWhoReserved))
            {
                books = books.Where(x => x.WhoReserved == BookWhoReserved);
            }
            if (!string.IsNullOrEmpty(BookWhoReleased))
            {
                books = books.Where(x => x.WhoReleased == BookWhoReleased);
            }
            if (!string.IsNullOrEmpty(BookWhoReceivedReturn))
            {
                books = books.Where(x => x.WhoReceivedReturn == BookWhoReceivedReturn);
            }
            if (!string.IsNullOrEmpty(BookReserveMonth))
            {
                books = books.Where(x => x.ReservationDateRelease.Month.ToString() == BookReserveMonth);
            }
            if (!string.IsNullOrEmpty(BookReserveDay))
            {
                books = books.Where(x => x.ReservationDateRelease.Day.ToString() == BookReserveDay);
            }
            if (!string.IsNullOrEmpty(BookReserveYear))
            {
                books = books.Where(x => x.ReservationDateRelease.Year.ToString() == BookReserveYear);
            }

            var bookFilterVM = new ManagerBookFilterViewModel
            {
                WhoReserved = new SelectList(await WhoReservedQuery.Distinct().ToListAsync()),
                WhoReleased = new SelectList(await WhoReleasedQuery.Distinct().ToListAsync()),
                WhoReceivedReturn = new SelectList(await WhoReceivedReturnQuery.Distinct().ToListAsync()),
                ReserveMonth = new SelectList(await ReserveMonthQuery.Distinct().ToListAsync()),
                ReserveDay = new SelectList(await ReserveDayQuery.Distinct().ToListAsync()),
                ReserveYear = new SelectList(await ReserveYearQuery.Distinct().ToListAsync()),
                Books = await books.ToListAsync()
            };

            return View(bookFilterVM);
        }

        [Authorize(Policy = "adminmanagerpolicy")]
        public IActionResult ShowAllUsers()
        {
            return View(_context.Users.ToList());
        }

        public IActionResult DuplicateBook()
        {
            TempData["msg"] = "<script>alert('Livro já foi adicionado!');</script>";
            return RedirectToAction("Create");
        }

        public async Task CheckUserReservationTimeLimit()
        {
            var users = await _context.Users.ToListAsync();

            var books = await _context.UserBooks
                             .Include(b => b.ApplicationUser)
                             .Include(c => c.Book)
                             .ToListAsync();

            var booksReserved = new List<ApplicationUserBook>();
            foreach (var bookReserved in books)
            {
                if (bookReserved.Book.Reserved == "Askeed")
                {
                    booksReserved.Add(bookReserved);
                }
            }

            foreach (var user in users)
            {
                var userInRole = await _userManager.IsInRoleAsync(user, "Usuário");
                if (userInRole)
                { 
                    foreach (var bookUser in booksReserved)
                    {
                        if (bookUser.ApplicationUser.Email == user.Email)
                        {
                            var limiteTempo = DateTime.Compare(DateTime.Now, user.ReservationDate.AddMinutes(10));

                            if (limiteTempo > 0)
                            {
                                user.AskedReservation = false;
                                await _userManager.UpdateAsync(user);

                                bookUser.Book.Reserved = "Free";
                                _context.Update(bookUser);
                                await _context.SaveChangesAsync();

                                var deleteRelationship = _context.UserBooks.FirstOrDefault(x => x.ApplicationUser == user);
                                _context.UserBooks.Remove(deleteRelationship);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }

    }
}
