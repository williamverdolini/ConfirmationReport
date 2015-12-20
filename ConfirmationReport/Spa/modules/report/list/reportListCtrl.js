angular.module('ConfirmationReport')
    .filter('ModeDecode', function ($translate) {
        return function (input) {
            switch (input) {
                case 0: return $translate.instant('LIST.MODE_ON_SITE');// 'Intervento c/o cliente';
                case 1: return $translate.instant('LIST.MODE_LOCAL');//'Intervento c/o ns. sede';
                case 2: return $translate.instant('LIST.MODE_PHONE');//'Intervento telefonico';
                default: return $translate.instant('LIST.MODE_OTHER');//'Altro';
            }

        }
    })
    .controller('ReportListCtrl', [
        'reportList',
        '$stateParams',
        '$translate',
        'ModeDecodeFilter',
        function (reportList, $stateParams, $translate, ModeDecodeFilter) {
            //var type = $stateParams.listType == 'draft' ? 'in BOZZA' : '';
            var type = $stateParams.listType == 'draft' ? $translate.instant('LIST.TITLE_DRAFT') : '';
            

            this.local = {
                reportList: reportList,
                //title: 'Elenco dei Rapporti di intervento ' + type
                title: $translate.instant('LIST.TITLE', { listType: type })
                //title: 'Elenco dei Rapporti di intervento ' + type
            };
        }
    ]);