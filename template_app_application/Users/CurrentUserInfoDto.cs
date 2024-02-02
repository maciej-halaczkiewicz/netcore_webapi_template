using AutoMapper;
using template_app_application.Abstractions;
using template_app_domain.Entities;

namespace template_app_application.Users
{
    public class CurrentUserInfoDto : IMapFrom<User>
    {
        public string Username { get; set; } = null!;
        public IList<RoleDto> Roles { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, CurrentUserInfoDto>()
                .ForMember(x => x.Roles, x => x.MapFrom(y => y.UserRoles));
        }
    }
}
