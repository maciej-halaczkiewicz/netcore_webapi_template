using System.Security.Claims;
using template_app_application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace template_app_api.Implementations;

public class MyClaimsTransformation : IClaimsTransformation
{
    private readonly ISender _sender;

    public MyClaimsTransformation(ISender sender)
    {
        _sender = sender;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        ClaimsIdentity claimsIdentity = new ClaimsIdentity();
        var claimType = "Role";

        var userRoles = await _sender.Send(new GetUserRoles{UserName = principal.Identity.Name});
        foreach (var userRole in userRoles)
        {
            claimsIdentity.AddClaim(new Claim(claimType, userRole.RoleName));
        }

        principal.AddIdentity(claimsIdentity);
        return principal;
    }
}