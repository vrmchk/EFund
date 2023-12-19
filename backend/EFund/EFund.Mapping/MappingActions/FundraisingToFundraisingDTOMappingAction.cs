using AutoMapper;
using EFund.Common.Models.DTO.Fundraising;
using EFund.Common.Models.DTO.FundraisingReport;
using EFund.DAL.Entities;

namespace EFund.Mapping.MappingActions;

public class FundraisingToFundraisingDTOMappingAction : IMappingAction<Fundraising, FundraisingDTO>
{
    public void Process(Fundraising source, FundraisingDTO destination, ResolutionContext context)
    {
        destination.Reports = context.Mapper.Map<List<FundraisingReportDTO>>(source.Reports);
    }
}