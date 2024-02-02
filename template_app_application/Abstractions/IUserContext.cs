using template_app_application.Enums;

namespace template_app_application.Abstractions
{
    public interface IUserContext
    {
        public string UserName { get; }
        public bool IsInRole(Roles role);
    }
}
