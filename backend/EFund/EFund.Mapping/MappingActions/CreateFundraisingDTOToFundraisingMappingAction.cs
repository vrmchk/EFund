using AutoMapper;
using EFund.Common.Models.DTO.Fundraising;
using EFund.DAL.Entities;

namespace EFund.Mapping.MappingActions;

public class CreateFundraisingDTOToFundraisingMappingAction : IMappingAction<CreateFundraisingDTO, Fundraising>
{
    public void Process(CreateFundraisingDTO source, Fundraising destination, ResolutionContext context)
    {
        destination.MonobankFundraising = new MonobankFundraising { JarId = source.MonobankJarId };
    }
}