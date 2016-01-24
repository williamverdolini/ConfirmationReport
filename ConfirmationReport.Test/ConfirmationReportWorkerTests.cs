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

    }
}
