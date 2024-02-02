using Microsoft.AspNetCore.Authentication;

namespace template_app_integration_tests.Authentication;

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public string DefaultUserName { get; set; } = null!;
}