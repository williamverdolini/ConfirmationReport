angular.module('ConfirmationReport')
    .controller('LoginCtrl', [
        '$scope',
        'authService',
        '$state',
        '$translate',
        'ConfirmationReportSettings',
        function ($scope, authService, $state, $translate, ConfirmationReportSettings) {
            $scope.local = {
                loginForm: {},
                UserName: null,
                Password: null,
                errorMsg: false,
                language: localStorage.getItem(ConfirmationReportSettings.language)
            };

            $scope.actions = {
                login: function () {
                    if ($scope.local.loginForm.$valid) {
                        inputData = {
                            username: $scope.local.UserName,
                            password: $scope.local.Password
                        }
                        $scope.local.errorMsg = false;
                        authService.login(inputData).then(
                            function success(data) {
                                if (authService.deniedState)
                                    //$state.go(authService.deniedState.name, authService.deniedState.params);
                                    $state.go(authService.deniedState.name, authService.deniedState.params, { reload: true });
                                else
                                    //$state.go('master.report');
                                    $state.go('master.report', {}, { reload: true });
                            },
                            function error() {
                                $scope.local.errorMsg = true
                            })

                    }
                },
                changeLanguage: function (langKey) {
                    localStorage.setItem(ConfirmationReportSettings.language, langKey);
                    $scope.local.language = langKey;
                    $translate.use(langKey);
                }
            }
        }
    ])