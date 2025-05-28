using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities;

public class ViolationGroup : BaseEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public List<Violation> Violations { get; set; } = [];
}