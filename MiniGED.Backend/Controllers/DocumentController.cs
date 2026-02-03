using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MiniGED.API.Data;
using MiniGED.API.Dtos;
using MiniGED.API.Models;

namespace MiniGED.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DocumentController(ApplicationDbContext context)
    {
        _context = context;
    }



    [HttpPost("add")]
    public async Task<IActionResult> AddNewDocument(DocumentDto documentDto)
    {

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return Unauthorized("User Not Authenticated");

        Document newDocument = new Document()
        {
            Title = documentDto.Title,
            Description = documentDto.Description,
            UserId = userId
        };

        _context.Documents.Add(newDocument);
        await _context.SaveChangesAsync();
        return Ok("done");
    }


    
    [HttpGet]
    public async Task<IActionResult> GetUserDocuments()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized();

        int userId = int.Parse(userIdClaim.Value);

        var documents = await _context.Documents.Where(d => d.UserId == userId).Include(d => d.Files).Select(d => new
        {
            d.Id,
            d.Title,
            d.Description,
            d.UploadedAt
        }).ToListAsync();

        return Ok(documents);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocument(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        int userId = int.Parse(userIdClaim.Value);

        var document = await _context.Documents.Include(d => d.Files).FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);

        if (document == null)
        {
            return NotFound(new { message = "Document introuvable ou accès refusé." });
        }

        _context.Documents.Remove(document);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Document supprimé avec succès." });
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentById(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized();
    
        int userId = int.Parse(userIdClaim.Value);
    
        var document = await _context.Documents
            .Include(d => d.Files)
            .FirstOrDefaultAsync(d => d.Id == id && d.UserId == userId);
    
        if (document == null)
            return NotFound(new { message = "Document introuvable ou accès refusé." });
    
        return Ok(new
        {
            document.Id,
            document.Title,
            document.Description,
            document.UploadedAt,
            Files = document.Files.Select(f => new
            {
                f.Id,
                f.FileName,
                f.FilePath,
                f.FileSize
            }).ToList()
        });
    }

} 