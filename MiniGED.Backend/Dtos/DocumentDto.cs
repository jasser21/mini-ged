namespace MiniGED.API.Dtos
{
    public class DocumentDto
    {
        public required string Title {  get; set; }
        public string Description { get; set; } = string.Empty;

    }
}