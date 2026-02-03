using MiniGED.API.Data;
using MiniGED.API.Dtos;
using MiniGED.API.Interfaces;
using MiniGED.API.Models;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;


namespace MiniGED.API.Services
{
    public class DocumentTextExtractor : IDocumentTextExtractor
    {
       private readonly ApplicationDbContext _context;
        // public TextExtractionService() { }
        public DocumentTextExtractor(ApplicationDbContext context) {
            _context = context;
        }

        public  IEnumerable<FilePageSchema> ParsePdfContent(FileProcessingPayload dataparseobj)
        {
            List<FilePageSchema> pages = [];

            var folderName = Path.Combine("Resource", "AllFiles");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var Containingdocument= _context.Documents.Find(dataparseobj.DocumentId);
            var FileMetaData = _context.DocumentFiles.Find(dataparseobj.FileId);
            
            
            var fileName =   dataparseobj.FileId + ".pdf";
            var fullPath = Path.Combine(pathToSave, fileName);
            
            using PdfDocument document = PdfDocument.Open(fullPath);
            foreach (Page page in document.GetPages())
            {
                IReadOnlyList<Letter> letters = page.Letters;
                string example = string.Join(string.Empty, letters.Select(x => x.Value));
               // content = content + example;
                IEnumerable<Word> words = page.GetWords();

                //foreach(Word word in words)
                //{
                //    content += word.Text + " ";
                // };
                FilePageSchema pagecontent = new FilePageSchema
                {
                    Id = $"{dataparseobj.FileId}_{page.Number}",
                    FileIdIndexed = dataparseobj.FileId,
                    UserIdIndexed = dataparseobj.UserId,
                    DocumentIdIndexed = dataparseobj.DocumentId,
                    DocumentName = Containingdocument.Title,
                    FileName= FileMetaData.FileName,
                    Description = Containingdocument.Description,
                    PageNumber = page.Number,
                    PageContent = page.Text,

                };
                pages.Add(pagecontent);

                
            }
            return pages;
        }
    }
}
