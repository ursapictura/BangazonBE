namespace Bangazon.Models;

public class Product
{
    public int Id { get; set;}
    public int SellerId { get; set;}
    public User Seller { get; set;}
    public string Name { get; set;}
    public int Quantity { get; set;}
    public decimal Price { get; set;}
    public DateTime DatePosted { get; set;}
    public string Description { get; set;}
    public string Image { get; set;}
    public int CategoryId { get; set;}
    public Category Category { get; set;}
    public List<Order> Orders { get; set; } = new List<Order>();
}