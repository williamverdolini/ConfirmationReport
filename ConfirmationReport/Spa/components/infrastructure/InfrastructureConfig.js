angular.module('ConfirmationReport.infrastructure',
    [
        'chieffancypants.loadingBar'
    ])
    .config([
        'RestangularProvider',
        'CustomInterceptorsProvider',
        function (RestangularProvider, CustomInterceptorsProvider) {
            var customInterceptors = CustomInterceptorsProvider.$get();
            //Restangular Global configuration
            RestangularProvider.addResponseInterceptor(customInterceptors.ConvertAllDates);
            RestangularProvider.addRequestInterceptor(customInterceptors.LoaderOn);
            RestangularProvider.addResponseInterceptor(customInterceptors.LoaderOff);
        }
    ])