using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wellway.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientIdSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE SEQUENCE dbo.PatientIdSequence START WITH 51 INCREMENT BY 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP SEQUENCE dbo.PatientIdSequence");
        }
    }
}
