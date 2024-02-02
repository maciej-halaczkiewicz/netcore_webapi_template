using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace template_app_infrastructure.Context;

public partial class TemplateAppContext : DbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
    }
}
