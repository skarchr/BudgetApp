(function() {
    'use strict';

    angular.module('budgetApp').controller('toastrController', ['$scope', 'viewBags', 'toastr', function($scope, viewBags, toastr) {

        var viewbagList = viewBags.messages;

        $.each(viewbagList, function(i, val) {

            if(val !== ''){
                switch (i) {
                case 'success':
                    toastr.success(val);
                    break;
                case 'error':
                    toastr.error(val);
                    break;
                case 'info':
                    toastr.info(val);
                    break;
                default:
                    return false;
                }
            }
            return false;
        });

    }]);

})();