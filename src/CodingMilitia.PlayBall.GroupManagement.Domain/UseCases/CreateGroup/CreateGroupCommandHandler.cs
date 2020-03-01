using System.Threading;
using System.Threading.Tasks;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data;
using CodingMilitia.PlayBall.GroupManagement.Domain.Data.Queries;
using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using CodingMilitia.PlayBall.GroupManagement.Domain.Shared;
using MediatR;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.CreateGroup
{
    public sealed class CreateGroupCommandHandler
        : IRequestHandler<CreateGroupCommand, Either<Error, CreateGroupCommandResult>>
    {
        private readonly IVersionedRepository<Group, uint> _groupsRepository;
        private readonly IQueryHandler<UserByIdQuery, Optional<User>> _userByIdQueryHandler;

        public CreateGroupCommandHandler(
            IVersionedRepository<Group, uint> groupsRepository,
            IQueryHandler<UserByIdQuery, Optional<User>> userByIdQueryHandler)
        {
            _groupsRepository = Ensure.NotNull(groupsRepository, nameof(groupsRepository));
            _userByIdQueryHandler = Ensure.NotNull(userByIdQueryHandler, nameof(userByIdQueryHandler));
        }

        public async Task<Either<Error, CreateGroupCommandResult>> Handle(
            CreateGroupCommand request,
            CancellationToken cancellationToken)
        {
            var maybeUser = await _userByIdQueryHandler.HandleAsync(
                new UserByIdQuery(request.UserId),
                cancellationToken);

            if (!maybeUser.TryGetValue(out var currentUser))
            {
                return Result.Invalid<CreateGroupCommandResult>("Invalid user to create a group.");
            }

            var group = new Group(request.Name, currentUser);

            var addedGroup = await _groupsRepository.AddAsync(group, cancellationToken);

            return Result.Success(new CreateGroupCommandResult(
                addedGroup.Id,
                addedGroup.Name,
                addedGroup.RowVersion.ToString(),
                new CreateGroupCommandResult.User(
                    currentUser.Id,
                    currentUser.Name)));
        }
    }
}