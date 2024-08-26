namespace Bangazon.Models;

public class Order
{
    public int Id { get; set;}
    public int BuyerId { get; set; }
    public User Buyer { get; set; }
    public int? PaymentTypeId { get; set; }
    public DateTime? OrderDate { get; set; }
    public bool Closed { get; set; }
    public string? Address { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
    public decimal? Total
    {
        get
        {
            decimal total = 0;
            if(Products.Count > 0)
            {
                foreach(var product in Products)
                {
                    total += product.Price;
                }
                return total;
            }
            return 0;
        }
    }
}