using AutoMapper;
using template_app_application.Abstractions;
using template_app_application.Enums;
using template_app_application.Helpers;
using template_app_domain.Entities;
using MediatR;

namespace template_app_application.Users.Queries.Handlers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsers, PageableList<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public GetAllUsersHandler(IUnitOfWork unitOfWork, IUserContext userContext, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mapper = mapper;
        _userRepository = _unitOfWork.GetCustomRepository<IUserRepository>();
    }

    public async Task<PageableList<UserDto>> Handle(GetAllUsers request, CancellationToken cancellationToken)
    {
        var users = _userRepository.GetAll();
        var userList = await users.PaginatedListAsync<User, UserDto>(request.PageSize, request.PageNumber, _mapper.ConfigurationProvider);
        return userList;
    }
};