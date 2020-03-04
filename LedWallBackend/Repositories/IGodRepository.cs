using System.Threading.Tasks;

namespace LedWallBackend.Controllers
{
    public interface IGodRepository
    {
        Task SavePictureAsync(Picture picture);
    }
}