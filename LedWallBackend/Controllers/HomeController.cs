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
            var bmp = ShrinkImageTo(imageData, 400, 250);
            var base64String = ToBase64(bmp);
            var colors = MapToColorDto(bmp);

            var picture = Picture.Create(colors, base64String);
            await _repository.SavePictureAsync(picture);

            return Redirect("/");
        }

        private static string ToBase64(Bitmap bmp)
        {
            var stream = new MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            var imageBytes = stream.ToArray();
            var base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

        private static Bitmap ShrinkImageTo(ImageData imageData, int width, int height)
        {
            var byteBuffer = Convert.FromBase64String(imageData.ImageAsBase64);
            var ms = new MemoryStream(byteBuffer);
            var image = Image.FromStream(ms);

            var newImage = new Bitmap(width, height);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, width, height);
            var bmp = new Bitmap(newImage);
            return bmp;
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