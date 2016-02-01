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

        [SetUp]
        public void CreateStubs()
        {
            mapper = Substitute.For<IMapper>();
            repo = Substitute.For<IConfirmationReportRepository>();
        }

        [TearDown]
        public void DistroyStubs()
        {
            mapper = null;
            repo = null;
        }

        [Test]
        public async Task FindByNumber__If_Report_exists__Mapping_Returns_ConfirmationReportViewModel_with_that_number()
        {
            // Arrange
            int model = 5;
            var searchedReport = new ConfirmationReport { ReportNumber = 5 };
            var returnedReport = new ConfirmationReportViewModel { ReportNumber = 5 };

            repo.FindByNumber(model).Returns(searchedReport);
            mapper.Map<ConfirmationReportViewModel>(searchedReport).Returns(returnedReport);

            var worker = new ConfirmationReportWorker(repo, mapper);

            // Act
            var actual = await worker.FindByNumber(model);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(returnedReport.ReportNumber, actual.ReportNumber);
        }

        [Test]
        public async Task FindByNumber__If_Report_not_exists__Mapping_Returns_null()
        {
            // Arrange
            int model = 5;
            var searchedReport = null as ConfirmationReport;
            var returnedReport = null as ConfirmationReportViewModel;

            repo.FindByNumber(model).Returns(searchedReport);
            mapper.Map<ConfirmationReportViewModel>(searchedReport).Returns(returnedReport);

            var worker = new ConfirmationReportWorker(repo, mapper);

            // Act
            var actual = await worker.FindByNumber(model);

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public async Task FindById__If_Report_exists__Mapping_Returns_ConfirmationReportViewModel_with_that_id()
        {
            // Arrange
            int model = 5;
            var searchedReport = new ConfirmationReport { Id = 5 };
            var returnedReport = new ConfirmationReportViewModel { Id = 5 };

            repo.FindById(model).Returns(searchedReport);
            mapper.Map<ConfirmationReportViewModel>(searchedReport).Returns(returnedReport);

            var worker = new ConfirmationReportWorker(repo, mapper);

            // Act
            var actual = await worker.FindById(model);

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(returnedReport.Id, actual.Id);
        }

        [Test]
        public async Task FindById__If_Report_not_exists__Mapping_Returns_null()
        {
            // Arrange
            int model = 5;
            var searchedReport = null as ConfirmationReport;
            var returnedReport = null as ConfirmationReportViewModel;

            repo.FindById(model).Returns(searchedReport);
            mapper.Map<ConfirmationReportViewModel>(searchedReport).Returns(returnedReport);

            var worker = new ConfirmationReportWorker(repo, mapper);

            // Act
            var actual = await worker.FindById(model);

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public async Task FindAllByOwner__If_owner_has_reports__Mapping_Returns_list_of_mapped_report_with_that_owner()
        {
            // Arrange
            string owner = "wilver";
            ReportStatus? status = null;
            var data = new List<ConfirmationReport> { new ConfirmationReport { OwnerName = "wilver" } };
            List<ConfirmationReport> returnedList = null;
            var mappedData = new List<ConfirmationReportViewModel> { new ConfirmationReportViewModel { OwnerName = "wilver" } };

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            repo.FindAllByOwner(owner, status).Returns(set);
            mapper.Map<List<ConfirmationReportViewModel>>(Arg.Do<List<ConfirmationReport>>(x => returnedList = x));

            var worker = new ConfirmationReportWorker(repo, mapper);

            // Act
            var actual = await worker.FindAllByOwner(owner, status);

            // Assert
            Assert.IsNotNull(returnedList);
            Assert.That(returnedList.Count, Is.EqualTo(data.Count));
            Assert.That(returnedList, Is.EquivalentTo(data));
        }

        [Test]
        public async Task FindAllByOwner__If_owner_has_no_reports__Mapping_Returns_empty_list()
        {
            // Arrange
            string owner = "wilver";
            ReportStatus? status = null;
            var data = new List<ConfirmationReport> { };
            List<ConfirmationReport> returnedList = null;

            // Create a DbSet substitute.
            var set = Substitute.For<DbSet<ConfirmationReport>, IQueryable<ConfirmationReport>, IDbAsyncEnumerable<ConfirmationReport>>()
                                .SetupData(data);
            repo.FindAllByOwner(owner, status).Returns(set);
            mapper.Map<List<ConfirmationReportViewModel>>(Arg.Do<List<ConfirmationReport>>(x => returnedList = x));

            var worker = new ConfirmationReportWorker(repo, mapper);

            // Act
            var actual = await worker.FindAllByOwner(owner, status);

            // Assert
            Assert.IsNotNull(returnedList);
            Assert.That(returnedList.Count, Is.EqualTo(data.Count));
            Assert.That(returnedList, Is.EquivalentTo(data));
        }

    }
}
