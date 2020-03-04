using System;
using System.Threading.Tasks;
using LedWallBackend.Domain;

namespace LedWallBackend.Repositories
{
    public interface IGodRepository
    {
        Task<int> SavePictureAsync(Picture picture);
        Task SaveRawPictureAsync(ImageData bmp, Guid id);
    }
}