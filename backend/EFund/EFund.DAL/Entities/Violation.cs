using System.ComponentModel.DataAnnotations.Schema;
using EFund.DAL.Entities.Base;

namespace EFund.DAL.Entities
{
    public class Violation : BaseEntity<Guid>
    {
        public string Title { get; set; } = string.Empty;

        public Guid ViolationGroupId { get; set; }

        [ForeignKey(nameof(ViolationGroupId))]
        public ViolationGroup ViolationGroup { get; set; } = null!;
    }
} 