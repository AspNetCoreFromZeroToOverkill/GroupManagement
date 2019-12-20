using System.Collections.Generic;

namespace CodingMilitia.PlayBall.GroupManagement.Domain.UseCases.GetUserGroups
{
    public sealed class GetUserGroupsQueryResult
    {
        public GetUserGroupsQueryResult(IReadOnlyCollection<Group> groups)
        {
            Groups = groups;
        }
        
        public IReadOnlyCollection<Group> Groups { get; set; }
        
        public class Group
        {
            public Group(long id, string name, string rowVersion, User creator)
            {
                Id = id;
                Name = name;
                RowVersion = rowVersion;
                Creator = creator;
            }
            
            public long Id { get; }
            public string Name { get; }
            public string RowVersion { get; }
            public User Creator { get; }
        }

        public class User
        {
            public User(string id, string name)
            {
                Id = id;
                Name = name;
            }
            
            public string Id { get; }
            public string Name { get; }
        }
    }
}