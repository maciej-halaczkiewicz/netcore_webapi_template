using template_app_domain.Entities;
using MediatR;

namespace template_app_application.Users.Commands;

public class CreateUser : IRequest<User>
{
    public string? Username { get; set; }
};