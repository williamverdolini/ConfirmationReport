using System.Data.Entity;
using ConfirmRep.Models.Domain;
using ConfirmRep.Repositories.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ConfirmRep.Repositories
{
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext() : base("AuthContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, AuthConfiguration>());
        }
    }
}