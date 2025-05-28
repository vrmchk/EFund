using System;
using System.Collections.Generic;

namespace EFund.Common.Models.DTO.Violation
{
    public class ViolationGroupDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<ViolationDTO> Violations { get; set; } = [];
    }
} 