using AutoMapper;
using AutoMapper.QueryableExtensions;
using template_app_application.Abstractions;
using template_app_application.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace template_app_application.Users.Queries.Handlers;

public class GetCurrentUserInfoHandler : IRequestHandler<GetCurrentUserInfo, CurrentUserInfoDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public GetCurrentUserInfoHandler(IUnitOfWork unitOfWork, IUserContext userContext, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mapper = mapper;
        _userRepository = _unitOfWork.GetCustomRepository<IUserRepository>();
    }

    public async Task<CurrentUserInfoDto> Handle(GetCurrentUserInfo request, CancellationToken cancellationToken)
    {
        var userName = _userContext.UserName;
        var users = await _userRepository.GetAll().Where(x=>x.Username == userName).ProjectTo<CurrentUserInfoDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);
        return users;
    }
};