using Wellway.Domain.Common;
using Wellway.Domain.Enums;
using Wellway.Domain.ValueObjects;

namespace Wellway.Domain.Entities;

public sealed class Patient : AuditableEntity
{
    public string HospitalPatientId { get; private set; } = null!;
    public string? NhsNumber { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? PreferredName { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public BiologicalSex BiologicalSex { get; private set; }
    public string? GenderIdentity { get; private set; }
    public string? Pronouns { get; private set; }
    public Address Address { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string EmergencyContactName { get; private set; } = string.Empty;
    public string EmergencyContactPhone { get; private set; } = string.Empty;
    public string EmergencyContactRelationship { get; private set; } = string.Empty;
    public BloodType BloodType { get; private set; } = BloodType.Unknown;

    private readonly List<Appointment> _appointments = [];
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    private Patient() { }

    public Patient(
        string createdBy,
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        BiologicalSex biologicalSex,
        Address address,
        string phoneNumber,
        string emergencyContactName,
        string emergencyContactPhone,
        string emergencyContactRelationship,
        string? nhsNumber = null,
        string? preferredName = null,
        string? genderIdentity = null,
        string? pronouns = null,
        string? email = null,
        BloodType bloodType = BloodType.Unknown)
        : base(createdBy)
    {
        NhsNumber = nhsNumber;
        FirstName = firstName;
        LastName = lastName;
        PreferredName = preferredName;
        DateOfBirth = dateOfBirth;
        BiologicalSex = biologicalSex;
        GenderIdentity = genderIdentity;
        Pronouns = pronouns;
        Address = address;
        PhoneNumber = phoneNumber;
        Email = email;
        EmergencyContactName = emergencyContactName;
        EmergencyContactPhone = emergencyContactPhone;
        EmergencyContactRelationship = emergencyContactRelationship;
        BloodType = bloodType;
    }

    public void UpdatePersonalDetails(
        string firstName,
        string lastName,
        string? preferredName,
        string? pronouns,
        string? genderIdentity)
    {
        FirstName = firstName;
        LastName = lastName;
        PreferredName = preferredName;
        Pronouns = pronouns;
        GenderIdentity = genderIdentity;
        UpdateTimestamp();
    }

    public void UpdateContactDetails(string phoneNumber, string? email, Address address)
    {
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        UpdateTimestamp();
    }

    public void UpdateEmergencyContact(
        string emergencyContactName,
        string emergencyContactPhone,
        string emergencyContactRelationship)
    {
        EmergencyContactName = emergencyContactName;
        EmergencyContactPhone = emergencyContactPhone;
        EmergencyContactRelationship = emergencyContactRelationship;
        UpdateTimestamp();
    }

    public void UpdateBloodType(BloodType bloodType)
    {
        BloodType = bloodType;
        UpdateTimestamp();
    }

    public void UpdateNhsNumber(string nhsNumber)
    {
        NhsNumber = nhsNumber;
        UpdateTimestamp();
    }
}
