using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using ConfirmRep.Models.Domain;
using ConfirmRep.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace ConfirmRep.Test
{
    [TestFixture]
    public class ConfirmationReportRepositoryTests
    {
        private ConfirmationReportRepository repo;
        private ConfirmReportContext context;

        [SetUp]
        public void CreateRepoWithStubbedContext()
        {
            context = Substitute.For<ConfirmReportContext>();
            repo = new ConfirmationReportRepository(context);      
        }

        [TearDown]
        public void DistroyRepoWithStubbedContext()
        {
            context = null;
            repo = null;
        }

        [Test]
        public async Task FindByNumber__If_ReportNumber_exists__Return_ConfirmationReport_with_that_number()
        {
            // Arrange
            int reportNumber = 5;
            var searchedReport = new ConfirmationReport { ReportNumber = 5 };
            var data = new List<ConfirmationReport> { searchedReport };

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);

            // Act
            var actual = await repo.FindByNumber(reportNumber);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(reportNumber, actual.ReportNumber);
        }

        [Test]
        public async Task FindByNumber__If_ReportNumber_not_exists__Return_null()
        {
            // Arrange
            int model = 5;
            var searchedReport = new ConfirmationReport { ReportNumber = 6 };
            var data = new List<ConfirmationReport> { searchedReport };

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);

            // Act
            var actual = await repo.FindByNumber(model);

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public async Task FindById__If_ReportId_exists__Return_ConfirmationReport_with_that_id()
        {
            // Arrange
            int reportId = 5;
            var searchedReport = new ConfirmationReport { Id = 5 };
            var data = new List<ConfirmationReport> { searchedReport };

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.FindAsync(reportId).Returns(searchedReport);

            // Act
            var actual = await repo.FindById(reportId);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(reportId, actual.Id);
        }

        [Test]
        public async Task FindById__If_ReportId_not_exists__Return_null()
        {
            // Arrange
            int model = 5;
            var searchedReport = new ConfirmationReport { Id = 6 };
            var data = new List<ConfirmationReport> { searchedReport };

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);

            // Act
            var actual = await repo.FindById(model);

            // Assert
            Assert.IsNull(actual);
        }
    }
}
