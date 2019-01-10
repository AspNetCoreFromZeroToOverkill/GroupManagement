using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Mappings
{
    public static class GroupMappings
    {
        public static GroupModel ToModel(this Group model)
        {
            return model != null ? new GroupModel { Id = model.Id, Name = model.Name, RowVersion = model.RowVersion} : null;
        }
        
        public static Group ToServiceModel(this GroupModel model)
        {
            return model != null ? new Group { Id = model.Id, Name = model.Name, RowVersion = model.RowVersion} : null;
        }

        public static IReadOnlyCollection<GroupModel> ToModel(this IReadOnlyCollection<Group> models)
        {
            if (models.Count == 0)
            {
                return Array.Empty<GroupModel>();
            }
            
            var groups = new GroupModel[models.Count];
            var i = 0;
            foreach (var model in models)
            {
                groups[i] = model.ToModel();
                ++i;
            }

            return new ReadOnlyCollection<GroupModel>(groups);
        }
    }
}