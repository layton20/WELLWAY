using Bogus;
using Microsoft.EntityFrameworkCore;
using Wellway.Domain.Entities;
using Wellway.Domain.Enums;
using Wellway.Infrastructure.Persistence;

namespace Wellway.Infrastructure.Persistence.Seeding;

public static class AppointmentSeeder
{
    private static readonly Dictionary<string, string[]> ReasonsByDepartment = new()
    {
        ["Accident & Emergency"] = ["Acute chest pain", "Head injury assessment", "Severe laceration", "Breathing difficulties", "Suspected fracture"],
        ["Cardiology"]           = ["Chest pain assessment", "ECG review", "Heart palpitations", "Post-cardiac event follow-up", "Hypertension review"],
        ["Orthopaedics"]         = ["Knee pain review", "Hip replacement follow-up", "Fracture assessment", "Joint stiffness", "Shoulder injury"],
        ["Neurology"]            = ["Migraine management", "Seizure follow-up", "Memory loss assessment", "Nerve pain review", "Tremor investigation"],
        ["Oncology"]             = ["Chemotherapy review", "Post-surgery follow-up", "Treatment planning", "Scan results review", "Symptom management"],
        ["Paediatrics"]          = ["Growth assessment", "Vaccination review", "Respiratory infection", "Developmental check", "Ear infection review"],
        ["General Medicine"]     = ["Annual health review", "Medication review", "Blood test results", "Chronic condition management", "Referral follow-up"],
        ["Radiology"]            = ["MRI scan", "X-ray", "CT scan", "Ultrasound scan", "Bone density scan"],
    };

    private static readonly string[] CancellationReasons =
    [
        "Patient request", "Clinician unavailable", "Patient unwell", "Rescheduled at patient's request", "No reason provided",
    ];

    private static readonly string[] ClinicalNotes =
    [
        "Patient responded well to treatment.",
        "Follow-up required in 6 weeks.",
        "Referred to specialist for further investigation.",
        "Patient reported significant improvement.",
        "Medication dosage reviewed and adjusted.",
        "Further imaging requested.",
    ];

    public static async Task SeedAsync(
        WellwayDbContext context,
        List<Patient> patients,
        List<Staff> staff,
        List<Department> departments)
    {
        if (await context.Appointments.AnyAsync()) return;

        var faker = new Faker("en_GB");

        var doctorsByDepartmentId = staff
            .Where(s => s.StaffRole == StaffRole.Doctor)
            .ToDictionary(s => s.DepartmentId);

        // Determine per-patient counts, adjusting to exactly 120 total
        var counts = patients.Select(_ => faker.Random.Int(1, 4)).ToList();
        int total = counts.Sum();

        while (total > 120)
        {
            var eligible = Enumerable.Range(0, counts.Count).Where(i => counts[i] > 1).ToList();
            counts[faker.PickRandom(eligible)]--;
            total--;
        }
        while (total < 120)
        {
            var eligible = Enumerable.Range(0, counts.Count).Where(i => counts[i] < 4).ToList();
            counts[faker.PickRandom(eligible)]++;
            total++;
        }

        var appointments = new List<Appointment>(total);
        var now = DateTimeOffset.UtcNow;

        for (int p = 0; p < patients.Count; p++)
        {
            var patient = patients[p];

            for (int a = 0; a < counts[p]; a++)
            {
                var department = faker.PickRandom(departments);

                if (!doctorsByDepartmentId.TryGetValue(department.Id, out var doctor))
                    doctor = faker.PickRandom(staff.Where(s => s.StaffRole == StaffRole.Doctor).ToList());

                var appointmentType = faker.Random.WeightedRandom(
                    [AppointmentType.InitialConsultation, AppointmentType.FollowUp, AppointmentType.Emergency, AppointmentType.Procedure],
                    [0.30f, 0.45f, 0.10f, 0.15f]);

                int durationInMinutes = appointmentType switch
                {
                    AppointmentType.InitialConsultation => 60,
                    AppointmentType.FollowUp            => 30,
                    AppointmentType.Emergency           => 45,
                    AppointmentType.Procedure           => 90,
                    _                                   => 30,
                };

                bool isPast = faker.Random.Bool(0.6f);

                DateTimeOffset scheduledAt;
                if (isPast)
                {
                    var baseDate = now.AddDays(-faker.Random.Int(1, 180));
                    scheduledAt = new DateTimeOffset(
                        baseDate.Year, baseDate.Month, baseDate.Day,
                        faker.Random.Int(8, 16), faker.PickRandom(new[] { 0, 15, 30, 45 }), 0,
                        TimeSpan.Zero);
                }
                else
                {
                    var baseDate = now.AddDays(faker.Random.Int(1, 90));
                    scheduledAt = new DateTimeOffset(
                        baseDate.Year, baseDate.Month, baseDate.Day,
                        faker.Random.Int(8, 16), faker.PickRandom(new[] { 0, 15, 30, 45 }), 0,
                        TimeSpan.Zero);
                }

                if (!ReasonsByDepartment.TryGetValue(department.Name, out var reasons))
                    reasons = ["General consultation"];

                var appointment = new Appointment(
                    createdBy: "system-seed",
                    patientId: patient.Id,
                    staffId: doctor.Id,
                    departmentId: department.Id,
                    appointmentType: appointmentType,
                    scheduledAt: scheduledAt,
                    durationInMinutes: durationInMinutes,
                    reasonForVisit: faker.PickRandom(reasons),
                    room: $"Room {faker.Random.Int(1, 20)}");

                // Transition to desired status via domain methods
                if (isPast)
                {
                    var roll = faker.Random.Float();
                    if (roll < 0.60f)
                    {
                        appointment.Confirm();
                        appointment.Complete();
                    }
                    else if (roll < 0.70f)
                    {
                        appointment.MarkAsNoShow();
                    }
                    else
                    {
                        appointment.Cancel(faker.PickRandom(CancellationReasons));
                    }
                }
                else
                {
                    var roll = faker.Random.Float();
                    if (roll < 0.50f)
                    {
                        // Scheduled — no action required
                    }
                    else if (roll < 0.90f)
                    {
                        appointment.Confirm();
                    }
                    else
                    {
                        appointment.Cancel(faker.PickRandom(CancellationReasons));
                    }
                }

                if (faker.Random.Bool(0.4f))
                    appointment.UpdateNotes(faker.PickRandom(ClinicalNotes));

                appointments.Add(appointment);
            }
        }

        context.Appointments.AddRange(appointments);
        await context.SaveChangesAsync();
    }
}
