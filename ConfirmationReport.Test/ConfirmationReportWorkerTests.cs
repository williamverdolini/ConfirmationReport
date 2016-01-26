using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class ConfirmationReportWorkerTests
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
        public async Task SaveDraft__Report_null__Return_null()
        {
            // Arrange
            ConfirmationReportViewModel model = null;
            mapper.Map<ConfirmationReport>(null).Returns(null as ConfirmationReport);
            mapper.Map<ConfirmationReportViewModel>(null).Returns(null as ConfirmationReportViewModel);

            var worker = new ConfirmationReportWorker(repo, mapper);

            // Act
            var actual = await worker.SaveDraft(model);

            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public async Task SaveDraft__Repo_SaveDraft_is_called_with_mapped_ConfirmationReport()
        {
            // Arrange
            ConfirmationReportViewModel model = new ConfirmationReportViewModel { };
            ConfirmationReport mappedModel = new ConfirmationReport { };
            mapper.Map<ConfirmationReport>(model).Returns(mappedModel);

            var worker = new ConfirmationReportWorker(repo, mapper);

            // Act
            await worker.SaveDraft(model);

            // Assert
            await repo.Received().SaveDraft(mappedModel);
        }

        [Test]
        public async Task Save__Repo_Save_is_called_with_mapped_ConfirmationReport()
        {
            // Arrange
            ConfirmationReportViewModel model = new ConfirmationReportViewModel { };
            ConfirmationReport mappedModel = new ConfirmationReport { };
            mapper.Map<ConfirmationReport>(model).Returns(mappedModel);

            var worker = new ConfirmationReportWorker(repo, mapper);

            // Act
            await worker.Save(model);

            // Assert
            await repo.Received().Save(mappedModel);
        }

    }
}
