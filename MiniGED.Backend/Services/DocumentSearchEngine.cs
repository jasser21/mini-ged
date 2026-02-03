using System;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Meilisearch;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiniGED.API.Dtos;
using MiniGED.API.Interfaces;
using MiniGED.API.Models;
using Newtonsoft.Json.Linq;
using static UglyToad.PdfPig.Core.PdfSubpath;



namespace MiniGED.API.Services
{
    public class DocumentSearchEngine : IDocumentSearchEngine
    {
        MeilisearchClient _client;
        public DocumentSearchEngine(MeilisearchClient client) { 
            _client = client;
        }
        public async Task<TaskInfoStatus> AddIndex(IEnumerable<FilePageSchema> Files)
        {
            
            // await  client.CreateIndexAsync("files", "id");
            var index = _client.Index("files");

            TaskInfo task = await index.AddDocumentsAsync<FilePageSchema>(Files);
            //var testDoc = new FilePageSchema
            //{
            //    Id = "test_1",
            //    PageContent = "Test content",
            //    FileIdIndexed = 1,
            //    PageNumber = 1,
            //    DocumentIdIndexed = 1,
            //    UserIdIndexed = 1,
            //};
            // await index.AddDocumentsAsync(new[] { testDoc });
            // Poll until done
            TaskResource finalTask;

            do
            {
                await Task.Delay(300); // small delay between checks
                finalTask = await _client.GetTaskAsync(task.TaskUid);
            }
            while (finalTask.Status == TaskInfoStatus.Enqueued || finalTask.Status == TaskInfoStatus.Processing);

            return finalTask.Status;
        }

        public async Task<ISearchable<FilePageSchemaResponse>> SearchIndexAsync(String keyword,int userId)
        {
            var index = _client.Index("files");
            var settings = new Settings
            {
                SearchableAttributes = new[] {
        "pageContent",
        "documentName",
        "fileName",
        "description"
    }
            };
            await index.UpdateSettingsAsync(settings);
            var files = await index.SearchAsync<FilePageSchemaResponse>(
                keyword,
                new SearchQuery
                {
                    Q=keyword,
                    AttributesToHighlight = new[] { "Title", "Description", "PageContent" },
                    AttributesToCrop = new string[] { "PageContent" }, 
                    HighlightPreTag = "<mark>",
                    HighlightPostTag = "</mark>",
                    Filter = $"userIdIndexed = {userId}",
                }
                 );
            
            //foreach (var prop in files.Hits)
            //{
            //    Console.WriteLine(prop._Formatted.PageContent);
            //}
           
            return files;
        }
    }
}
