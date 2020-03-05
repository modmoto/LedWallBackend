using System;
using System.Threading.Tasks;
using LedWallBackend.Domain;
using LedWallBackend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LedWallBackend.Controllers
{
    public class SqlManController : Controller
    {
        private readonly IPictureRepository _repository;
        private readonly string _infoConnectionString;

        public SqlManController(IPictureRepository repository, DbConnctionInfo info)
        {
            _repository = repository;
            _infoConnectionString = info.ConnectionString;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pic = await _repository.LoadFirstUndecidedPicture();
            return View(pic);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Guid pictureId, string button)
        {
            var state = Enum.Parse<ApprovalState>(button);
            var pic = await _repository.LoadPicture(pictureId);
            pic.MakeDecision(state);
            await _repository.SavePictureAsync(pic);
            return Redirect("/SqlMan");
        }
    }
}