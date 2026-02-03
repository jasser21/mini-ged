using System.Text.Json.Serialization;

namespace MiniGED.API.Dtos
{
    public class FilePageSchema
    {
        public string Id { get; set; }
        public int UserIdIndexed { get; set; }
        public int DocumentIdIndexed { get; set; }
        
        public int FileIdIndexed { get; set; }

        public string DocumentName { get; set; }

        public string Description { get; set; }
        public string FileName { get; set; }

        
        public int PageNumber { get; set; }

        public string PageContent { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = DateTime.Now;




    }

}
