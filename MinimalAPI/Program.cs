using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalAPI;
using MinimalAPI.Models;
using MinimalAPI.Validators;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ProductsMinimalApiContext>();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(ProductValidator));

// Configure JWT authentication
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Define CRUD endpoints
app.MapGet("/products", async (ProductsMinimalApiContext Context) => await Context.Products.ToListAsync());


app.MapGet("/products/{id}", async (int Id, ProductsMinimalApiContext Context) =>
    await Context.Products.FindAsync(Id)
        is Product product
            ? Results.Ok(product)
            : Results.NotFound());

app.MapPost("/products", async (Product Product, ProductsMinimalApiContext Context) =>
{
    Context.Products.Add(Product);
    await Context.SaveChangesAsync();
    return Results.Created($"/Products/{Product.Id}", Product);

}).AddEndpointFilter<ValidationFilter<Product>>()
.RequireAuthorization();


app.MapPut("/products/{id}", async  (int Id, Product Product, ProductsMinimalApiContext Context) =>
{
    var product = await Context.Products.FindAsync(Id);

    if (product is null) return Results.NotFound();

    product.Name = Product.Name;
    product.Price = Product.Price;
    product.Stock = Product.Stock;
    

    await Context.SaveChangesAsync();

    return Results.NoContent();
}).AddEndpointFilter<ValidationFilter<Product>>()
.RequireAuthorization();


app.MapDelete("/products/{id}", async (int Id, ProductsMinimalApiContext Context) =>
{
    if (await Context.Products.FindAsync(Id) is Product product)
    {
        Context.Products.Remove(product);
        await Context.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
}).RequireAuthorization();

// Enable middleware for authentication and Swagger
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();