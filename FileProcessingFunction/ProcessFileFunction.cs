using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FileProcessingFunction
{
    public class ProcessFileFunction
    {
        private readonly ILogger<ProcessFileFunction> _logger;

        public ProcessFileFunction(ILogger<ProcessFileFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(ProcessFileFunction))]
        public async Task Run(
        [BlobTrigger("uploads/{name}", Connection = "AzureWebJobsStorage")] BlobClient blobClient,string name)
        {
            _logger.LogInformation($"Processing file: {name}");

            // Download file stream
            BlobDownloadInfo download = await blobClient.DownloadAsync();
            using (StreamReader reader = new StreamReader(download.Content))
            {
                string content = await reader.ReadToEndAsync();
                _logger.LogInformation($"File Content: {content.Substring(0, Math.Min(100, content.Length))}..."); // Log first 100 chars
            }

            _logger.LogInformation($"Finished processing file: {name}");
        }


    }
}
