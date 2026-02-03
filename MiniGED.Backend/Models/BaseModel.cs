namespace MiniGED.API.Models;

public abstract class BaseModel
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDate { get; set; }
}