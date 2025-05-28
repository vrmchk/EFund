using EFund.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFund.DAL.Contexts;

public partial class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public virtual DbSet<UserRegistration> UserRegistrations { get; set; } = null!;
    public virtual DbSet<UserMonobank> UserMonobanks { get; set; } = null!;
    public virtual DbSet<Fundraising> Fundraisings { get; set; } = null!;
    public virtual DbSet<MonobankFundraising> MonobankFundraisings { get; set; } = null!;
    public virtual DbSet<Tag> Tags { get; set; } = null!;
    public virtual DbSet<FundraisingReport> FundraisingReports { get; set; } = null!;
    public virtual DbSet<ReportAttachment> ReportAttachments { get; set; } = null!;
    public virtual DbSet<ViolationGroup> ViolationGroups { get; set; } = null!;
    public virtual DbSet<Violation> Violations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        OnModelCreatingPartial(builder);
    }

    partial void OnModelCreatingPartial(ModelBuilder builder);
}