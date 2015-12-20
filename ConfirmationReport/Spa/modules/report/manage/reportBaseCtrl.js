/// <reference path="~/Spa/bower_components/angular/angular.min.js" />
angular.module('ConfirmationReport')
    .factory('ReportBaseCtrl', [
        function(){
            function ReportBaseCtrl($scope, ReportService, $state, authService, $stateParams, $rootScope) {

                function createDefaultDetail(Id) {
                    //no blank default 
                    //see: https://github.com/angular-ui/bootstrap/issues/1114
                    return {
                        ReportId: Id,
                        Date: new Date(),
                        FromTime: new Date("01/01/2000 08:30:00"),
                        ToTime: new Date("01/01/2000 18:30:00"),
                        Description: ''
                    }
                }

                $scope.local = {
                    report: {
                        ReportNumber: null,
                        ReportDate: new Date(),
                        Details: [
                            createDefaultDetail()
                        ],
                        OwnerName: authService.user.UserName
                    },
                    reportForm: {},
                    dateFormat: 'dd/MM/yyyy',
                    newDetail: {
                        Date: new Date()
                    },
                    saveAndPrint: false
                }

                $scope.actions = {
                    addNewDetail: function () {
                        $scope.local.report.Details.push(createDefaultDetail($scope.local.report.Id || 0));
                    },
                    confirmDelete: function () {
                        $scope.local.isPopVisibile = !$scope.local.isPopVisibile;
                        $scope.local.indexToDelete = 1;
                    },
                    deleteDetail: function (index) {
                        $scope.local.report.Details.splice(index, 1);
                    },
                    print: function(){
                        window.print();
                    },
                    saveReport: function (report) {
                        console.log($scope.local.reportForm.$valid)
                        var _saveAndPrint = $scope.local.saveAndPrint;
                        $scope.local.saveAndPrint = false;
                        if ($scope.local.report.Details.length == 0) {
                            $scope.actions.addNewDetail();
                            return;
                        }
                        if ($scope.local.reportForm.$valid) {
                            var promise;
                            if (_saveAndPrint) {
                                promise = ReportService.Save(report);
                            }
                            else {
                                promise = ReportService.SaveDraft(report);
                            }
                            promise.then(function (savedReport) {
                                //see: https://github.com/angular-ui/ui-router/blob/a7d25c6/src/state.js#L1080
                                //     $state.reload('master.report.manage') does not work, cause implicitly use notify:true, so
                                //     reload without notification(in order to force the state's resolve function)
                                //     and after fire manually the $stateChangeSuccess event to re-init the controller
                                if ($state.current.name == 'master.report.manage') {
                                    $state.transitionTo($state.current, $stateParams, { reload: true, notify: false })
                                        .then(function () {
                                            $rootScope.$broadcast('$stateChangeSuccess', $state.current, $stateParams, $state.current, $stateParams);
                                        });
                                }
                                else
                                    $state.go('master.report.manage', { reportNumber: savedReport.ReportNumber }, { reload: true });
                            })
                        }
                    }


                }
            }

            ReportBaseCtrl.$inject = ['$scope', 'ReportService', '$state', 'authService','$stateParams','$rootScope'];
            return (ReportBaseCtrl);
        }

    ])