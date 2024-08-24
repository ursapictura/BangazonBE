namespace Bangazon.Models;

public class User
{
    public string FirebaseKey { get; set; }
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<Product> Products { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
}