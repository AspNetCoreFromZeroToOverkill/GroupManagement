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
        private readonly QueryRunner<UserByIdQuery, Optional<User>> _getUserById;

        public CreateGroupCommandHandler(
            IVersionedRepository<Group, uint> groupsRepository,
            QueryRunner<UserByIdQuery, Optional<User>> getUserById)
        {
            _groupsRepository = Ensure.NotNull(groupsRepository, nameof(groupsRepository));
            _getUserById = Ensure.NotNull(getUserById, nameof(getUserById));
        }

        public async Task<Either<Error, CreateGroupCommandResult>> Handle(
            CreateGroupCommand request,
            CancellationToken cancellationToken)
        {
            var maybeUser = await _getUserById(
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