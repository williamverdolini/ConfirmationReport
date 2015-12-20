angular.module('ConfirmationReport.infrastructure')
    .factory('CustomInterceptors', ['cfpLoadingBar', function (cfpLoadingBar) {
        var _ConvertAllDates = function (data) {
            var IsoDateRegExp = /\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d(\.*)/;

            for (var key in data) {
                if (typeof data[key] == "object")
                    data[key] = _ConvertAllDates(data[key]);
                else if (typeof data[key] == "string" && IsoDateRegExp.test(data[key]))
                    data[key] = new Date(data[key]);
            }
            return data;
        }

        return {
            ConvertAllDates: function (data, operation, what, url, response, deferred) {
                return _ConvertAllDates(data);
            },
            LoaderOn: function (element) { cfpLoadingBar.start(); return element; },
            LoaderOff: function (data) { cfpLoadingBar.complete(); return data; }
        }
    }])