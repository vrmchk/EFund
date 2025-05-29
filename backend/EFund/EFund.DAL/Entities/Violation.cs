using System.ComponentModel.DataAnnotations.Schema;
using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities;

public class Violation : BaseEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }

    public Guid ViolationGroupId { get; set; }

    [ForeignKey(nameof(ViolationGroupId))]
    public ViolationGroup ViolationGroup { get; set; } = null!;

    public List<Complaint> Complaints { get; set; } = [];
}