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

            var memoryStream = new MemoryStream(byteBuffer) {Position = 0};

            var bmp = new Bitmap(memoryStream);

            using (Bitmap resized = new Bitmap(400, 600, bmp.PixelFormat))
            {
                using var g = Graphics.FromImage(resized);
                g.DrawImage(bmp, new Rectangle(Point.Empty, resized.Size));
                var bitmapResized = new Bitmap(400, 600, g);

                var matrix = new Color[bitmapResized.Width][];
                for (var i = 0; i <= bitmapResized.Width - 1; i++)
                {
                    matrix[i] = new Color[bitmapResized.Height];
                    for (var j = 0; j < bitmapResized.Height - 1; j++)
                    {
                        matrix[i][j] = bitmapResized.GetPixel(i, j);
                    }
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