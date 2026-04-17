namespace JoyitasChirinos.Domain.Common;
public abstract class AuditableEntity : BaseEntity
{
    public DateTime UpdatedAt { get; protected set; } = DateTime.Now;
    public void Touch() => UpdatedAt = DateTime.Now;
}
