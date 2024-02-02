using template_app_domain.Entities;
using template_app_infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace template_app_data_migrations;

public class SeedDataHelper
{
    private readonly TemplateAppContext _context;

    public SeedDataHelper(TemplateAppContext context)
    {
        _context = context;
    }

    public async Task AddRole(string name)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == name);
        if (role == null)
        {
            await _context.Roles.AddAsync(new Role { IsActive = true, Name = name, Description = $"{name} description" });
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddUser(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username );
        if (user == null)
        {
            await _context.Users.AddAsync(new User() { Username = username, CreatedOn = DateTime.UtcNow, ModifiedOn = DateTime.UtcNow, CreatedBy = "System", ModifiedBy = "System" });
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddUserToRole(string roleName, string username)
    {
        var userRole = await _context.UserRoles.FirstOrDefaultAsync(x => x.Role.Name == roleName && x.User.Username == username);
        if (userRole == null)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            await _context.UserRoles.AddAsync(new UserRole() { RoleId = role.Id, UserId = user.Id, CreatedBy = "System", CreatedOn = DateTime.UtcNow });
            await _context.SaveChangesAsync();
        }
    }
}