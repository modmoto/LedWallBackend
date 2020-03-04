using System;
using System.Drawing;
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
            var byteBuffer = Convert.FromBase64String(imageData.ImageAsBase64);

            var memoryStream = new MemoryStream(byteBuffer) {Position = 0};

            var bmp = new Bitmap(memoryStream);

            var bitmapResized = ResizeBmp(bmp, 400, 600);
            MapToColorDto(bitmapResized);

            memoryStream.Close();

            return Redirect("/");
        }

        private static Bitmap ResizeBmp(Bitmap bmp, int x, int y)
        {
            return bmp;
            // var resized = new Bitmap(x, y, bmp.PixelFormat);
            // using var g = Graphics.FromImage(resized);
            // g.DrawImage(bmp, new Rectangle(Point.Empty, resized.Size));
            // var bitmapResized = new Bitmap(x, y, g);
            // return bitmapResized;
        }

        private static void MapToColorDto(Bitmap bitmap)
        {
            var matrix = new Color[bitmap.Width][];
            for (var i = 0; i <= bitmap.Width - 1; i++)
            {
                matrix[i] = new Color[bitmap.Height];
                for (var j = 0; j < bitmap.Height - 1; j++)
                {
                    matrix[i][j] = bitmap.GetPixel(i, j);
                }
            }
        }
    }

    public class ImageData
    {
        public string ImageAsBase64 { get; set; }
    }
}