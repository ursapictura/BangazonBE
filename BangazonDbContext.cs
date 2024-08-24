using Banagazon.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

public class BangazonDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrederItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<PaymentType> PaymentTypes { get; set; }

    public BangazonDbContext(DbContextOptions<BangazonDbContext> context) : base(context)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        modelBuilder.Entity<Users>().HasData(new User[]
        {
            new User {Id = 1, Username = "InspectorSpaceTime", FirstName = "Abed", LastName = "Nadir", Email = "anadir@greendale.edu"},
            new User {Id = 2, Username = "ConstableReggie", FirstName = "Troy", LastName = "Barnes", Email = "tbarnes@greendale.edu"},
            new User {Id = 3, Username = "shirleySandwiches", FirstName = "Shirley", LastName = "Bennet", Email = "sbennett@greendale.edu"},
            new User {Id = 4, Username = "Wingman", FirstName = "Jeff", LastName = "Winger", Email = "jwinger@greendale.edu"},
            new User {Id = 5, Username = "Britta", FirstName = "Britta", LastName = "Perry", Email = "bperry@greendale.edu"},
            new User {Id = 6, Username = "Milady", FirstName = "Annie", LastName = "Edison", Email = "aedison@greendale.edu"},
            new User {Id = 7, Username = "DeanLightful", FirstName = "Craig", LastName = "Pelton", Email = "dean@greendale.edu"},
            new User {Id = 8, Username = "StarBurns", FirstName = "Alex", LastName = "Osbourne", Email = "aosbourne@greendale.edu"},
            new User {Id = 9, Username = "Kevin", FirstName = "Ben", LastName = "Chang", Email = "kevin@greendale.edu"},
        })

        modelBuilder.Entity<Categories>().HasData(new Category[]
        {
            new Category {Id = 1, Name = "Paintball"},
            new Category {Id = 2, Name = "School Supplies"},
            new Category {Id = 3, Name = "Costumes"},
            new Category {Id = 4, Name = "Black Market"}
        })

        modelBuilder.Entity<PaymentTypes>().HasData(new PaymentType[]
        {
            new PaymentType {Id = 1, Name = "Card"},
            new PaymentType {Id = 2, Name = "Paypal"},
            new PaymentType {Id = 3, Name = "ApplePay"},
            new PaymentType {Id = 4, Name = "GooglePay"},
            new PaymentType {Id = 5, Name = "I.O.U."},
        })

        modelBuilder.Entity<Products>().HasData(new Product[]
        {
            new Product {Id = 1, Name = "Red Paintballs x 50", SellerId = 8, Quantity = 20, Price = 20.00, DatePosted = new DateTime(2013, 09, 23), Description = "50 red paintballs. High quality rounds quaranteed to explode on contact.", CategoryId = 1, Image = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fsc01.alicdn.com%2Fkf%2FHTB1t.dojr_I8KJjy1Xaq6zsxpXaz%2F230016343%2FHTB1t.dojr_I8KJjy1Xaq6zsxpXaz.jpg&f=1&nofb=1&ipt=bae9eed89237af046bf8fc55a031b15e180a44a5e85cdbded158ec1f1157e017&ipo=images"},
            new Product {Id = 2, Name = "Silver Paintballs x 50", SellerId = 4, Quantity = 10, Price = 40.00, DatePosted = new DateTime(2013, 09, 24), Description = "50 silver paintballs. Rare find.", CategoryId = 1, Image = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.discountpaintball.com%2Fcdn-cgi%2Fimage%2Fwidth%253D600%252Cquality%253D100%2Fassets%2Fimages%2F900-270-03611-1.jpg&f=1&nofb=1&ipt=5236d4f3c11f23bd6bdeada50f8d01cb3e7895dce7fcdf4be2f94872590f36c1&ipo=images"},
            new Product {Id = 3, Name = "Villanous Goatee", SellerId = 2, Quantity = 7, Price = 5.00, DatePosted = new DateTime(2012, 03, 16), Description = "Goatees for dark timelines. Made from high quality felt.", CategoryId = 3, Image = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.pinimg.com%2F736x%2F94%2Fd9%2Fa5%2F94d9a54617e69c7fc968373236573d00--timeline-tape.jpg&f=1&nofb=1&ipt=c81763d68d9d5d8fb91354adbc059652caebe9f105935212167c7af72f3ae05c&ipo=images"},
            new Product {Id = 4, Name = "Test Answers for Ladders", SellerId = 9, Quantity = 4, Price = 25.00, DatePosted = new DateTime(2011, 08, 16), Description = "Answers to upcoming Ladders midterm.", CategoryId = 4, Image = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fcdn-0.studybreaks.com%2Fwp-content%2Fuploads%2F2017%2F07%2FTest-Bubble-Sheet-1024x682.jpg&f=1&nofb=1&ipt=3d37374e4800ba9dc3128d3f3fdf684e9bb6d9d44fb0de5a3f0326d55d94f13e&ipo=images"},
            new Product {Id = 5, Name = "Ballgown size 8", SellerId = 7, Quantity = 1, Price = 50.00, DatePost = new DateTime(2009, 11, 17), Description = "Sequined ballgown in shimmering magenta, size 8.", CategoryId = 3, Image = "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2Fwww.heyuguys.com%2Fimages%2F2012%2F09%2FCommunity-Fabulous-Dean.jpg&f=1&nofb=1&ipt=e7c19e8d51e20a333b685ac09292f375c392c74887ef357c7fda0fa57750983e&ipo=images"},
            new Product {Id = 6, Name = "Purple gel pen", SellerId = 6, Quantity = 12, Price = 2.00, DatePosted = new DateTime(2010, 10, 14), Description = "Restractable purple gel pen with fine point tip.", CategoryId = 2, Image = "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fwww.theknowledgetree.com%2Fprodimages%2F175684-DEFAULT-l.jpg&f=1&nofb=1&ipt=386507ad54b25be58a9b3644397e91cab2fd976025805b45625776df3f3d09d4&ipo=images"}
        })

        modelBuilder.Entity<Orders>().HasData(new Order[]
        {
            new Order {Id = 1, BuyerId = 2, PaymentTypeId = 2, Closed = true, Address = "Apt 303, Greendale, Colorado"},
            new Order {Id = 2, BuyerId = 8, PaymentTypeId = 5, Closed = false, Address = "Abandoned Horse Stables, Greendale Community College, Greendale, Colorado"},
            new Order {Id = 3, BuyerId = 9, PaymentTypeId = 1, Closed = true, Address = "Inside the Air Vents, Greendale Community College, Greendale, Colorado"},
            new Order {Id = 4, BuyerId = 1, PaymentTypeId = 3, Closed = false, Address = "Apt 303, Greendale, Colorado"}
        })
    }
}