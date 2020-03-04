using System.Collections.Generic;
using System.Threading.Tasks;
using LedWallBackend.Domain;
using MongoDB.Driver;

namespace LedWallBackend.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly IMongoDatabase _context;

        public PictureRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017/");
            _context = client.GetDatabase("PictureDb");
        }

        public async Task SavePictureAsync(Picture picture)
        {
            var collection = _context.GetCollection<Picture>("Pictures");
            await collection.InsertOneAsync(picture);
        }

        public async Task<Picture> LoadFirstUndecidedPicture()
        {
            var collection = _context.GetCollection<Picture>("Pictures");
            var undecidedPicture = await collection.Find(p => p.State == ApprovalState.Undecided).FirstOrDefaultAsync();
            return undecidedPicture;
        }

        public async Task<IEnumerable<Picture>> LoadApprovedPictures()
        {
            var collection = _context.GetCollection<Picture>("Pictures");
            var undecidedPictures = await collection.FindAsync(p => p.State == ApprovalState.Approved);
            return undecidedPictures.ToList();
        }
    }
}