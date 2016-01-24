angular.module('ConfirmationReport.translations',
    [
        'pascalprecht.translate',
        'ConfirmationReport.infrastructure'
    ])
    .config([
        '$translateProvider',
        'ConfirmationReportSettings',
        function ($translateProvider, ConfirmationReportSettings) {
            // register italian translation table
            $translateProvider.translations('it_IT', {
                'NAV': {
                    'BRAND': 'Azienda S.P.A.',
                    'SEARCH_PLACEHOLDER': 'Numero Rapporto...',
                    'NEW_REPORT': 'Nuovo Rapportino',
                    'YOUR_REPORTS_TITLE': 'I tuoi rapportini...',
                    'DRAFTS': 'In Bozza',
                    'ALL': 'Tutti',
                    'LOGOUT': 'Esci'
                },
                'LOGIN': {
                    'LOGIN_TITLE': 'Rapportini Online',
                    'USERNAME': 'User Name',
                    'PASSWORD': 'Password',
                    'INVALID_CREDENTIALS': 'Credenziali non corrette',
                    'LOGIN_BUTTON': 'Login',
                    'REGISTRATION_MESSAGE': "Se non sei registrato contatta l'amministratore di sistema."
                },
                'ERROR': {
                    'GENERIC': '<h1>Errore generico</h1>',
                    'NOT_FOUND_REPORT': "<h1>Ooooooops...<br/><br/><small><p>Il Rapporto di intervento n.<b>{{reportNumber}}</b> NON è stato trovato.</p><p>Sicuro di aver inserito il numero corretto?</p></small></h1>",
                    'EXCEPTION': "<h1>Houston, abbiamo un problema!<br/><small>Spiega a William cosa hai fatto e le cose (forse) si sistemeranno.</small></h1>"
                },
                'REPORT': {
                    'CONFIRMATION_REPORT': 'Rapporto di Intervento',
                    'NUMBER': 'N.',
                    'NUMBET_TOOLTIP': 'Numero temporaneo, sarà confermato al primo salvataggio.',
                    'AT_DATE': 'Del:',
                    'CUSTOMER': 'Cliente',
                    'CUSTOMER_REP': 'Persona di riferimento',
                    'OWNERNAME': 'Tecnico Azienda S.P.A.',
                    'OTHER_OPERATORS': 'Altri tecnici Azienda S.P.A.',
                    'INTERVENTION_MODE': 'Modalità di intervento',
                    'INTERVENTION_MODE_ONSITE': 'Intervento c/o cliente',
                    'INTERVENTION_MODE_PHONE': 'Intervento telefonico',
                    'INTERVENTION_MODE_LOCAL': 'Intervento c/o ns. sede',
                    'INTERVENTION_MODE_OTHER': 'Altro : ',
                    'INTERVENTION_MODE_OTHER_PO': 'Altra modalità',
                    'DETAIL_DATE': 'Data',
                    'DETAIL_FROM_TIME': 'Dalle Ore',
                    'DETAIL_TO_TIME': 'Alle Ore',
                    'DETAIL_DESCRIPTION': 'Descrizione Intervento',
                    'DETAIL_DELETE_CONFIRM': 'Confermi la cancellazione della riga?',
                    'DETAIL_NEW': 'Nuovo dettaglio',
                    'NOTES': 'Note',
                    'SAVE_BUTTON': 'Salva',
                    'SAVE_COMPLETE_PRINT_BUTTON': 'Salva Definitivo e Stampa',
                    'PRINT_BUTTON': 'Stampa',
                    'DRAFT': 'Bozza'
                },
                'LIST': {
                    'TITLE': "Elenco dei Rapporti di intervento {{listType}}",
                    'TITLE_DRAFT': 'in BOZZA',
                    'DRAFT': 'Bozza',
                    'ROW_TITLE': 'Rapportino n.{{ReportNumber}} del {{ReportDate}} - {{CustomerName}}',
                    'ROW_SUBTITLE': '{{InterventionMode}}, in carico a: {{OwnerCompleteName}}',
                    'NO_ROWS': 'Nessun Rapportino recuperato.',
                    'MODE_ON_SITE': 'Intervento c/o cliente',
                    'MODE_LOCAL': 'Intervento c/o ns. sede',
                    'MODE_PHONE': 'Intervento telefonico',
                    'MODE_OTHER': 'Altro'
                }
            });
            // register english translation table
            $translateProvider.translations('en_EN', {
                'NAV': {
                    'BRAND': 'New Company LTD',
                    'SEARCH_PLACEHOLDER': 'Report Number...',
                    'NEW_REPORT': 'New Report',
                    'YOUR_REPORTS_TITLE': 'Your reports...',
                    'DRAFTS': 'Drafts',
                    'ALL': 'All',
                    'LOGOUT': 'Logout'
                },
                'LOGIN': {
                    'LOGIN_TITLE': 'Online Confirmation Report',
                    'USERNAME': 'User Name',
                    'PASSWORD': 'Password',
                    'INVALID_CREDENTIALS': 'Invalid Credentials',
                    'LOGIN_BUTTON': 'Login',
                    'REGISTRATION_MESSAGE': "If you're not registered yet, please contact your system administrator."
                },
                'ERROR': {
                    'GENERIC': '<h1>Generic Error...</h1>',
                    'NOT_FOUND_REPORT': "<h1>Ooooooops...<br/><br/><small><p>The Confirmation Report n.<b>{{reportNumber}}</b> was NOT found.</p><p>Are you sure did you put the correct number?</p></small></h1>",
                    'EXCEPTION': "<h1>Houston, we have a problem!<br/><small>Explain to your system admin what did you have done and (maybe) it could be solved.</small></h1>"
                },
                'REPORT': {
                    'CONFIRMATION_REPORT': 'Confirmation Report',
                    'NUMBER': 'N.',
                    'NUMBET_TOOLTIP': 'Temporary number, it will be confirmed at the first saving.',
                    'AT_DATE': 'At:',
                    'CUSTOMER': 'Customer',
                    'CUSTOMER_REP': 'Customer Representative',
                    'OWNERNAME': 'Reference Operator',
                    'OTHER_OPERATORS': 'Other operators involved',
                    'INTERVENTION_MODE': 'Intervention Mode',
                    'INTERVENTION_MODE_ONSITE': 'On Site',
                    'INTERVENTION_MODE_PHONE': 'By Phone',
                    'INTERVENTION_MODE_LOCAL': 'Local',
                    'INTERVENTION_MODE_OTHER': 'Other : ',
                    'INTERVENTION_MODE_OTHER_PO': 'Other intervention mode',
                    'DETAIL_DATE': 'Date',
                    'DETAIL_FROM_TIME': 'From',
                    'DETAIL_TO_TIME': 'To',
                    'DETAIL_DESCRIPTION': 'Intervention Description',
                    'DETAIL_DELETE_CONFIRM': 'Are you sure you want to delete the row?',
                    'DETAIL_NEW': 'New detail',
                    'NOTES': 'Notes',
                    'SAVE_BUTTON': 'Save Draft',
                    'SAVE_COMPLETE_PRINT_BUTTON': 'Save final and Print',
                    'PRINT_BUTTON': 'Print',
                    'DRAFT': 'Draft'
                },
                'LIST': {
                    'TITLE': "List of {{listType}} Confirmation Reports",
                    'TITLE_DRAFT': 'DRAFT',
                    'DRAFT': 'Draft',
                    'ROW_TITLE': 'Confirmation Report n.{{ReportNumber}} at {{ReportDate}} - {{CustomerName}}',
                    'ROW_SUBTITLE': '{{InterventionMode}}, in charge of: {{OwnerCompleteName}}',
                    'NO_ROWS': 'No Confirmation Report found.',
                    'MODE_ON_SITE': 'On Site intervention',
                    'MODE_LOCAL': 'Local intervention',
                    'MODE_PHONE': 'Intervention by Phone',
                    'MODE_OTHER': 'Other Mode Intervention'
                }
            });

            var preferredLanguage = localStorage.getItem(ConfirmationReportSettings.language) || 'it_IT';
            $translateProvider.preferredLanguage(preferredLanguage);
            if (localStorage.getItem(ConfirmationReportSettings.language) == null) {                
                localStorage.setItem(ConfirmationReportSettings.language, preferredLanguage);
            }
        }
    ])