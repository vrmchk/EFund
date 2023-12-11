using System.ComponentModel.DataAnnotations;

namespace EFund.DAL.Entities.Base;

public abstract class BaseEntity<T> 
    where T : IEquatable<T>
{
    [Key]
    public T Id { get; set; } = default!;
}