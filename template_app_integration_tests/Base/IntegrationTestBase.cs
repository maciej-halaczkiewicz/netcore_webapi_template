using NUnit.Framework;

namespace template_app_integration_tests.Base;
public class IntegrationTestBase : TestBase
{
    [SetUp]
    public virtual async Task TestSetUp()
    {
        await Initialize();
    }
    [TearDown]
    public virtual async Task TestTearDown()
    {
        await StopAsync();
    }
}