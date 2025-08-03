namespace BrushAndCanvas.Api.Controllers;

using BrushAndCanvas.Api.Data;
using BrushAndCanvas.Api.Models;
using BrushAndCanvas.Api.Dtos.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] bool includeArchived = false)
    {
        var query = _context.Products.AsQueryable();

        if (!includeArchived)
        {
            query = query.Where(p => !p.IsArchived);
        }

        var products = await query.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(CreateProductDto productDto)
    {
        var newProduct = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            ImageUrl = productDto.ImageUrl,
            StockQuantity = productDto.StockQuantity
        };
        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, UpdateProductDto updateDto)
    {
        var productToUpdate = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsArchived);
        if (productToUpdate == null)
        {
            return NotFound();
        }

        productToUpdate.Name = updateDto.Name;
        productToUpdate.Description = updateDto.Description;
        productToUpdate.Price = updateDto.Price;
        productToUpdate.ImageUrl = updateDto.ImageUrl;
        productToUpdate.StockQuantity = updateDto.StockQuantity;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var productToDelete = await _context.Products.FindAsync(id);

        if (productToDelete == null)
        {
            return NotFound();
        }

        productToDelete.IsArchived = true;
        _context.SaveChanges();
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/restore")]
    public async Task<IActionResult> RestoreProduct(int id)
    {
        var productToRestore = await _context.Products.FindAsync(id);

        if (productToRestore == null)
        {
            return NotFound();
        }

        productToRestore.IsArchived = false;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
