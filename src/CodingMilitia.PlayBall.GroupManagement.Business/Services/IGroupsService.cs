using System.Collections.Generic;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;

namespace CodingMilitia.PlayBall.GroupManagement.Business.Services
{
    public interface IGroupsService
    {
        IReadOnlyCollection<Group> GetAll();

        Group GetById(long id);

        Group Update(Group group);

        Group Add(Group group);
    }
}