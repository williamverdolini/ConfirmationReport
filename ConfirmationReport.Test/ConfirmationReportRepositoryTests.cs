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

        [Test]
        public void FindAllByOwner__If_Reports_Context_is_empty__Return_null()
        {
            // Arrange
            var data = new List<ConfirmationReport> {};

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);
            ReportStatus? status = null;

            // Act
            var actual = repo.FindAllByOwner("wilver", status);

            // Assert
            Assert.IsNotNull(actual);
            Assert.That(actual.Count(), Is.EqualTo(0));            
        }

        [Test]
        public void FindAllByOwner__If_Reports_not_contains_owner_items__Return_null()
        {
            // Arrange
            var data = new List<ConfirmationReport> { new ConfirmationReport { Id = 1, OwnerName = "matraf" }, new ConfirmationReport { Id = 2, OwnerName = "giofae" } };

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);
            ReportStatus? status = null;

            // Act
            var actual = repo.FindAllByOwner("wilver", status);

            // Assert
            Assert.IsNotNull(actual);
            Assert.That(actual.Count(), Is.EqualTo(0));
        }

        [Test]
        public void FindAllByOwner__If_Reports_contains_owner_items__Return_item_list()
        {
            // Arrange
            var data = new List<ConfirmationReport> { new ConfirmationReport { Id = 1, OwnerName = "matraf" }, new ConfirmationReport { Id = 2, OwnerName = "giofae" } };

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);
            ReportStatus? status = null;

            // Act
            var actual = repo.FindAllByOwner("matraf", status);

            // Assert
            Assert.IsNotNull(actual);
            Assert.That(actual.Count(), Is.GreaterThan(0));
        }

        [TestCase("wilver", null, ExpectedResult = 0)]
        [TestCase("matraf", null, ExpectedResult = 3)]
        [TestCase("giofae", null, ExpectedResult = 1)]
        [TestCase("matraf", ReportStatus.Draft, ExpectedResult = 2)]
        [TestCase("matraf", ReportStatus.Completed, ExpectedResult = 1)]
        [TestCase("giofae", ReportStatus.Draft, ExpectedResult = 1)]
        [TestCase("giofae", ReportStatus.Completed, ExpectedResult = 0)]
        public int FindAllByOwner__If_Reports_contains_owner_items__Return_item_list(string owner, ReportStatus? status)
        {
            // Arrange
            var data = new List<ConfirmationReport> { 
                new ConfirmationReport { Id = 1, OwnerName = "matraf", Status=ReportStatus.Draft }, 
                new ConfirmationReport { Id = 2, OwnerName = "matraf", Status=ReportStatus.Draft }, 
                new ConfirmationReport { Id = 3, OwnerName = "matraf", Status=ReportStatus.Completed }, 
                new ConfirmationReport { Id = 4, OwnerName = "giofae", Status=ReportStatus.Draft } };

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);

            // Act
            var actual = repo.FindAllByOwner(owner, status);

            // Assert
            return actual.Count();
        }

        [Test]
        public async Task FindNewReportNumber_If_Reports_Context_is_empty__Return_1()
        {
            // Arrange
            //var data = new List<ConfirmationReport> { new ConfirmationReport { Id = 1, OwnerName = "matraf" }, new ConfirmationReport { Id = 2, OwnerName = "giofae" } };
            var data = new List<ConfirmationReport> {};

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);

            // Act
            var actual = await repo.FindNewReportNumber();

            // Assert
            //Assert.IsNotNull(actual);
            Assert.That(actual, Is.EqualTo(1));
        }

        [Test]
        public async Task FindNewReportNumber_If_Reports_Context_is_not_empty__Return_max_ReportNumber_plus_1()
        {
            // Arrange
            var data = new List<ConfirmationReport> { new ConfirmationReport { Id = 1, OwnerName = "matraf", ReportNumber=3 }, new ConfirmationReport { Id = 2, OwnerName = "giofae", ReportNumber=5 } };

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);

            // Act
            var actual = await repo.FindNewReportNumber();

            // Assert
            //Assert.IsNotNull(actual);
            Assert.That(actual, Is.EqualTo(6));
        }

        [Test]
        public async Task SaveDraft__Report_with_Id0__Is_Added_to_repo()
        {
            // Arrange
            var savedReport = new ConfirmationReport {Id = 0 };
            // Create a DbSet substitute.
            var data = new List<ConfirmationReport> { };
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);

            context.Reports.Add(savedReport).Returns(savedReport);
            context.SaveChangesAsync().Returns(0);

            // Act
            await repo.SaveDraft(savedReport);

            // Assert
            context.Reports.Received().Add(savedReport);
            await context.Received().SaveChangesAsync();
            //Assert.That(savedReport.Status, Is.EqualTo(ReportStatus.Draft));
        }

        [Test]
        public async Task SaveDraft__Report_with_Id_not0__Is_Updated_into_repo()
        {
            // Arrange
            var savedReport = new ConfirmationReport { Id = 5 };
            // Create a DbSet substitute.
            var data = new List<ConfirmationReport> { new ConfirmationReport {Id=5}};
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);
            context.Reports.Attach(savedReport).Returns(savedReport);
            context.SaveChangesAsync().Returns(0);

            // Act
            await repo.SaveDraft(savedReport);

            // Assert
            context.Reports.Received().Attach(savedReport);
            await context.Received().SaveChangesAsync();
            //Assert.That(savedReport.Status, Is.EqualTo(ReportStatus.Draft));
        }

        [Test]
        public async Task SaveDraft__Report__Is_Marked_as_Draft()
        {
            // Arrange
            var savedReport = new ConfirmationReport{};
            // Create a DbSet substitute.
            var data = new List<ConfirmationReport> { };
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);

            context.Reports.Add(savedReport).Returns(savedReport);
            context.SaveChangesAsync().Returns(0);

            // Act
            await repo.SaveDraft(savedReport);

            // Assert
            Assert.That(savedReport.Status, Is.EqualTo(ReportStatus.Draft));
        }

        [Test]
        public async Task Save__Report_with_Id0__Is_Added_to_repo()
        {
            // Arrange
            var savedReport = new ConfirmationReport { Id = 0 };
            // Create a DbSet substitute.
            var data = new List<ConfirmationReport> { };
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);

            context.Reports.Add(savedReport).Returns(savedReport);
            context.SaveChangesAsync().Returns(0);

            // Act
            await repo.Save(savedReport);

            // Assert
            context.Reports.Received().Add(savedReport);
            await context.Received().SaveChangesAsync();
            //Assert.That(savedReport.Status, Is.EqualTo(ReportStatus.Draft));
        }

        [Test]
        public async Task Save__Report_with_Id_not0__Is_Updated_into_repo()
        {
            // Arrange
            var savedReport = new ConfirmationReport { Id = 5 };
            // Create a DbSet substitute.
            var data = new List<ConfirmationReport> { new ConfirmationReport { Id = 5 } };
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);
            context.Reports.Attach(savedReport).Returns(savedReport);
            context.SaveChangesAsync().Returns(0);

            // Act
            await repo.Save(savedReport);

            // Assert
            context.Reports.Received().Attach(savedReport);
            await context.Received().SaveChangesAsync();
            //Assert.That(savedReport.Status, Is.EqualTo(ReportStatus.Draft));
        }

        [Test]
        public async Task Save__Report__Is_Marked_as_Completed()
        {
            // Arrange
            var savedReport = new ConfirmationReport { };
            // Create a DbSet substitute.
            var data = new List<ConfirmationReport> { };
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            context.Reports.Returns(set);

            context.Reports.Add(savedReport).Returns(savedReport);
            context.SaveChangesAsync().Returns(0);

            // Act
            await repo.Save(savedReport);

            // Assert
            Assert.That(savedReport.Status, Is.EqualTo(ReportStatus.Completed));
        }
    }
}
