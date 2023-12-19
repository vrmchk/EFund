using AutoMapper;
using EFund.Common.Models.DTO.Fundraising;
using EFund.DAL.Entities;

namespace EFund.Mapping.MappingActions;

public class UpdateFundraisingDTOToFundraisingMappingAction : IMappingAction<UpdateFundraisingDTO, Fundraising>
{
    public void Process(UpdateFundraisingDTO source, Fundraising destination, ResolutionContext context)
    {
        destination.MonobankFundraising.JarId = source.MonobankJarId;
    }
}