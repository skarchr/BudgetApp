(function() {
    'use strict';

    angular.module('budgetApp').controller('sidebarController', ['$scope', '$http', function ($scope, $http) {

        $scope.shrink = false;

        
            $http({ method: 'GET', url: '/Home/GetShrinkStatus' })
            .success(function (response) {
                var result = false;
                
                if (response === 'True') {
                    result = true;                    
                }
                $scope.shrink = result;
            });
        


        $scope.toggleShrinkStatus = function () {

            $scope.shrink = !$scope.shrink;

            $http({ method: 'POST', url: '/Home/SaveShrinkStatus' })
            .success(function (response) {
                console.log('saved');
            }).error(function(response) {
                console.log('error');
            });
        };


    }]);

})();