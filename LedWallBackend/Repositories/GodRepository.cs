using System;
using System.IO;
using System.Threading.Tasks;
using LedWallBackend.Domain;

namespace LedWallBackend.Repositories
{
    public class GodRepository : IGodRepository
    {
        public Task<int> SavePictureAsync(Picture picture)
        {
            return Task.FromResult(1);
        }

        public Task SaveRawPictureAsync(ImageData imageData, Guid id)
        {
            var fileNameWitPath = $"{id}.png";
            using var fs = new FileStream(fileNameWitPath, FileMode.Create);
            using BinaryWriter bw = new BinaryWriter(fs);
            var data = Convert.FromBase64String(imageData.ImageAsBase64);
            bw.Write(data);
            bw.Close();

            return Task.CompletedTask;
        }
    }
}