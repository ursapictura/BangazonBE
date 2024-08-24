namespace Bangazon.Models;

public class Order
{
    public int Id { get; set;}
    public int UserId { get; set; }
    public User User { get; set; }
    public string? PaymentTypeId { get; set; }
    public DateTime? OrderDate { get; set; }
    public bool Closed { get; set; }
    public string? Address { get; set; }
    public List<Product> Products { get; set; } = [];
    public decimal? Total
    {
        get
        {
            decimal total = 0;
            if(Products.Count > 0)
            {
                foreach(var products in Products)
                {
                    total += products.Price
                }
                return total;
            }
            return 0;
        }
    }
}