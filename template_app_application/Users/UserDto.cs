using template_app_application.Abstractions;
using template_app_domain.Entities;

namespace template_app_application.Users
{
    public class UserDto : IMapFrom<User>
    {
        public string Username { get; set; } = null!;
    }
}
