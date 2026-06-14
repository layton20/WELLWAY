using Mapster;
using Wellway.Application.Features.Patients.DTOs;
using Wellway.Domain.Entities;
using Wellway.Domain.ValueObjects;

namespace Wellway.Application.Features.Patients.Mappings;

public sealed class PatientMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Address, AddressDto>();

        config.NewConfig<Patient, PatientDto>()
            .Map(dest => dest.BiologicalSex, src => src.BiologicalSex.ToString())
            .Map(dest => dest.BloodType, src => src.BloodType.ToString());

        config.NewConfig<Patient, PatientSummaryDto>()
            .Map(dest => dest.Status, src => src.IsDeleted ? "Inactive" : "Active");
    }
}
