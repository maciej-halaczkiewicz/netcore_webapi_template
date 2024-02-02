using MediatR;

namespace template_app_application.Users.Queries
{
    public class GetCurrentUserInfo : IRequest<CurrentUserInfoDto>
    {
    }
}
