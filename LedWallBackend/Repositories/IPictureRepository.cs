using System.Collections.Generic;
using System.Threading.Tasks;
using LedWallBackend.Domain;

namespace LedWallBackend.Repositories
{
    public interface IPictureRepository
    {
        Task SavePictureAsync(Picture picture);
        Task<Picture> LoadFirstUndecidedPicture();
        Task<IEnumerable<Picture>> LoadApprovedPictures();
    }
}