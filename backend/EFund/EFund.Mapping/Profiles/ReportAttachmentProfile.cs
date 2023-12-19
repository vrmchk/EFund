using AutoMapper;
using EFund.Common.Models.DTO.ReportAttachment;
using EFund.DAL.Entities;

namespace EFund.Mapping.Profiles;

public class ReportAttachmentProfile : Profile
{
    public ReportAttachmentProfile()
    {
        CreateMap<ReportAttachment, ReportAttachmentDTO>()
            .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.FilePath));
    }
}