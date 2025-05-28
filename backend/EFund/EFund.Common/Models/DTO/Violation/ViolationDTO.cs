using System;

namespace EFund.Common.Models.DTO.Violation
{
    public class ViolationDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid ViolationGroupId { get; set; }
    }
} 