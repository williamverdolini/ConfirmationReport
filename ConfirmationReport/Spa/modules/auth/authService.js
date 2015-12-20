/// <reference path="~/Spa/bower_components/angular/angular.min.js" />
angular.module('ConfirmationReport')
    .factory('tokenRestangular', [
        'Restangular',
        function (Restangular) {
            return Restangular.withConfig(function (RestangularConfigurer) {
                RestangularConfigurer.setBaseUrl('');
            }); 
        }
    ])
    .factory('tokenService', [
        'tokenRestangular',
        '$httpParamSerializerJQLike',
        function (tokenRestangular, $httpParamSerializerJQLike) {
            return {
                login: function (loginData) {
                    angular.extend(loginData, { grant_type: 'password' });
                    return tokenRestangular.service('token').post($httpParamSerializerJQLike(loginData), {}, { 'Content-Type': 'application/x-www-form-urlencoded' });
                }
            }          
        }
    ])
    .factory('authService', [
            'tokenService',
            'ConfirmationReportSettings',
            '$q',
            'Restangular',
            function (tokenService, ConfirmationReportSettings, $q, Restangular) {
                // store token in localStorage if passed, otherwise clear localStorage
                var _setToken = function (token) {
                    if (!token) {
                        localStorage.removeItem(ConfirmationReportSettings.authToken);
                    } else {
                        localStorage.setItem(ConfirmationReportSettings.authToken, token);
                    }
                };
                var _getToken = function () {
                    return localStorage.getItem(ConfirmationReportSettings.authToken);
                }

                var _authService = {}

                _authService.user = null;
                _authService.deniedState = null;
                _authService.login = function (loginData) {
                    // create deferring result
                    var deferred = $q.defer();

                    tokenService.login(loginData).then(
                        function success(result, status) {
                            if (result.access_token) {
                                _setToken(result.access_token);
                                _authService.getUserInfo().then(
                                    function () {
                                        deferred.resolve(result);
                                    });                                
                            }
                            else {
                                var _authErr = {
                                    code: result.error,
                                    description: result.error_description,
                                    status: status
                                }
                                deferred.reject(_authErr);
                            }
                        },
                        function error(error, status) {
                            deferred.reject(error)
                        });

                    return deferred.promise;
                }
                _authService.logout = function () {
                    _setToken(null);
                }
                _authService.getUserInfo = function () {
                    // create deferring result
                    var deferred = $q.defer();

                    Restangular.setDefaultHeaders({ Authorization: 'bearer ' + _getToken() });
                    var account = Restangular.one('Account');
                    account.customGET('UserInfo').then(
                        function success(user) {
                            _authService.user = user;
                            deferred.resolve(user);
                        },
                        function error(error, status) {
                            deferred.reject(error)
                        });
                    return deferred.promise;
                }
                _authService.checkUser = function () {
                    // create deferring result
                    var deferred = $q.defer();
                    if (_authService.user == null) {
                        if (localStorage.getItem(ConfirmationReportSettings.authToken)) {
                            _authService.getUserInfo().then(
                                function () { deferred.resolve(true); },
                                function () { deferred.reject(false); }
                                )
                        }
                        else {
                            deferred.reject(false);
                        }
                    }
                    else {
                        deferred.resolve(true);
                    }
                    return deferred.promise;
                }

                return _authService;
            }
    ])