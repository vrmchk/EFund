using System.ComponentModel.DataAnnotations;

namespace EFund.DAL.Entities;

public class Tag
{
    [Key]
    public string Name { get; set; } = string.Empty;
    public ICollection<Fundraising> Fundraisings { get; set; } = new List<Fundraising>();
}