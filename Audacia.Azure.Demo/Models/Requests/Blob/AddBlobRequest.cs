using Microsoft.AspNetCore.Http;

namespace Audacia.Azure.Demo.Models.Requests
{
    public class AddBlobRequest
    {
        public string ContainerName { get; set; }
        
        public IFormFile File { get; set; }
    }
}