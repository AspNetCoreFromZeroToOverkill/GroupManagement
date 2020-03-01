using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;
using CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.CreateGroup;
using CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.DeleteGroup;
using CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.GetUserGroupDetail;
using CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.GetUserGroups;
using CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.UpdateGroupDetails;
using CodingMilitia.PlayBall.GroupManagement.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Features.Groups
{
    [Route("groups")]
    public class GroupsController : ControllerBase
    {
        private static readonly string GetByIdActionName
            = nameof(GetByIdAsync).Replace("Async", string.Empty);

        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMediator _mediator;

        public GroupsController(IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAsync(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetUserGroupsQuery(_currentUserAccessor.Id), ct);
            return Ok(result.Groups);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetUserGroupQueryResult>> GetByIdAsync(long id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetUserGroupQuery(_currentUserAccessor.Id, id), ct);

            return result.ToActionResult();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<UpdateGroupDetailsCommandResult>> UpdateAsync(
            long id,
            UpdateGroupDetailsCommandModel model,
            CancellationToken ct)
            => (await _mediator.Send(
                    new UpdateGroupDetailsCommand(
                        _currentUserAccessor.Id,
                        id,
                        model.Name,
                        model.RowVersion),
                    ct))
                .ToActionResult();

        [HttpPut]
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<CreateGroupCommandResult>> AddAsync(
            CreateGroupCommandModel model,
            CancellationToken ct)
            => (await _mediator.Send(
                    new CreateGroupCommand(
                        _currentUserAccessor.Id,
                        model.Name),
                    ct))
                .ToUntypedActionResult(
                    success => 
                        CreatedAtAction(
                            GetByIdActionName,
                            new {id = success.Id},
                            success));

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> RemoveAsync(long id, CancellationToken ct)
        {
            await _mediator.Send(new DeleteGroupCommand(_currentUserAccessor.Id, id), ct);
            return NoContent();
        }
    }
}