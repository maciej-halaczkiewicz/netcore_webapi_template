using template_app_application.Abstractions;
using template_app_application.Enums;
namespace template_app_api.Implementations
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string UserName
        {
            get => _httpContextAccessor.HttpContext.User.Identity.Name;
        }

        public bool IsInRole(Roles role)
        {
            var isInRole = _httpContextAccessor.HttpContext.User.Claims.Any(x => x.Type.Equals("Role") && x.Value == role.ToString());
            return isInRole;
        }
    }
}
