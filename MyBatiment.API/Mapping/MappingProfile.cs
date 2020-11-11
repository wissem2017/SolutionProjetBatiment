using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using MyBatiment.API.Resources;
using MyBatiment.Core.Models;

namespace MyBatiment.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain(base de donnée )  vers Resource
            CreateMap<Owner, OwnerResource>();
            CreateMap<Owner, SaveOwnerResource>();

            CreateMap<ProjectItem, ProjectItemResource>();
            CreateMap<ProjectItem, SaveProjectItemResource>();

            CreateMap<ServiceItem, ServiceItemResource>();
            CreateMap<ServiceItem, SaveServiceItemResource>();

            CreateMap<User, UserResource>();
            CreateMap<User, SaveUserResource>();

            // Resources vers Domain ou la base de données
            CreateMap<OwnerResource, Owner>();
            CreateMap<SaveOwnerResource, Owner>();

            CreateMap<ProjectItemResource, ProjectItem>();
            CreateMap<SaveProjectItemResource, ProjectItem>();

            CreateMap<ServiceItemResource, ServiceItem>();
            CreateMap<SaveServiceItemResource, ServiceItem>();

            CreateMap<UserResource, User>();
            CreateMap<SaveUserResource, User>();

        }

    }
}
