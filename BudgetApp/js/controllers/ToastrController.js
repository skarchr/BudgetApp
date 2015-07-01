(function() {
    'use strict';

    angular.module('budgetApp').controller('toastrController', ['$scope', 'viewBags', 'toastr', function($scope, viewBags, toastr) {

        var viewbagList = viewBags.messages;

        $.each(viewbagList, function(i, val) {

            if(val !== ''){
                switch (i) {
                case 'success':
                    toastr.success(val,{ timeOut: 7000 });
                    break;
                case 'error':
                    toastr.error(val, { timeOut: 30000, closeButton: true });
                    break;
                case 'info':
                    toastr.info(val);
                    break;
                case 'statusMessage':

                    if ('An error has occurred.' === val) {
                        toastr.error(val, { timeOut: 30000, closeButton: true });
                    } else if ('Check your email and confirm your account.' === val) {
                        toastr.info(val);
                    } else {
                        toastr.success(val, { timeOut: 7000 });
                    }
                    break;
                default:
                    break;
                }
            }

        });

    }]);

})();