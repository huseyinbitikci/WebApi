using System.Security.Cryptography;
using WebApi.Business.Abstract;
using WebApi.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using System.Collections;
using System.Drawing;
using System.IO;

namespace WebApi.Business.Concrate
{
    public class DeviceManager : IDeviceService
    {
        Dictionary<string, string> _My_dict1;
        FieUploadManager _fieUploadManager;

        public DeviceManager(Dictionary<string, string> my_dict1, FieUploadManager fieUploadManager)
        {
            _fieUploadManager = fieUploadManager;
            _My_dict1 = my_dict1;
        }

        public string Login(User user)
        {
            var salt = HashCreate();
            string value = user.UserName + user.Password;
            var hash = HashCreate(value, salt);
            return hash;
        }


        public string HashCreate(string value, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                password: value,
                salt: System.Text.Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            return Convert.ToBase64String(valueBytes);
        }

        public string HashCreate()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        public async Task<string> GetsnapshotAsync(string hash)
        {
            var selectValue = _My_dict1[hash];
            var value = selectValue.Split("*");
            string sWebServiceUrl = "http://212.125.30.226:60213/cgi-bin/snapshot.cgi?channel=1";
            string username = value[0];
            string password = value[1];
            string path = "";
            try
            {
                //HttpClient client = new HttpClient();
                var credentials = new NetworkCredential(username, password);
                using (var handler = new HttpClientHandler { Credentials = credentials })
                using (var client = new HttpClient(handler))
                {
                    byte[] result = await client.GetByteArrayAsync(sWebServiceUrl);
                    path = await _fieUploadManager.UploadFile(result);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Message :{0} ", e.Message);
            }
            return path;
        }

    }
}
