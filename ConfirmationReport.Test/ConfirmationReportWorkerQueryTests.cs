using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using ConfirmRep.Infrastructure.Common;
using ConfirmRep.Models.Domain;
using ConfirmRep.Models.View;
using ConfirmRep.Repositories;
using ConfirmRep.ServiceWorkers;
using NSubstitute;
using NUnit.Framework;

namespace ConfirmRep.Test
{
    [TestFixture]
    public class ConfirmationReportWorkerQueryTests
    {
        private IMapper mapper;
        private IConfirmationReportRepository repo;
        private ConfirmReportContext context;

        [SetUp]
        public void CreateStubs()
        {
            context = Substitute.For<ConfirmReportContext>();
            mapper = Substitute.For<IMapper>();
            repo = Substitute.For<ConfirmationReportRepository>(context);
        }

        [TearDown]
        public void DistroyStubs()
        {
            context = null;
            mapper = null;
            repo = null;
        }

        [Test]
        public async Task FindByNumber__If_ReportNumber_exists__Return_ConfirmationReport_with_that_number()
        {
            // Arrange
            int model = 5;
            var searchedReport = new ConfirmationReport { ReportNumber = 5 };
            var data = new List<ConfirmationReport> { searchedReport };
            var returnedReport = new ConfirmationReportViewModel { ReportNumber = 5 };

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);

            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);

            mapper.Map<ConfirmationReportViewModel>(searchedReport).Returns(returnedReport);

            var worker = new ConfirmationReportWorker(repo, mapper);

            // Act
            var actual = await worker.FindByNumber(model);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(returnedReport.ReportNumber, actual.ReportNumber);
        }

        [Test]
        public async Task FindByNumber__If_ReportNumber_not_exists__Return_null()
        {
            // Arrange
            int model = 5;
            var searchedReport = new ConfirmationReport { ReportNumber = 6 };
            var data = new List<ConfirmationReport> { searchedReport };
            var returnedReport = null as ConfirmationReportViewModel;

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);

            context.Reports.Returns(set);
            context.Reports.AsNoTracking().Returns(set);

            mapper.Map<ConfirmationReportViewModel>(searchedReport).Returns(returnedReport);

            var worker = new ConfirmationReportWorker(repo, mapper);

            // Act
            var actual = await worker.FindByNumber(model);

            // Assert
            Assert.IsNull(actual);
        }
    }
}
