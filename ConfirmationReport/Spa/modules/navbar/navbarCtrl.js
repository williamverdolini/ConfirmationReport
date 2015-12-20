angular.module('ConfirmationReport')
    .controller('NavbarCtrl', [
        'authService',
        '$state',
        function (authService, $state) {
            this.local = {
                user: authService.user
            };

            this.actions = {
                doLogout: function () {
                    authService.logout();
                    $state.go('master.login');
                }
            };
        }
    ])