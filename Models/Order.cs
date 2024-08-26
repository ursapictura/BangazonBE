namespace Bangazon.Models;

public class Order
{
    public int Id { get; set;}
    public int BuyerId { get; set; }
    public User Buyer { get; set; }
    public int? PaymentTypeId { get; set; }
    public PaymentType? PaymentType { get; set; }
    public DateTime? OrderDate { get; set; }
    public bool Closed { get; set; }
    public string? Address { get; set; }
    public List<Product> Products { get; set; }
    public decimal? TotalPrice => (
       Products != null ? Products.Sum(p => p.Price) : null);
}