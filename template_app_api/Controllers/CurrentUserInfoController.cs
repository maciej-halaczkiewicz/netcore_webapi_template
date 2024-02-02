using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using template_app_application.Users.Queries;

namespace template_app_api.Controllers
{
    [Authorize("UserPolicy")]
    public class CurrentUserInfoController : ApiControllerBase
    {
        [HttpGet("/api/current-user-info")]
        public async Task<IResult> Get()
        {
            var userInfo = await Mediator.Send(new GetCurrentUserInfo());
            return TypedResults.Ok(userInfo);
        }
    }
}
