using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LedWallBackend.Domain;
using MongoDB.Driver;

namespace LedWallBackend.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly DbConnctionInfo _info;

        public PictureRepository(DbConnctionInfo info)
        {
            _info = info;
        }

        public async Task SavePictureAsync(Picture picture)
        {
            var collection = GetContext().GetCollection<Picture>("Pictures");
            await collection.ReplaceOneAsync(
                p => p.Id == picture.Id,
                options: new ReplaceOptions { IsUpsert = true },
                replacement: picture);
        }

        public async Task<Picture> LoadFirstUndecidedPicture()
        {
            var collection = GetContext().GetCollection<Picture>("Pictures");
            var undecidedPicture = await collection.Find(p => p.State == ApprovalState.Undecided).FirstOrDefaultAsync();
            return undecidedPicture;
        }

        public async Task<IEnumerable<Picture>> LoadApprovedPictures()
        {
            var collection = GetContext().GetCollection<Picture>("Pictures");
            var undecidedPictures = await collection.FindAsync(p => p.State == ApprovalState.Approved);
            return undecidedPictures.ToList();
        }

        public async Task<Picture> LoadPicture(Guid pictureId)
        {
            var collection = GetContext().GetCollection<Picture>("Pictures");
            var undecidedPictures = collection.Find(p => p.Id == pictureId);
            return await undecidedPictures.FirstOrDefaultAsync();
        }

        private IMongoDatabase GetContext()
        {
            var client = new MongoClient(_info.ConnectionString);
            return client.GetDatabase("PictureDb");
        }
    }
}