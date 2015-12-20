/// <reference path="~/Spa/bower_components/angular/angular.min.js" />
angular.module('ConfirmationReport')
    .factory('ReportService', [
        'Restangular',
        '$q',
        function (Restangular, $q) {
            var reports = Restangular.all('Reports');
            var SearchReportStatus = {
                draft: 0,
                complete: 1,
                all: null
            };

            return {
                FindById: function (id) {
                    return reports.one('id', id).get();
                },
                FindByReportNumber: function (reportNumber) {
                    return reports.get(reportNumber);
                },
                FindNewReportNumber: function () {
                    return reports.customGET('FindNewReportNumber');
                },
                Save: function (report) {
                    return reports.customPOST(report, "Save");
                },
                SaveDraft: function (report) {
                    return reports.customPOST(report, "SaveDraft");
                },
                FindAllByOwner: function (inputData) {
                    var input = { ownerName: inputData.userName };
                    if (SearchReportStatus[inputData.status] != null)
                        input.status = SearchReportStatus[inputData.status];
                    return reports.customGET("FindAllByOwner", input);
                }
            }
    }])