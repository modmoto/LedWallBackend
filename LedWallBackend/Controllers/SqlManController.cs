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

        public Picture ApprovalPicture { get; set; }

        public SqlManController(IPictureRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ApprovalPicture = await _repository.LoadFirstUndecidedPicture();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string button)
        {
            var state = Enum.Parse<ApprovalState>(button);
            ApprovalPicture.MakeDecision(state);
            await _repository.SavePictureAsync(ApprovalPicture);
            return Redirect("/");
        }
    }
}