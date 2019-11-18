using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Business.Services;
using CodingMilitia.PlayBall.GroupManagement.Web.Mappings;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Controllers
{
    [Route("groups")]
    public class GroupsController : ControllerBase
    {
        private static readonly string GetByIdActionName 
            = nameof(GetByIdAsync).Replace("Async", string.Empty);
        
        private readonly IGroupsService _groupsService;

        public GroupsController(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAsync(CancellationToken ct)
        {
            var result = await _groupsService.GetAllAsync(ct);
            return Ok(result.ToModel());
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(long id, CancellationToken ct)
        {
            var group = await _groupsService.GetByIdAsync(id, ct);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group.ToModel());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, GroupModel model, CancellationToken ct)
        {
            model.Id = id; //not needed when we move to MediatR
            var group = await _groupsService.UpdateAsync(model.ToServiceModel(), ct);

            return Ok(group.ToModel());
        }

        [HttpPut]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddAsync(GroupModel model, CancellationToken ct)
        {
            model.Id = 0; //not needed when we move to MediatR
            var group = await _groupsService.AddAsync(model.ToServiceModel(), ct);

            return CreatedAtAction(GetByIdActionName, new {id = group.Id}, group.ToModel());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveAsync(long id, CancellationToken ct)
        {
            await _groupsService.RemoveAsync(id, ct);

            return NoContent();
        }
    }
}