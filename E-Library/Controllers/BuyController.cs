using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Library.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace E_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyController : ControllerBase
    {
        private readonly appContext _context;

        public BuyController(appContext context)
        {
            _context = context;
        }


        // GET: api/Buy/5
        [Authorize (policy: "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Buy>> GetBuy(int id)
        {
            var buy = _context.buy.Include(b => b.book).Include(b => b.user).SingleOrDefault(b => b.id == id);
            

            if (buy == null)
            {
                return NotFound();
            }

            return buy;
        }

        // POST: api/Buy
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(policy: "user")]
        [HttpPost]
        public async Task<ActionResult> PostBuy(IBuy buy)
        {

            var _book = _context.books.Find(buy.book);
            if (_book == null)
                return NotFound("Book Not Found");
            if (_book.available_amount == 0)
                return BadRequest("Out of stock");


            var id = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "id").Select(c => c.Value).FirstOrDefault();
            
            var _user = _context.users.Find(id);
            if (_user == null)
                return NotFound("User Not Found");

            

            Buy _buy = new Buy()
            {
                book = _book,
                user = _user,
                date = buy.date
            };

            _context.buy.Add(_buy);
            _book.available_amount = _book.available_amount - 1;

            _context.Update(_book);
            await _context.SaveChangesAsync();
            return NoContent();
            
        }

        // DELETE: api/Buy/5
        [Authorize(policy: "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuy(int id)
        {
            var buy = await _context.buy.FindAsync(id);
            if (buy == null)
            {
                return NotFound();
            }

            _context.buy.Remove(buy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BuyExists(int id)
        {
            return _context.buy.Any(e => e.id == id);
        }
    }
}
