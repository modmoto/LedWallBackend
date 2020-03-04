using System.Threading.Tasks;

namespace LedWallBackend.Controllers
{
    public class GodRepository : IGodRepository
    {
        public Task SavePictureAsync(Picture picture)
        {
            return Task.CompletedTask;
        }
    }
}