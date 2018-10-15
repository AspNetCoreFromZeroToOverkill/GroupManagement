using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CodingMilitia.PlayBall.GroupManagement.Business.Models;
using CodingMilitia.PlayBall.GroupManagement.Web.Models;

namespace CodingMilitia.PlayBall.GroupManagement.Web.Mappings
{
    public static class GroupMappings
    {
        public static GroupViewModel ToViewModel(this Group model)
        {
            return model != null ? new GroupViewModel { Id = model.Id, Name = model.Name } : null;
        }
        
        public static Group ToServiceModel(this GroupViewModel model)
        {
            return model != null ? new Group { Id = model.Id, Name = model.Name } : null;
        }

        public static IReadOnlyCollection<GroupViewModel> ToViewModel(this IReadOnlyCollection<Group> models)
        {
            if (models.Count == 0)
            {
                return Array.Empty<GroupViewModel>();
            }
            
            var groups = new GroupViewModel[models.Count];
            var i = 0;
            foreach (var model in models)
            {
                groups[i] = model.ToViewModel();
                ++i;
            }

            return new ReadOnlyCollection<GroupViewModel>(groups);
        }
    }
}