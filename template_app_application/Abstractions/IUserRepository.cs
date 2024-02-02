using template_app_domain.Entities;

namespace template_app_application.Abstractions;

public interface IUserRepository : IRepository<User>
{
    IQueryable<User> GetAll();
};