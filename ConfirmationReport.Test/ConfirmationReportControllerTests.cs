using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfirmRep.Controllers;
using ConfirmRep.Models.View;
using ConfirmRep.ServiceWorkers;
using NSubstitute;
using NUnit.Framework;
using System.Web.Http.Results;
using System.Web.Http;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;

namespace ConfirmRep.Test
{
    [TestFixture]
    public class ConfirmationReportControllerTests
    {
        private void SubstituteUrlHelper<T>(T controller, string returnedUrl) where T : ApiController
        {
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            // Mock the UrlHelper in order for the controller 
            // to be able to successfully call the Url.Link() method.
            UrlHelper urlHelper = Substitute.For<UrlHelper>();
            urlHelper.Link(Arg.Any<string>(), Arg.Any<object>()).Returns(returnedUrl);
            controller.Url = urlHelper;
        }

        [Test]
        public async Task SaveDraft__Report_With_Id_0__Return_Created_Status()
        {
            // Arrange
            var model = new ConfirmationReportViewModel { Id = 0 };

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.SaveDraft(model).Returns(new ConfirmationReportViewModel { Id = 5 });

            var controller = new ConfirmationReportController(worker);
            SubstituteUrlHelper(controller, "http://localhost/api/Reports/id/5");

            // Act
            var actual = await controller.SaveDraft(model);

            // Assert
            Assert.IsAssignableFrom<CreatedNegotiatedContentResult<ConfirmationReportViewModel>>(actual);
        }

        [Test]
        public async Task SaveDraft__Report_With_Id_Not_0__Return_Ok_Status()
        {
            // Arrange
            var model = new ConfirmationReportViewModel { Id = 5 };

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.SaveDraft(model).Returns(new ConfirmationReportViewModel { Id = 5 });

            var controller = new ConfirmationReportController(worker);
            SubstituteUrlHelper(controller, "http://localhost/api/Reports/id/5");

            // Act
            var actual = await controller.SaveDraft(model);

            // Assert
            Assert.IsAssignableFrom<OkNegotiatedContentResult<ConfirmationReportViewModel>>(actual);
        }

        [Test]
        public async Task SaveDraft__If_Model_is_invalid__Return_BadRequest()
        {
            // Arrange
            var model = new ConfirmationReportViewModel { Id = 5 };

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.SaveDraft(model).Returns(new ConfirmationReportViewModel { Id = 5 });

            var controller = new ConfirmationReportController(worker);
            controller.ModelState.AddModelError("fakeError", "fakeError");
            //SubstituteUrlHelper(controller, "http://localhost/api/Reports/id/5");

            // Act
            var actual = await controller.SaveDraft(model);

            // Assert
            Assert.IsAssignableFrom<InvalidModelStateResult>(actual);
        }

        [Test]
        public async Task Save__Report_With_Id_0__Return_Created_Status()
        {
            // Arrange
            var model = new ConfirmationReportViewModel { Id = 0 };

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.SaveDraft(model).Returns(new ConfirmationReportViewModel { Id = 5 });

            var controller = new ConfirmationReportController(worker);
            SubstituteUrlHelper(controller, "http://localhost/api/Reports/id/5");

            // Act
            var actual = await controller.Save(model);

            // Assert
            Assert.IsAssignableFrom<CreatedNegotiatedContentResult<ConfirmationReportViewModel>>(actual);
        }

        [Test]
        public async Task Save__Report_With_Id_Not_0__Return_Ok_Status()
        {
            // Arrange
            var model = new ConfirmationReportViewModel { Id = 5 };

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.SaveDraft(model).Returns(new ConfirmationReportViewModel { Id = 5 });

            var controller = new ConfirmationReportController(worker);
            SubstituteUrlHelper(controller, "http://localhost/api/Reports/id/5");

            // Act
            var actual = await controller.Save(model);

            // Assert
            Assert.IsAssignableFrom<OkNegotiatedContentResult<ConfirmationReportViewModel>>(actual);
        }

        [Test]
        public async Task Save__If_Model_is_invalid__Return_BadRequest()
        {
            // Arrange
            var model = new ConfirmationReportViewModel { Id = 5 };

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.SaveDraft(model).Returns(new ConfirmationReportViewModel { Id = 5 });

            var controller = new ConfirmationReportController(worker);
            controller.ModelState.AddModelError("fakeError", "fakeError");
            //SubstituteUrlHelper(controller, "http://localhost/api/Reports/id/5");

            // Act
            var actual = await controller.Save(model);

            // Assert
            Assert.IsAssignableFrom<InvalidModelStateResult>(actual);
        }

        [Test]
        public async Task FindByNumber__if_report_found__Return_Ok_Status()
        {
            // Arrange
            int model = 1;

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.FindByNumber(model).Returns(new ConfirmationReportViewModel { Id = 5, ReportNumber = 1 });

            var controller = new ConfirmationReportController(worker);
            //SubstituteUrlHelper(controller, "http://localhost/api/Reports/id/5");

            // Act
            var actual = await controller.FindByNumber(model);

            // Assert
            Assert.IsAssignableFrom<OkNegotiatedContentResult<ConfirmationReportViewModel>>(actual);
        }

        [Test]
        public async Task FindByNumber__if_report_found__Return_report_with_same_reportNumber()
        {
            // Arrange
            int model = 1;

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.FindByNumber(model).Returns(new ConfirmationReportViewModel { Id = 5, ReportNumber = 1 });

            var controller = new ConfirmationReportController(worker);
            //SubstituteUrlHelper(controller, "http://localhost/api/Reports/id/5");

            // Act
            var actual = await controller.FindByNumber(model) as OkNegotiatedContentResult<ConfirmationReportViewModel>;

            // Assert
            Assert.IsNotNull(actual.Content);
            Assert.AreEqual(model, actual.Content.ReportNumber);

        }

        [Test]
        public async Task FindByNumber__if_report_not_found__Return_NotFound_Status()
        {
            // Arrange
            int model = 1;

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.FindByNumber(model).Returns(null as ConfirmationReportViewModel);
            var controller = new ConfirmationReportController(worker);

            // Act
            var actual = await controller.FindByNumber(model);

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(actual);
        }

        [Test]
        public async Task FindById__if_report_not_found__Return_NotFound_Status()
        {
            // Arrange
            int model = 1;

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.FindById(model).Returns(null as ConfirmationReportViewModel);
            var controller = new ConfirmationReportController(worker);

            // Act
            var actual = await controller.FindById(model);

            // Assert
            Assert.IsAssignableFrom<NotFoundResult>(actual);
        }

        [Test]
        public async Task FindById__if_report_found__Return_Ok_Status()
        {
            // Arrange
            int model = 1;

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.FindById(model).Returns(new ConfirmationReportViewModel { Id = 1 });

            var controller = new ConfirmationReportController(worker);
            //SubstituteUrlHelper(controller, "http://localhost/api/Reports/id/5");

            // Act
            var actual = await controller.FindById(model);

            // Assert
            Assert.IsAssignableFrom<OkNegotiatedContentResult<ConfirmationReportViewModel>>(actual);
        }

        [Test]
        public async Task FindById__if_report_found__Return_report_with_same_reportNumber()
        {
            // Arrange
            int model = 1;

            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.FindById(model).Returns(new ConfirmationReportViewModel { Id = 1 });

            var controller = new ConfirmationReportController(worker);
            //SubstituteUrlHelper(controller, "http://localhost/api/Reports/id/5");

            // Act
            var actual = await controller.FindById(model) as OkNegotiatedContentResult<ConfirmationReportViewModel>;

            // Assert
            Assert.IsNotNull(actual.Content);
            Assert.AreEqual(model, actual.Content.Id);

        }

        [Test]
        public async Task FindAllByOwner__If_Model_is_invalid__Return_BadRequest()
        {
            // Arrange
            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.FindAllByOwner(null, null).Returns(new List<ConfirmationReportViewModel>());

            var controller = new ConfirmationReportController(worker);
            controller.ModelState.AddModelError("fakeError", "fakeError");

            // Act
            var actual = await controller.FindAllByOwner(null, null);

            // Assert
            Assert.IsAssignableFrom<InvalidModelStateResult>(actual);
        }

        [Test]
        public async Task FindAllByOwner__If_Owner_has_report__Return_Ok_Status()
        {
            var fakeList = new List<ConfirmationReportViewModel>(){
                new ConfirmationReportViewModel{ OwnerName = "wilver"}
            };
            // Arrange
            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.FindAllByOwner("wilver", null).Returns(fakeList);

            var controller = new ConfirmationReportController(worker);

            // Act
            var actual = await controller.FindAllByOwner("wilver", null);

            // Assert
            Assert.IsAssignableFrom<OkNegotiatedContentResult<List<ConfirmationReportViewModel>>>(actual);
        }

        [Test]
        public async Task FindNewReportNumber__Return_OkNegotiatedContentResult_of_Int()
        {
            // Arrange
            IConfirmationReportWorker worker = Substitute.For<IConfirmationReportWorker>();
            worker.FindNewReportNumber().Returns(1);

            var controller = new ConfirmationReportController(worker);

            // Act
            var actual = await controller.FindNewReportNumber();

            // Assert
            Assert.IsAssignableFrom<OkNegotiatedContentResult<int>>(actual);
        }
    }
}
