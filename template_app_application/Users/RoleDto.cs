using template_app_application.Abstractions;
using template_app_domain.Entities;

namespace template_app_application.Users
{
    public class RoleDto:IMapFrom<UserRole>
    {
        public string RoleName { get; set; }
    }
}
