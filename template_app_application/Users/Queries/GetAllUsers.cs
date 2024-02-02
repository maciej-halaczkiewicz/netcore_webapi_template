using template_app_application.Helpers;
using MediatR;

namespace template_app_application.Users.Queries
{
    public class GetAllUsers : PageableRequest, IRequest<PageableList<UserDto>>
    {
    }
}
