using template_app_application.Abstractions;
using template_app_application.Enums;

namespace template_app_integration_tests.Implementations
{
    public class MockedUserContext : IUserContext
    {
        public string UserName => "testUser";

        public bool IsInRole(Roles role)
        {
            return false;
        }
    }
}
