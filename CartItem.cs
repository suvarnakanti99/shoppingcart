using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
	public class CartItem
	{
		[Key]
		public int CartId { get; set; }
		public int productId { get; set; }
		public int Quantity { get; set; }
		public Product? product{get; set;}
		
	}
}

