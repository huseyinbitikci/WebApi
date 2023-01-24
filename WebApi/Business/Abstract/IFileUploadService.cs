namespace WebApi.Business.Abstract
{
    public interface IFileUploadService
    {
        Task<string> UploadFile(byte[] file);
    }
}
