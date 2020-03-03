using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LedWallBackend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromBody] ImageData imageData)
        {
            string fileNameWitPath = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "- ").Replace(":", "") + ".png";
            using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData.ImageAsBase64);
                    bw.Write(data);
                    bw.Close();
                }
            }

            return Redirect("/");
        }
    }

    public class ImageData
    {
        public string ImageAsBase64 { get; set; }
    }
}