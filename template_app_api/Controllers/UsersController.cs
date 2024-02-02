using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using template_app_application.Users.Queries;

namespace template_app_api.Controllers
{
    [Authorize("AdminPolicy")]
    public class UsersController : ApiControllerBase
    {
        [HttpGet("/api/user-list")]
        public async Task<IResult> Get()
        {
            var users = await Mediator.Send(new GetAllUsers());
            return TypedResults.Ok(users);
        }
    }
}
