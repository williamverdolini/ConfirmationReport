using System.Data.Entity.Migrations;

namespace ConfirmRep.Repositories.Migrations
{
    internal sealed class ConfirmReportConfiguration : DbMigrationsConfiguration<ConfirmReportContext>
    {
        public ConfirmReportConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "ConfirmRep.Repositories.ConfirmReportContext";
        }
    }

    internal sealed class AuthConfiguration : DbMigrationsConfiguration<AuthContext>
    {
        public AuthConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "ConfirmRep.Models.AuthContext";
        }
    }
}