using Bogus;
using Microsoft.EntityFrameworkCore;
using Wellway.Domain.Entities;
using Wellway.Domain.Enums;
using Wellway.Domain.ValueObjects;
using Wellway.Infrastructure.Persistence;

namespace Wellway.Infrastructure.Persistence.Seeding;

public static class PatientSeeder
{
    private const int PatientCount = 50;

    private static readonly (string City, string County, string PostcodeArea)[] CityData =
    [
        ("Birmingham",   "West Midlands",       "B"),
        ("Manchester",   "Greater Manchester",  "M"),
        ("Leeds",        "West Yorkshire",      "LS"),
        ("Sheffield",    "South Yorkshire",     "S"),
        ("Liverpool",    "Merseyside",          "L"),
        ("Bristol",      "Bristol",             "BS"),
        ("Coventry",     "West Midlands",       "CV"),
        ("Leicester",    "Leicestershire",      "LE"),
        ("Nottingham",   "Nottinghamshire",     "NG"),
        ("Newcastle",    "Tyne and Wear",       "NE"),
    ];

    private static readonly string[] GenderIdentityOptions =
    [
        "Male", "Female", "Non-binary", "Transgender woman",
        "Transgender man", "Gender fluid", "Prefer not to say",
    ];

    private static readonly string[] EmergencyRelationships =
    [
        "Spouse", "Parent", "Sibling", "Child", "Partner", "Friend", "Carer",
    ];

    public static async Task<List<Patient>> SeedAsync(WellwayDbContext context)
    {
        if (await context.Patients.AnyAsync())
            return await context.Patients.ToListAsync();

        var faker = new Faker("en_GB");
        var usedNhsNumbers = new HashSet<string>();
        var patients = new List<Patient>(PatientCount);

        for (int i = 1; i <= PatientCount; i++)
        {
            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();

            var preferredName = faker.Random.Bool(0.3f) ? faker.Name.FirstName() : null;

            var dobYear = faker.Random.Int(1940, 2005);
            var dobMonth = faker.Random.Int(1, 12);
            var dobDay = faker.Random.Int(1, DateTime.DaysInMonth(dobYear, dobMonth));
            var dateOfBirth = new DateOnly(dobYear, dobMonth, dobDay);

            var biologicalSex = faker.Random.WeightedRandom(
                [BiologicalSex.Male, BiologicalSex.Female, BiologicalSex.NotDisclosed],
                [0.45f, 0.45f, 0.10f]);

            string? genderIdentity = faker.Random.Bool(0.8f) ? faker.PickRandom(GenderIdentityOptions) : null;

            string? pronouns = genderIdentity switch
            {
                "Male" or "Transgender man"          => "he/him",
                "Female" or "Transgender woman"      => "she/her",
                "Non-binary" or "Gender fluid"       => "they/them",
                _                                    => null,
            };

            string? nhsNumber = null;
            if (faker.Random.Bool(0.85f))
            {
                string candidate;
                do { candidate = faker.Random.ReplaceNumbers("##########"); }
                while (!usedNhsNumbers.Add(candidate));
                nhsNumber = candidate;
            }

            var cityInfo = faker.PickRandom(CityData);
            var district = faker.Random.Int(1, 15);
            var sector = faker.Random.Int(1, 9);
            var unit = faker.Random.String2(2, "ABCDEFGHJKLMNPRSTUVWXY");
            var postCode = $"{cityInfo.PostcodeArea}{district} {sector}{unit}";

            var address = new Address(
                line1: faker.Address.StreetAddress(),
                line2: faker.Random.Bool(0.3f) ? faker.Address.SecondaryAddress() : null,
                city: cityInfo.City,
                county: cityInfo.County,
                postCode: postCode,
                country: "United Kingdom");

            var bloodType = faker.Random.WeightedRandom(
                [BloodType.Unknown, BloodType.APositive, BloodType.ANegative, BloodType.BPositive,
                 BloodType.BNegative, BloodType.ABPositive, BloodType.ABNegative, BloodType.OPositive, BloodType.ONegative],
                [0.30f, 0.0875f, 0.0875f, 0.0875f, 0.0875f, 0.0875f, 0.0875f, 0.0875f, 0.0875f]);

            patients.Add(new Patient(
                createdBy: "system-seed",
                firstName: firstName,
                lastName: lastName,
                dateOfBirth: dateOfBirth,
                biologicalSex: biologicalSex,
                address: address,
                phoneNumber: faker.Phone.PhoneNumber("07### ######"),
                emergencyContactName: faker.Name.FullName(),
                emergencyContactPhone: faker.Phone.PhoneNumber("07### ######"),
                emergencyContactRelationship: faker.PickRandom(EmergencyRelationships),
                nhsNumber: nhsNumber,
                preferredName: preferredName,
                genderIdentity: genderIdentity,
                pronouns: pronouns,
                email: faker.Random.Bool(0.7f) ? faker.Internet.Email(firstName, lastName) : null,
                bloodType: bloodType));
        }

        context.Patients.AddRange(patients);
        await context.SaveChangesAsync();

        return patients;
    }
}
