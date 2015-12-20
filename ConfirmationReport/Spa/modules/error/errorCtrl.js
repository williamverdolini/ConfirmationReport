angular.module('ConfirmationReport')
    .controller('ErrorCtrl', [
        '$scope',
        'errorObj',
        '$sce',
        '$translate',
        function ($scope, errorObj, $sce, $translate) {
            $scope.errorMsg = $sce.trustAsHtml(errorObj ? errorObj.errorMsg : $translate.instant('ERROR.GENERIC'));
        }
    ])