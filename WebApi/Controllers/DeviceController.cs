using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Cryptography;
using WebApi.Business.Concrate;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        DeviceManager _deviceManager;
        WebClient _webClient;

        Dictionary<string, string> _My_dict1;
        public DeviceController(DeviceManager deviceManager, Dictionary<string, string> My_dict1, WebClient WebClient)
        {
            _My_dict1 = My_dict1;
            _deviceManager = deviceManager;
            _webClient = WebClient;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(User user)
        {

            string hash = _deviceManager.Login(user);
            _My_dict1.Add(hash, user.UserName + "*" + user.Password);
            return Ok(hash.ToString());
        }

        [HttpGet]
        [Route("Getsnapshot")]
        public  async Task<IActionResult> Getsnapshot(string hash)
        {
            if (!_My_dict1.ContainsKey(hash))
            {
                return BadRequest("Girilen değer doğru değil");
            }
            var response= await _deviceManager.GetsnapshotAsync(hash);
            return Ok(response);
        }

    }
}
