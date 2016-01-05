(function() {
    'use strict';

    angular.module('budgetApp', ['ui.bootstrap', 'ngAnimate', 'ui.gravatar', 'toastr'])
        .config(function (toastrConfig) {
            angular.extend(toastrConfig, {
                positionClass: 'toast-bottom-right'
            });
        });

})();