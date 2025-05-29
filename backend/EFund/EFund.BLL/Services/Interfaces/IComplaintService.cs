using EFund.Common.Models.DTO.Complaint;
using EFund.Common.Models.DTO.Error;
using LanguageExt;

namespace EFund.BLL.Services.Interfaces;

public interface IComplaintService
{
    Task<List<ComplaintDTO>> SearchAsync(SearchComplaintsDTO dto);
    Task<Either<ErrorDTO, ComplaintDTO>> GetByIdAsync(Guid id);
    Task<Either<ErrorDTO, ComplaintDTO>> AddAsync(Guid requestedBy, CreateComplaintDTO dto);
    Task<Option<ErrorDTO>> AcceptAsync(ComplaintAcceptDTO dto, Guid reviewedBy);
    Task<Option<ErrorDTO>> RejectAsync(ComplaintRejectDTO dto, Guid reviewedBy);
    Task<Option<ErrorDTO>> RequestChangesAsync(ComplaintRequestChangesDTO dto, Guid reviewedBy);
}