using System.ComponentModel.DataAnnotations;

namespace EFund.Common.Enums;

public enum BadgeType
{
    None,

    [Display(Name = "Freshman", Description = "Just joined the platform. Let the journey begin!")]
    Freshman,

    [Display(Name = "Novice Fundraiser", Description = "Completed your first fundraising campaign. Welcome aboard!")]
    NoviceFundraiser,

    [Display(Name = "Trailblazer", Description = "Completed 3+ campaigns. You're gaining momentum!")]
    IntermediateFundraiser,

    [Display(Name = "Fundraising Champion", Description = "Successfully completed 10+ fundraising campaigns. You're a pro!")]
    ExperiencedFundraiser,

    [Display(Name = "User Since", Description = "Fundraising for")]
    UserSince
}