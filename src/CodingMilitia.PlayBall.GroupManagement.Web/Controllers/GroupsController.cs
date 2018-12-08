using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Web.Mappings;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    [Route("groups")]
    public class GroupsController : Controller
    {
        private readonly IGroupsService _groupsService;

        public GroupsController(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> IndexAsync(CancellationToken ct)
        {
            var result = await _groupsService.GetAllAsync(ct);
            return View("Index", result.ToViewModel());
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> DetailsAsync(long id, CancellationToken ct)
        {
            var group = await _groupsService.GetByIdAsync(id, ct);

            if (group == null)
            {
                return NotFound();
            }

            return View("Details", group.ToViewModel());
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(long id, GroupViewModel model, CancellationToken ct)
        {
            var group = await _groupsService.UpdateAsync(model.ToServiceModel(), ct);

            if (group == null)
            {
                return NotFound();
            }

            return RedirectToAction("IndexAsync");
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReallyAsync(GroupViewModel model, CancellationToken ct)
        {
            await _groupsService.AddAsync(model.ToServiceModel(), ct);

            return RedirectToAction("IndexAsync");
        }
    }
}