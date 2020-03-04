using System;
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
        private readonly IPictureRepository _repository;

        public HomeController(IPictureRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
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

            var colors = MapToColorDto(bmp);

            memoryStream.Close();

            var picture = Picture.Create(colors, imageData.ImageAsBase64);
            await _repository.SaveRawPictureAsync(picture.Id, imageData);
            await _repository.SavePictureAsync(picture);

            return Redirect("/");
        }

        private static Pixel[][] MapToColorDto(Bitmap bitmap)
        {
            var matrix = new Pixel[bitmap.Width][];
            for (var i = 0; i <= bitmap.Width - 1; i++)
            {
                matrix[i] = new Pixel[bitmap.Height];
                for (var j = 0; j < bitmap.Height - 1; j++)
                {
                    matrix[i][j] = new Pixel
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