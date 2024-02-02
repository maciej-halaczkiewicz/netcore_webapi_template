using template_app_domain.Entities;

namespace template_app_application.Abstractions;

public interface IUserRoleRepository : IRepository<UserRole>
{
    IQueryable<UserRole> GetUserRoles(string userName);
};