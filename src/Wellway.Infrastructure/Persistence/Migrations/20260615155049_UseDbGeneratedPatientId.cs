using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wellway.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UseDbGeneratedPatientId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP SEQUENCE dbo.PatientIdSequence");
            migrationBuilder.Sql("CREATE SEQUENCE dbo.PatientIdSequence START WITH 1 INCREMENT BY 1");

            migrationBuilder.AlterColumn<string>(
                name: "HospitalPatientId",
                table: "Patients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValueSql: "'WW-' + FORMAT(NEXT VALUE FOR dbo.PatientIdSequence, 'D6')",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HospitalPatientId",
                table: "Patients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValueSql: "'WW-' + FORMAT(NEXT VALUE FOR dbo.PatientIdSequence, 'D6')");

            migrationBuilder.Sql("DROP SEQUENCE dbo.PatientIdSequence");
            migrationBuilder.Sql("CREATE SEQUENCE dbo.PatientIdSequence START WITH 51 INCREMENT BY 1");
        }
    }
}
