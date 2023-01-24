using System.Drawing;
using WebApi.Business.Abstract;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Business.Concrate
{
    public class FieUploadManager : IFileUploadService
    {
        public async Task<string> UploadFile(byte[] file)
        {
            string local_file_dir = Path.Combine(Directory.GetCurrentDirectory(), $"Content/");
            if (!Directory.Exists(Path.Combine(local_file_dir))) Directory.CreateDirectory(Path.Combine(local_file_dir));
            string _id = Guid.NewGuid().ToString();
            local_file_dir +=$"{ _id}.jpeg";
            var stream = new MemoryStream(file);
            System.Drawing.Image img = new Bitmap(stream);
            img.Save(local_file_dir, System.Drawing.Imaging.ImageFormat.Jpeg);
            return local_file_dir;
        }
    }
}
