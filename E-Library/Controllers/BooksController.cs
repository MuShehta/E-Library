using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Library.Model;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace E_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly appContext _context;

        public BooksController(appContext dbcontext)
        {
            _context = dbcontext;
        }


        // GET: api/Books

        [HttpGet]
        //[Authorize (policy:"admin")]
        public async Task<ActionResult<List<Book>>> Getbooks(int page = 0)
        {

            if (page < 0)
                return BadRequest("page can not be less than 0");
            return await _context.books.Skip(page * 100).Take(100).ToListAsync();

        }

        // GET: api/Books/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Book>> GetBook(int id)
        //{
        //    var book = await _context.books.FindAsync(id);

        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    return book;
        //}


        //GET: api/Books/clean code
       [HttpGet("{title}")]
        public async Task<ActionResult<IEnumerable<Book>>> search(string title)
        {
            var books = await _context.books.Where(x => x.title.ToLower() == title.ToLower()).ToListAsync();

            if (books == null)
            {
                return NotFound();
            }

            return books;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostBook([FromForm]BookForImage form_book)
        {
            

            IFormFile file = form_book.file;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\");
            FileInfo fileInfo = new FileInfo(file.Name);
            string fileName = DateTime.Now.Ticks + file.FileName;

            await file.CopyToAsync(new FileStream(path + fileName, FileMode.Create));


            Book book = new Book()
            {
                id = form_book.id,
                title = form_book.title,
                author = form_book.author,
                publisher = form_book.publisher,
                price = form_book.price,
                available_amount = form_book.available_amount,
                sold_amount = form_book.sold_amount,
                buy = form_book.buy,
                image = fileName
            };
            _context.books.Add(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images\\");

            
            string old_image_path = path + "\\" + book.image;
            System.IO.File.Delete(old_image_path);

            _context.books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.books.Any(e => e.id == id);
        }
    }
}
