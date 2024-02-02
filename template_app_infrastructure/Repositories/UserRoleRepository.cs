using template_app_application.Abstractions;
using template_app_domain.Entities;
using template_app_infrastructure.Context;
using template_app_infrastructure.UnitOfWork;
namespace template_app_infrastructure.Repositories;

public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(TemplateAppContext context) 
        : base(context)
    {
    }

    public IQueryable<UserRole> GetUserRoles(string userName)
    {
        return _context.UserRoles.Where(x => x.Role.IsActive && x.User.Username == userName).AsQueryable();
    }
};