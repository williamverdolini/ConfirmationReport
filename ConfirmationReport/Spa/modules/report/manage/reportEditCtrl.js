/// <reference path="~/Spa/bower_components/angular/angular.min.js" />

angular.module('ConfirmationReport')
    .controller('ReportEditCtrl', [
        '$scope',
        'ReportBaseCtrl',
        '$injector',
        'report',
        function ($scope, ReportBaseCtrl, $injector, report) {
            // inherit Discitur Base Controller
            $injector.invoke(ReportBaseCtrl, this, { $scope: $scope });

            $scope.local.report = report;

        }
    ]);