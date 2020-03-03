using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
            var byteBuffer = Convert.FromBase64String(imageData.ImageAsBase64);

            var memoryStream = new MemoryStream(byteBuffer);
            memoryStream.Position = 0;

            var bmp = new Bitmap(memoryStream);

            var matrix = new Color[bmp.Width][];
            for (var i = 0; i <= bmp.Width - 1; i++)
            {
                matrix[i] = new Color[bmp.Height];
                for (var j = 0; j < bmp.Height - 1; j++)
                {
                    matrix[i][j] = bmp.GetPixel(i, j);
                }
            }

            memoryStream.Close();

            return Redirect("/");
        }
    }

    public class ImageData
    {
        public string ImageAsBase64 { get; set; }
    }
}