/// <reference path="~/Spa/bower_components/angular/angular.min.js" />
angular.module('ConfirmationReport')
    .controller('ReportCtrl', [
        '$scope',        
        'ReportBaseCtrl',
        '$injector',
        'newReportNumber',
        'authService',
        function ($scope, ReportBaseCtrl, $injector, newReportNumber, authService) {
            // inherit Discitur Base Controller
            $injector.invoke(ReportBaseCtrl, this, { $scope: $scope });

            $scope.local.report.ReportNumber = newReportNumber;
            $scope.local.report.OwnerName = authService.user.UserName;
            $scope.local.report.OwnerCompleteName = authService.user.Name + " " + authService.user.Surname
        }
    ])