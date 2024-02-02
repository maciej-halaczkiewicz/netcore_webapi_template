using MediatR;

namespace template_app_application.Users.Queries
{
    public class GetUserRoles : IRequest<IList<RoleDto>>
    {
        public string UserName { get; set; }
    }
}
