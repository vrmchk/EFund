using System.ComponentModel.DataAnnotations;
using EFund.Common.Enums;

namespace EFund.DAL.Entities;

public class Badge
{
    [Key]
    public BadgeType Type { get; set; }

    public List<User> Users { get; set; } = [];
}