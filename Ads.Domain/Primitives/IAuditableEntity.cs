namespace Ads.Domain.Primitives;

public interface IAuditableEntity
{
    DateTime CreationDate { get; set; }
}