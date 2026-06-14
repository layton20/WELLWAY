using System.Text.RegularExpressions;
using FluentValidation;
using Wellway.Domain.Enums;

namespace Wellway.Application.Features.Patients.Commands.RegisterPatient;

public sealed class RegisterPatientCommandValidator : AbstractValidator<RegisterPatientCommand>
{
    public RegisterPatientCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PreferredName).MaximumLength(100).When(x => x.PreferredName is not null);
        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Date of birth must be in the past.");
        RuleFor(x => x.BiologicalSex)
            .NotEmpty()
            .IsEnumName(typeof(BiologicalSex), caseSensitive: false)
            .WithMessage($"BiologicalSex must be one of: {string.Join(", ", Enum.GetNames<BiologicalSex>())}.");
        RuleFor(x => x.BloodType)
            .IsEnumName(typeof(BloodType), caseSensitive: false)
            .WithMessage($"BloodType must be one of: {string.Join(", ", Enum.GetNames<BloodType>())}.")
            .When(x => x.BloodType is not null);
        RuleFor(x => x.NhsNumber)
            .Length(10)
            .Matches(@"^\d{10}$").WithMessage("NHS number must be exactly 10 digits.")
            .When(x => x.NhsNumber is not null);
        RuleFor(x => x.GenderIdentity).MaximumLength(100).When(x => x.GenderIdentity is not null);
        RuleFor(x => x.Pronouns).MaximumLength(50).When(x => x.Pronouns is not null);

        RuleFor(x => x.Address).NotNull();
        When(x => x.Address is not null, () =>
        {
            RuleFor(x => x.Address.Line1).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Address.Line2).MaximumLength(200).When(x => x.Address.Line2 is not null);
            RuleFor(x => x.Address.City).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Address.County).MaximumLength(100).When(x => x.Address.County is not null);
            RuleFor(x => x.Address.PostCode)
                .NotEmpty()
                .MaximumLength(10)
                .Matches(@"^[A-Z]{1,2}[0-9][0-9A-Z]?\s?[0-9][A-Z]{2}$", RegexOptions.IgnoreCase)
                .WithMessage("PostCode must be a valid UK postcode (e.g. SW1A 1AA).");
            RuleFor(x => x.Address.Country).MaximumLength(100).When(x => x.Address.Country is not null);
        });

        RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email)
            .EmailAddress().MaximumLength(255)
            .When(x => x.Email is not null);
        RuleFor(x => x.EmergencyContactName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EmergencyContactPhone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.EmergencyContactRelationship).NotEmpty().MaximumLength(100);
    }
}
