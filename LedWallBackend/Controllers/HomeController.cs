using System;
using System.IO;
using System.Threading.Tasks;
using LedWallBackend.Domain;
using LedWallBackend.Repositories;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
            var ms = new MemoryStream(byteBuffer);

            var image = Image.Load(ms);
            image.Mutate(x => x
                .Resize(400, 250));

            var pixelImage = image.CloneAs<Rgba32>();

            var base64String = ToBase64(pixelImage);
            var colors = MapToColorDto(pixelImage);

            var picture = Picture.Create(colors, base64String);
            await _repository.SavePictureAsync(picture);

            return Redirect("/");
        }

        private static string ToBase64(Image bmp)
        {
            var stream = new MemoryStream();
            bmp.SaveAsPng(stream);
            var imageBytes = stream.ToArray();
            var base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

        private static Pixel[][] MapToColorDto(Image<Rgba32> bitmap)
        {
            var matrix = new Pixel[bitmap.Width][];
            for (var i = 0; i <= bitmap.Width - 1; i++)
            {
                matrix[i] = new Pixel[bitmap.Height];
                for (var j = 0; j < bitmap.Height - 1; j++)
                {
                    matrix[i][j] = new Pixel
                    {
                        Red = bitmap[i, j].R,
                        Green = bitmap[i, j].G,
                        Blue = bitmap[i, j].B
                    };
                }
            }

            return matrix;
        }
    }
}