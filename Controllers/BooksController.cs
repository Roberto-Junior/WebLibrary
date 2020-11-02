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

        public BooksController(ApplicationDbContext context, 
            IAuthorizationService authorization, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _authorization = authorization;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string BookGenre, string BookAuthor,
                                                string searchString)
        {
            await CheckUserReservationTimeLimit();
            var allowed = await _authorization.AuthorizeAsync(User, "userpolicy");
            if (allowed.Succeeded)
                return RedirectToAction("BookList");

            IQueryable<string> genreQuery = from m in _context.Book
                                            orderby m.Genre
                                            select m.Genre;

            IQueryable<string> authorQuery = from m in _context.Book
                                             orderby m.Author
                                             select m.Author;

            var books = from m in _context.Book
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(BookGenre))
            {
                books = books.Where(x => x.Genre == BookGenre);
            }
            if (!string.IsNullOrEmpty(BookAuthor))
            {
                books = books.Where(x => x.Author == BookAuthor);
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [Authorize(Policy = "adminmanagerpolicy")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "adminmanagerpolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Genre,PageNumbers,Description")] Book book)
        {         
            if (ModelState.IsValid)
            {
                book.Reserved = "Free";
                book.ReservationDateRelease = DateTime.Now;
                book.ReservationDateReturn = DateTime.Now;
                book.WhoReserved = "None";
                book.WhoReleased = "None";

                IEnumerable<Book> bookNameFind = await _context.Book.ToListAsync();
                foreach (var item in bookNameFind)
                {
                    if (book.Title.ToUpper() == item.Title.ToUpper())
                        return RedirectToAction("DuplicateBook");
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [Authorize(Policy = "adminmanagerpolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Genre,PageNumbers,Description,Reserved,ReservationDateRelease,ReservationDateReturn,WhoReserved,WhoReleased")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction("Index");
            }
            return View(book);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if(book.Reserved == "Free")
            {
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
            IQueryable<string> genreQuery = from m in _context.Book
                                            orderby m.Genre
                                            select m.Genre;

            IQueryable<string> authorQuery = from m in _context.Book
                                            orderby m.Author
                                            select m.Author;

            var books = from m in _context.Book
                        where m.Reserved == "Free"
                        select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                books = books.Where(s => s.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(BookGenre))
            {
                books = books.Where(x => x.Genre == BookGenre);
            }
            if (!string.IsNullOrEmpty(BookAuthor))
            {
                books = books.Where(x => x.Author == BookAuthor);
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
            var book = await _context.Book
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

            book.WhoReserved = user.Email;
            book.ReservationDateRelease = DateTime.Now;
            book.Reserved = "Askeed";
            _context.Update(book);
            await _context.SaveChangesAsync();

            user.ReservationDate = DateTime.Now;
            user.AskedReservation = true;
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index");
        }

        [Authorize(Policy = "userpolicy")]
        public async Task<IActionResult> UserReservation()
        {
            await CheckUserReservationTimeLimit();
            var user = await _userManager.GetUserAsync(User);
            var books = await _context.Book.ToListAsync();
            var historicalbooks = await _context.BookReservationHistory.ToListAsync();

            var bookReserved = books.Where(x => x.WhoReserved == user.Email);
            var bookHistorical = historicalbooks.Where(x => x.WhoReserved == user.Email);

            var allBooks = new UserReservedAndHistoricalBooks(bookReserved, bookHistorical);

            return View(allBooks);
        }

        [HttpPost]
        [Authorize(Policy = "userpolicy")]
        public async Task<IActionResult> UserReservation(int id)
        {
            await CheckUserReservationTimeLimit();
            var user = await _userManager.GetUserAsync(User);
            var book = await _context.Book.FindAsync(id);

            if(book.Reserved == "WithUser")
            {
                TempData["msg"] = "<script>alert('Faça a devolução do livro e a reserva será cancelada automaticamente.');</script>";
            }
            else if(book.Reserved == "Askeed")
            {
                book.Reserved = "Free";
                book.WhoReserved = "None";
                await _context.SaveChangesAsync();

                user.AskedReservation = false;
                await _userManager.UpdateAsync(user);
                await _signInManager.RefreshSignInAsync(user);
            }

            return RedirectToAction("UserReservation");
        }

        [Authorize(Policy = "adminmanagerpolicy")]
        public async Task<IActionResult> ReleaseReservation()
        {
            await CheckUserReservationTimeLimit();

            var books = await _context.Book.ToListAsync();
            List<Book> booksReserved = new List<Book>();

            foreach (var item in books)
            {
                if (item.Reserved == "Askeed")
                    booksReserved.Add(item);
            }

            return View(booksReserved);
        }

        [HttpPost, ActionName("ReleaseReservation")]
        [Authorize(Policy = "adminmanagerpolicy")]
        public async Task<IActionResult> ConfirmReleaseReservation(int id)
        {
            await CheckUserReservationTimeLimit();

            var book = await _context.Book
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

            if (book.Reserved == "Askeed")
            {
                book.Reserved = "WithUser";
                book.ReservationDateRelease = DateTime.Now;
                book.WhoReleased = user.Email;
                _context.Update(book);
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
            var books = await _context.Book.ToListAsync();

            var booksReserved = books.Where(x => x.Reserved != "Free" && x.Reserved != "Askeed");

            return View(booksReserved);
        }

        [HttpPost, ActionName("ReturnReservation")]
        [Authorize(Policy = "adminmanagerpolicy")]
        public async Task<IActionResult> ConfirmReturnReservation(int id)
        {
            var book = await _context.Book
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

            user = _userManager.Users.Where(x => x.Email == book.WhoReserved).FirstOrDefault();
            user.AskedReservation = false;
            await _userManager.UpdateAsync(user);
            user = await _userManager.GetUserAsync(User);

            var bookHistory = new BookReservationHistory();
            bookHistory.Title = book.Title;
            bookHistory.Author = book.Author;
            bookHistory.Description = book.Description;
            bookHistory.ReservationDateRelease = book.ReservationDateRelease;
            bookHistory.ReservationDateReturn = DateTime.Now;
            bookHistory.WhoReleased = book.WhoReleased;
            bookHistory.WhoReserved = book.WhoReserved;
            bookHistory.WhoReceivedReturn = user.Email;
            _context.Add(bookHistory);
            await _context.SaveChangesAsync();

            book.WhoReserved = "None";
            book.WhoReleased = "None";
            book.Reserved = "Free";
            _context.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction("ReturnReservation");
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
                                                  orderby m.ReservationDateRelease
                                                  select m.ReservationDateRelease.Month.ToString();

            IQueryable<string> ReserveDayQuery = from m in _context.BookReservationHistory
                                                    orderby m.ReservationDateRelease
                                                    select m.ReservationDateRelease.Day.ToString();

            IQueryable<string> ReserveYearQuery = from m in _context.BookReservationHistory
                                                orderby m.ReservationDateRelease
                                                select m.ReservationDateRelease.Year.ToString();

            var books = from m in _context.BookReservationHistory
                        select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                books = books.Where(s => s.Title.Contains(SearchString));
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
            TempData["msg"] = "<script>alert('Livro já existe');</script>";
            return RedirectToAction("Create");
        }

        public async Task CheckUserReservationTimeLimit()
        {
            var users = _context.Users.ToList();
            var books = await _context.Book.ToListAsync();
            var booksReserved = new List<Book>();
            foreach (var bookReserved in books)
            {
                if (bookReserved.Reserved == "Askeed")
                {
                    booksReserved.Add(bookReserved);
                }
            }

            foreach (var user in users)
            {
                foreach (var book in booksReserved)
                {
                    if (book.WhoReserved == user.Email)
                    {
                        var limiteTempo = DateTime.Compare(DateTime.Now, user.ReservationDate.AddMinutes(10));

                        //já passou 10 minutos de limite da reserva
                        if (limiteTempo > 0)
                        {
                            user.AskedReservation = false;
                            book.WhoReserved = "None";
                            book.Reserved = "Free";

                            //salvando mudanças no usuario
                            await _userManager.UpdateAsync(user);
                            //await _signInManager.RefreshSignInAsync(user);

                            //salvando mudanças no livro
                            _context.Update(book);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }
    }
}
