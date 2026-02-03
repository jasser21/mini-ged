namespace MiniGED.API.Models;

public class Document : BaseModel
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<DocumentFile> Files { get; set; } = new List<DocumentFile>();
}