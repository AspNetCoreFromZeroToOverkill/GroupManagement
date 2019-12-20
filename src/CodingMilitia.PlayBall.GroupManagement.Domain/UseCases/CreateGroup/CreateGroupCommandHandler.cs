using System;
using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.CreateGroup
{
    public sealed class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, CreateGroupCommandResult>
    {
        private readonly IRepository<Group> _groupsRepository;
        private readonly IQueryHandler<UserByIdQuery, User> _userByIdQueryHandler;

        public CreateGroupCommandHandler(
            IRepository<Group> groupsRepository,
            IQueryHandler<UserByIdQuery, User> userByIdQueryHandler)
        {
            _groupsRepository = groupsRepository ?? throw new ArgumentNullException(nameof(groupsRepository));
            _userByIdQueryHandler =
                userByIdQueryHandler ?? throw new ArgumentNullException(nameof(userByIdQueryHandler));
        }

        public async Task<CreateGroupCommandResult> Handle(CreateGroupCommand request,
            CancellationToken cancellationToken)
        {
            var currentUser = await _userByIdQueryHandler.HandleAsync(
                new UserByIdQuery(request.UserId),
                cancellationToken);
            
            var group = new Group
            {
                Name = request.Name,
                Creator = currentUser
            };

            group.GroupUsers.Add(new GroupUser {User = currentUser, Role = GroupUserRole.Admin});

            var addedGroup = await _groupsRepository.AddAsync(group, cancellationToken);

            return new CreateGroupCommandResult(
                addedGroup.Id,
                addedGroup.Name,
                addedGroup.RowVersion.ToString(),
                new CreateGroupCommandResult.User(
                    currentUser.Id,
                    currentUser.Name));
        }
    }
}