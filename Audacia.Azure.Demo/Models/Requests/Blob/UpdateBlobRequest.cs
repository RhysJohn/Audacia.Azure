using Microsoft.AspNetCore.Http;

namespace Audacia.Azure.Demo.Models.Requests
{
    public class UpdateBlobRequest
    {
        public string ContainerName { get; set; }
        
        public string BlobName { get; set; }
        
        public IFormFile File { get; set; }
    }
}