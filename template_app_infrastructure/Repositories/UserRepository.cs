using template_app_application.Abstractions;
using template_app_domain.Entities;
using template_app_infrastructure.Context;
using template_app_infrastructure.UnitOfWork;

namespace template_app_infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(TemplateAppContext context) 
        : base(context)
    {
    }

    public IQueryable<User> GetAll()
    {
        return _context.Users;
    }

};