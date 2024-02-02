using template_app_application.Abstractions;
using template_app_domain.Entities;
using MediatR;

namespace template_app_application.Users.Commands.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUser, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepository = _unitOfWork.GetCustomRepository<IUserRepository>();
    }

    public async Task<User> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Username = request.Username,
            CreatedBy = "test",
            CreatedOn = DateTime.UtcNow,
            ModifiedBy = "test",
            ModifiedOn = DateTime.UtcNow
        };

        _userRepository.Add(user);
        await _unitOfWork.CommitAsync();
        return user;
    }
};