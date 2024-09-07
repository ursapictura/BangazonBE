using Bangazon.Models;
using Bangazon.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Http.HttpResults;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<BangazonDbContext>(builder.Configuration["BangazonDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5003")
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Check user
app.MapGet("/checkuser/{uid}", (BangazonDbContext db, string uid) =>
{
    var user = db.Users.Where(u => u.Uid == uid).ToList();

    if (uid == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(user);
});

//Get single user
app.MapGet("/api/users/{id}", (BangazonDbContext db, int id) => 
{
    User user = db.Users.SingleOrDefault(u => u.Id == id);
    if (user != null)
    {
        return Results.Ok(user);
    }
    return Results.NotFound("User Id not found");
});

//Add user
app.MapPost("/api/register", (BangazonDbContext db, User user) =>
{
    try
    {
        db.Users.Add(user);
        db.SaveChanges();
        return Results.Created($"/users/{user.Id}", user);
    }
    catch (DbUpdateException)
    {
        return Results.BadRequest("Unable to register user");
    }
});

//Delete user
app.MapDelete("/api/users/{id}", (BangazonDbContext db, int id) => 
{
    User user = db.Users.SingleOrDefault(u => u.Id == id);
    if (user == null)
    {
        return Results.NotFound("User Id not found");
    }
    db.Users.Remove(user);
    db.SaveChanges();
    return Results.NoContent();
});

//Update user
app.MapPut("/api/users/{id}", (BangazonDbContext db, int id, User user) =>
{
    User userToUpdate = db.Users.SingleOrDefault(u => u.Id == id);
    if (userToUpdate == null)
    {
        return Results.NotFound("User Id not found");
    }
    userToUpdate.Uid = user.Uid;
    userToUpdate.FirstName = user.FirstName;
    userToUpdate.LastName = user.LastName;
    userToUpdate.Email = user.Email;

    db.SaveChanges();
    return Results.NoContent();
});

//Get products
app.MapGet("/api/products", (BangazonDbContext db) =>
{
    return db.Products
        .Where(p => p.Quantity > 0)
        .Include(p => p.Seller)
        .Include(p => p.Category)
        .OrderByDescending(p => p.DatePosted)
        .ToList();
});

//Get 20 newest products
app.MapGet("/api/products/newest", (BangazonDbContext db) => 
{
    var newestProducts = db.Products
                            .Include(p => p.Category)
                            .Include(p => p.Seller)
                            .OrderByDescending(p => p.DatePosted)
                            .Take(20)
                            .ToList();
    return Results.Ok(newestProducts);
});

//Get single product
app.MapGet("/api/products/{id}", (BangazonDbContext db, int id) => 
{
    Product product = db.Products
                       .Include(p => p.Seller)
                       .SingleOrDefault(p => p.Id == id);
    if (product != null)
    {
        return Results.Ok(product);
    }
        return Results.NotFound("Product Id not found");
});

//Get products by category
app.MapGet("/api/products/category/{id}", (BangazonDbContext db, int id) =>
{
    var productsInCategory = db.Products
                                .Where(p => p.CategoryId == id)
                                .Where(p => p.Quantity > 0)
                                .OrderByDescending(p => p.DatePosted)
                                .ToList();

    if (productsInCategory.Count > 0)
    {
        return Results.Ok(productsInCategory);
    }
    
    return Results.NotFound("No products are currently available in this category");
});

// Get seller products
app.MapGet("/api/products/users/{userId}", (BangazonDbContext db, int userId) =>
{
var sellerProducts = db.Products
                        .Include(p => p.Category)
                        .Include(p => p.Seller)
                        .OrderByDescending(p => p.DatePosted)
                        .Where(p => p.SellerId ==  userId)
                        .ToList();
return Results.Ok(sellerProducts);
});

//Get user orders
app.MapGet("/api/{id}/orders", (BangazonDbContext db, int id) =>
{
    return db.Orders
            .Include(o => o.Products)
            .Where(o => o.BuyerId == id && o.Closed)
            .OrderByDescending(o => o.OrderDate)
            .ToList();
});


//Get user cart (open order)
app.MapGet("/api/cart/{userId}", (BangazonDbContext db, int userId) =>
{
    var cart = db.Orders
                    .Where(o => o.BuyerId == userId && !o.Closed)
                    .Include(o => o.Products)
                    .ThenInclude(p => p.Seller)
                    .ToList();

    if (cart.Count == 0)
    {
        Order newCart = new Order();
        newCart.BuyerId = userId;
        newCart.Closed = false;
        db.Orders.Add(newCart);
        db.SaveChanges();
        return Results.Ok(newCart);
    }
    
    return Results.Ok(cart);
});

//Get order by Id
app.MapGet("/api/orders/{id}", (BangazonDbContext db, int id) =>
{
    Order order = db.Orders
                .Include(o => o.Products)
                .Include(o => o.PaymentType)
                .SingleOrDefault(o => o.Id == id);
    if (order != null)
    {
        return Results.Ok(order);
    }
    return Results.NotFound("Order Id not found");

});


//Create an order
app.MapPost("/api/orders", (BangazonDbContext db, Order order) => 
{
    db.Orders.Add(order);
    db.SaveChanges();
    return Results.Created($"api/orders/{order.Id}", order);
});

//Patch to close/complete order
app.MapPatch("/api/orders/{id}", (BangazonDbContext db, int id, Order updatedOrder) =>
{
    var order = db.Orders.SingleOrDefault(o => o.Id == id);

    if (order == null)
    {
        return Results.NotFound("Order Id not found");
    }

    order.Closed = updatedOrder.Closed;
    order.PaymentTypeId = updatedOrder.PaymentTypeId;
    order.OrderDate = updatedOrder.OrderDate;
    db.SaveChanges();
    return Results.Ok("Order completed");
});

//Add product to order 
app.MapPost("/api/orders/addProduct", (BangazonDbContext db, AddProductToOrderDTO newProduct) =>
{
    var order = db.Orders
                .Include(o => o.Products)  
                .SingleOrDefault(o => o.Id == newProduct.OrderId);

    if (order == null)
    {
        return Results.NotFound("Order Id not found");
    }

    var product = db.Products
                .Include(p => p.Orders)
                .SingleOrDefault(p => p.Id == newProduct.ProductId);

    if (product == null)
    {
        return Results.NotFound("Product Id not found");
    }

    if (product.Quantity <= 0)
    {
        return Results.BadRequest("Product out of stock");
    }
    product.Quantity = product.Quantity - 1;
    order.Products.Add(product);
    db.SaveChanges();
    return Results.Created($"/api/orders/addProduct", newProduct);
});

//Delete product from order
app.MapDelete("/api/{orderId}/{productId}", (BangazonDbContext db, int orderId, int productId) => 
{
    var order = db.Orders
                .Include(o => o.Products)
                .SingleOrDefault(o => o.Id == orderId);
    
    if (order == null)
    {
        return Results.NotFound("Order Id not found");
    }

    var product = db.Products.SingleOrDefault(p => p.Id == productId);

    if (product == null)
    {
        return Results.BadRequest("Invalid request body");
    }

    if (product == null)
    {
        return Results.NotFound("Product Id not found");
    }

    if (order.Products.Contains(product))
    {
        order.Products.Remove(product);
        product.Quantity = product.Quantity + 1;
        db.SaveChanges();
    }

    return Results.Ok("Product removed from cart");
});

//Get Categories
app.MapGet("/api/categories", (BangazonDbContext db) => 
{
    return db.Categories.ToList();
});

// Get Payment Types
app.MapGet("/api/paymentTypes", (BangazonDbContext db) =>
{
    return db.PaymentTypes.ToList();
});

app.Run();