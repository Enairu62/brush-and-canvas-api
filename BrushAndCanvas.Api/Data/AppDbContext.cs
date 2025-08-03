namespace BrushAndCanvas.Api.Data;

using Microsoft.EntityFrameworkCore;
using BrushAndCanvas.Api.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Product> Products { get; set; }
}
