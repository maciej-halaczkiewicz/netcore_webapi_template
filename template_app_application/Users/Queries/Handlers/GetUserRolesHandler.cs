using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using template_app_application.Abstractions;

namespace template_app_application.Users.Queries.Handlers;

public class GetUserRolesHandler : IRequestHandler<GetUserRoles, IList<RoleDto>>
{
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public GetUserRolesHandler(IUnitOfWork unitOfWork, IUserContext userContext, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mapper = mapper;
        _userRoleRepository = _unitOfWork.GetCustomRepository<IUserRoleRepository>();
    }

    public async Task<IList<RoleDto>> Handle(GetUserRoles request, CancellationToken cancellationToken)
    {
        var userRoles = await _userRoleRepository.GetUserRoles(request.UserName).ProjectTo<RoleDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        return userRoles;
    }
};