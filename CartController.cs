using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        {
            return await _context.cartItems.Include(c => c.product).ToListAsync();
        }

        [HttpPost("add")]
        public async Task<ActionResult<CartItem>> AddToCart(int productId, int quantity)
        {
            var product = await _context.products.FindAsync(productId);
            if (product == null) return NotFound("Product not found");

            var cartItem = await _context.cartItems.FirstOrDefaultAsync(c => c.productId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem { productId = productId, Quantity = quantity };
                _context.cartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok(cartItem);
        }
        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.cartItems.FindAsync(id);
            if (cartItem == null) return NotFound();
            _context.cartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            _context.cartItems.RemoveRange(_context.cartItems);
            await _context.SaveChangesAsync();
            return Ok("Checkout successful!");
        }
    }
}

