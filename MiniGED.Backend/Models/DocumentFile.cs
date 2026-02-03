namespace MiniGED.API.Models;

public class DocumentFile : BaseModel
{

    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public long FileSize { get; set; }

    public FileProcessingStatus Status { get; set; }

    public int DocumentId { get; set; }
    public Document Document { get; set; } = null!;
}
public enum FileProcessingStatus
{
    Pending,
    Processing,
    Completed,
    Failed
}