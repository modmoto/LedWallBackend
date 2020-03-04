using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using LedWallBackend.Domain;
using LedWallBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LedWallBackend.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGodRepository _repository;

        public HomeController(IGodRepository repository)
        {
            _repository = repository;
        }
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
            var colors = MapToColorDto(bitmapResized);

            memoryStream.Close();

            var picture = Picture.Create(colors);
            await _repository.SavePictureAsync(picture);
            await _repository.SaveRawPictureAsync(imageData, picture.Id);

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

        private static IEnumerable<IEnumerable<LedColor>> MapToColorDto(Bitmap bitmap)
        {
            var matrix = new LedColor[bitmap.Width][];
            for (var i = 0; i <= bitmap.Width - 1; i++)
            {
                matrix[i] = new LedColor[bitmap.Height];
                for (var j = 0; j < bitmap.Height - 1; j++)
                {
                    matrix[i][j] = new LedColor
                    {
                        Red = bitmap.GetPixel(i, j).R,
                        Green = bitmap.GetPixel(i, j).G,
                        Blue = bitmap.GetPixel(i, j).B
                    };
                }
            }

            return matrix;
        }
    }
}