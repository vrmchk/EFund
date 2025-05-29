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

        builder.Entity<Complaint>(cfg =>
        {
            cfg.HasOne(c => c.Fundraising)
                .WithMany(f => f.Complaints)
                .HasForeignKey(c => c.FundraisingId)
                .OnDelete(DeleteBehavior.Restrict); // 🔐 Avoid unintended cascade

            cfg.HasOne(c => c.RequestedByUser)
                .WithMany()
                .HasForeignKey(c => c.RequestedBy)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Safe

            cfg.HasOne(c => c.RequestedForUser)
                .WithMany()
                .HasForeignKey(c => c.RequestedFor)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Avoid cascade cycles

            cfg.HasOne(c => c.ReviewedByUser)
                .WithMany()
                .HasForeignKey(c => c.ReviewedBy)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Optional reviewer
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder builder);
}