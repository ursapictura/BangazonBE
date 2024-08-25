using Bangazon.Models;
using Bangazon.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<CreekRiverDbContext>(builder.Configuration["CreekRiverDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

//Check user
app.MapGet("/checkuser/{uid}", (BangazonBEDbContext db, string id) =>
{
    var user = db.Users.SingleOrDefault(u => u.Id == id);

    if (id == null)
    {
        return Results.NotFound("User not registered");
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
app.MapPost("api/users/{id}", (BangazonDbContext db, User user) =>
{
    db.Users.Add(user);
    db.SaveChanges();
    return Results.Created($"api/users/{user.Id}", "user");
});

//Delete user
app.MapDelete("api/users/{id}", (BangazonDbContext db, int id) => 
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
app.MapPut("api/users/{id}", (BangazonDbContext db, int id, User user) =>
{
    User userToUpdate = db.Users.SingleOrDefault(u => u.Id == id);
    if (userToUpdate == null)
    {
        return Results.NotFound("User Id not found");
    }
    userToUpdate.FirebaseKey = user.FirebaseKey;
    userToUpdate.FirstName = user.FirstName;
    userToUpdate.LastName = user.LastName;
    userToUpdate.Email = user.Email;

    db.SaveChanges();
    return Results.NoContent();
});

//Get products
app.MapGet("api/products", (BaganzonDbContext db) => 
{
    return db.Products
        .Where(p => p.Quantity > 0)
        .Include(p => p.Seller)
        .Include(p => p.Category)
        .OrderByDescending(p => p.DatePosted)
        .ToList();
})

//Get 20 newest products
app.MapGet("api/products/newest", (BangazonDbContext db) => 
{
    <List>Product newestProducts = db.Products
                            .Include(p => p.Category)
                            .Include(p => p.Seller)
                            .OrderByDescending(p => p.DatePosted)
                            .Take(20)
                            .ToList();
    return Results.OK(newestProducts);
});

//Get single product
app.MapGet("api/products/{id}", (BangazonDbContext db, int id) => 
{
    Product product = db.Products.SingleOrDefault(p => p.Id == id);
    if (product != null)
    {
        return Results.Ok(product);
    }
        return Results.NotFound("Product Id not found");
});

//Get products by category
app.MapGet("api/products/category/{id}", (BangazonDbContext db, int id) =>
{
    <List>Products productsInCategory = db.Products
                                .Where(p => p.CategoryId == id)
                                .Where(p => p.Quantity > 0)
                                .OrderByDescending(p => p.DatePosted)
                                .ToList();

    if (productsInCategory.Count > 0)
    {
        return Results.Ok(productsInCategory);
    }
    
    return Results.NoContent("No products are currently available in this category");
});

//Get user orders
app.MapGet("api/orders", (BangazonDbContext db) => 
{
    return db.Orders.Include(p => p.Products);
})


//Get user cart (open order)
app.MapGet("api/cart",(BangazonDbContext db)
{
    var openOrder = db.Orders
                    .Where(o => !o.Closed)
                    .Include(p => p.Products)
                    .ToList();

    if (openOrder == null)
    {
        return Results.NotFound("No Open Orders");
    }
    
    return Results.Ok(openOrder);
});

//Get order by Id
app.MapGet("api/orders/{id}", (BangazonDbContext db, int id) =>
{
    Order order = db.Orders.SingleOrDefault(o => o.Id == id);
    if (order != null)
    {
        return Results.Ok(order);
    }
        return Results.NotFound("Order Id not found");

})


//Create an order
app.MapPost("api/orders", (BangazonDbContext db, Order order)) => 
{
    db.Orders.Add(order);
    db.SaveChanges();
    return Results.Ok($"api/orders/{order.id}", order);
};

//Patch to close/complete order
app.MapPatch("api/orders/{id}", (BangazonDbContext db, int id, Order updatedOrder) => 
{
    var order = db.Orders.SingleOrDefault(o => o.Id == id);

    if (order == null)
    {
        return Results.NotFound("Order Id not found");
    }

    order.Closed = updatedOrder.Closed;
    order.PaymentTypeId = updatedOrder.PaymentTypeId;
    db.SaveChanges();
    return Results.Ok("Order completed");
})

//Add product to order 
app.MapPost("/orders/addProduct", (BangazonDbContext db, addProductToOrderDTO newProduct) =>
{
    var order = db.Orders
                .Include(o => o.Products)  
                .SingleOrDefault(o => o.Id == newProduct.OrderId);

    if (order == null)
    {
        return Results.NotFound("Order Id not found");
    }

    var product = db.Products.FirstOrDefault(newProduct.ProductId);

    if (product == null)
    {
        return Results.NotFound("Product Id not found");
    }

    order.Products.Add(product);
    db.SaveChanges();
    return Results.Created($"/orders/addProduct", newProduct);
});

//Delete product from order
app.MapDelete("orders/deleteProduct", (BangazonDbContext db, deleteProductFromOrder removeProduct) => 
{
    var order = db.Orders
                .Include(o => o.Products)
                .SingleOrDefault(o => o.Id == removeProduct.OrderId);
    
    if (order == null)
    {
        return Results.NotFound("Order Id not found")
    }

    var product = db.Products.FirstOrDefault(removeProduct.ProductId);

    if (product == null)
    {
        return Results.NotFound("Product Id not found")
    }

    if (order.Products.Contains(product))
    {
        order.Products.Remove(product);
        db.SaveChanges();
    }

    return Results.Ok("$Product removed from cart");
});

//Get Categories
app.MapGet("api/categories", (BangazonDbContext db) => 
{
    return db.Categories.ToList();
});