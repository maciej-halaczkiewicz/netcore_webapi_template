using template_app_domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace template_app_infrastructure.Context;

public partial class TemplateAppContext : DbContext
{
    public TemplateAppContext(DbContextOptions<TemplateAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditHistory> AuditHistories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("db_owner");

        modelBuilder.Entity<AuditHistory>(entity =>
        {
            entity.ToTable("AuditHistory", "dbo");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Operation).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role", "dbo");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", "dbo");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole", "dbo");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
