namespace Bangazon.Models;

public class Product
{
    public int Id { get; set;}
    public int UserId { get; set;}
    public User User { get; set;}
    public string Name { get; set;}
    public int Quatity { get; set;}
    public decimal Price { get; set;}
    public datetime DatePosted { get; set;}
    public string Description { get; set;}
    public string Image { get; set;}
    public int CategoryId { get; set;}
    public List<Order> Orders { get; set; } = [];
}