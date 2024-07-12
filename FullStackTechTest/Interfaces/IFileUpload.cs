using Models;

namespace FullStackTechTest.Interfaces
{
    public interface IFileUpload
    {
        Task<List<JsonWrapper>> UploadFile(IFormFile file);
    }
}
