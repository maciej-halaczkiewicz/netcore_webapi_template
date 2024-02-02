namespace template_app_domain.Entities;

public partial class AuditHistory
{
    public long Id { get; set; }

    public string Type { get; set; } = null!;

    public int PrimaryKey { get; set; }

    public string Data { get; set; } = null!;

    public string Operation { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime CreatedBy { get; set; }
}
