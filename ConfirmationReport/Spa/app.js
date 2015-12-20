/// <reference path="~/Spa/bower_components/restangular/dist/restangular.js" />
/// <reference path="~/Spa/bower_components/angular/angular.min.js" />
/// <reference path="~/Spa/bower_components/angular-ui-router/release/angular-ui-router.min.js" />

angular.module('ConfirmationReport',
    [
        'ui.router',
        'ui.bootstrap',
        'restangular',
        'ConfirmationReport.infrastructure',
        'ConfirmationReport.translations',
        'permission'
    ])
    .config([
        '$stateProvider',
        '$locationProvider',
        '$urlRouterProvider',
        'RestangularProvider',
        function ($stateProvider, $locationProvider, $urlRouterProvider, RestangularProvider) {
            // for HTML5 mode
            $locationProvider.html5Mode(true);
            //see: https://github.com/Narzerus/angular-permission/issues/65#issuecomment-107104983
            //$urlRouterProvider.otherwise('/report');
            $urlRouterProvider.otherwise(function ($injector, $location) {
                var $state = $injector.get("$state");
                $state.go('master.report');
            });
            $stateProvider
                //Layout (Abstract States)
                .state('master', {
                    url: '/',
                    abstract: true,
                    templateUrl: 'layout/default.html'
                })
                .state('master.report', {
                    url: 'report',
                    parent: 'master',
                    title: 'New Report',
                    views: {
                        'navbar': {
                            templateUrl: 'modules/navbar/userNavbar.html',
                            controller: 'NavbarCtrl as navbar'
                        },
                        'main' : {
                            templateUrl: 'modules/report/manage/report.html',
                            controller: 'ReportCtrl',
                        }
                    },
                    resolve: {
                        newReportNumber: ['ReportService', function (ReportService) {
                            return ReportService.FindNewReportNumber();
                        }]
                    },
                    data: {
                        permissions: {
                            only: ['user'],
                            redirectTo: 'master.login'
                        }
                    }
                })
                .state('master.report.manage', {
                    url: 'reports/:reportNumber',
                    parent: 'master',
                    title: 'Report',
                    views: {
                        'navbar': {
                            templateUrl: 'modules/navbar/userNavbar.html',
                            controller: 'NavbarCtrl as navbar'
                        },
                        'main': {
                            templateProvider: function ($templateFactory, report, authService) {
                                if (report.Status == "1" || report.OwnerName != authService.user.UserName)
                                    return $templateFactory.fromUrl('modules/report/manage/reportView.html');
                                else
                                    return $templateFactory.fromUrl('modules/report/manage/report.html')
                            },
                            controller: 'ReportEditCtrl'
                        }
                    },
                    resolve: {
                        report: ['ReportService', '$stateParams', function (ReportService, $stateParams) {
                            return ReportService.FindByReportNumber($stateParams.reportNumber);
                        }]
                    },
                    data: {
                        permissions: {
                            only: ['user'],
                            redirectTo: 'master.login'
                        }
                    }
                })
                .state('master.report.manageById', {
                    url: 'reports/id/:id',
                    parent: 'master',
                    title: 'Report By ID',
                    views: {
                        'navbar': {
                            templateUrl: 'modules/navbar/userNavbar.html',
                            controller: 'NavbarCtrl as navbar'
                        },
                        'main': {
                            templateProvider: function ($templateFactory, report, authService) {
                                if (report.Status == "1" || report.OwnerName != authService.user.UserName)
                                    return $templateFactory.fromUrl('modules/report/manage/reportView.html');
                                else
                                    return $templateFactory.fromUrl('modules/report/manage/report.html')
                            },
                            controller: 'ReportEditCtrl'
                        }
                    },
                    resolve: {
                        report: ['ReportService', '$stateParams', function (ReportService, $stateParams) {
                            return ReportService.FindById($stateParams.id);
                        }]
                    },
                    data: {
                        permissions: {
                            only: ['user'],
                            redirectTo: 'master.login'
                        }
                    }
                })
                .state('master.report.list', {
                    url: 'reports/list/:listType',
                    parent: 'master',
                    title: 'Report By ID',
                    views: {
                        'navbar': {
                            templateUrl: 'modules/navbar/userNavbar.html',
                            controller: 'NavbarCtrl as navbar'
                        },
                        'main': {
                            templateUrl: 'modules/report/list/reportList.html',
                            controller: 'ReportListCtrl as list'
                        }
                    },
                    resolve: {
                        reportList: ['ReportService', '$stateParams', 'authService', function (ReportService, $stateParams, authService) {
                            return ReportService.FindAllByOwner({ userName: authService.user.UserName, status: $stateParams.listType || 'draft' });
                        }]
                    },
                    data: {
                        permissions: {
                            only: ['user'],
                            redirectTo: 'master.login'
                        }
                    }
                })
                .state('master.error', {
                    url: 'error',
                    parent: 'master',
                    title: 'Errore',
                    views: {
                        'navbar': {
                            templateUrl: 'modules/navbar/anonymousNavbar.html',
                            controller: 'NavbarCtrl as navbar'
                        },
                        'main': {
                            templateUrl: 'modules/error/error.html',
                            controller: 'ErrorCtrl',
                        }
                    },
                    resolve: {
                        errorObj: [function () {
                            return this.self.error;
                        }]
                    }
                })
                .state('master.login', {
                    url: 'login',
                    parent: 'master',
                    title: 'Login',
                    views: {
                        'navbar': {
                            templateUrl: 'modules/navbar/anonymousNavbar.html',
                            controller: 'NavbarCtrl as navbar'
                        },
                        'main': {
                            templateUrl: 'modules/auth/login.html',
                            controller: 'LoginCtrl'
                        }
                    }
                })
            //Restangular Global configuration
            RestangularProvider.setBaseUrl('/api');

        }
    ])
    .run([
        '$rootScope',
        '$state',
        'Permission',
        'authService',
        '$translate',
        function ($rootScope, $state, Permission, authService, $translate) {
            $rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, error) {
                event.preventDefault();
                var customError
                if (toState.name == 'master.report.manage' && error.status == '404') {
                    customError = { errorMsg: $translate.instant('ERROR.NOT_FOUND_REPORT', { reportNumber: toParams.reportNumber }) };
                }
                else {
                    customError = { errorMsg: $translate.instant('ERROR.EXCEPTION') };
                }
                $state.get('master.error').error = customError;
                return $state.go('master.error', {}, {reload: true});
            });

            //see: https://github.com/Narzerus/angular-permission/issues/103
            //     https://github.com/Narzerus/angular-permission/issues/81
            Permission.defineRole('user', function (stateParams) {
                return authService.checkUser();
            });

            var requestedState = null;
            $rootScope.$on('$stateChangePermissionDenied', function (event, toState, toParams, fromState, fromParams, error) {
                event.preventDefault();
                authService.deniedState = {
                    name: toState.name,
                    params : toParams
                }
            });
        }
    ])

