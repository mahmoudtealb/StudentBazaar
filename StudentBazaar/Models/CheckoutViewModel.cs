using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentBazaar.Web.Models
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
        
        [Display(Name = "Payment Method")]
        [Required(ErrorMessage = "Please select a payment method")]
        public string SelectedPaymentMethod { get; set; }
        
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
    }

    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}