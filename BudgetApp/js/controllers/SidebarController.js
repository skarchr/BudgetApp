(function() {
    'use strict';

    angular.module('budgetApp').controller('sidebarController', ['$scope', '$http', 'sharedProperties', function ($scope, $http, sharedProperties) {


        $scope.selected = sharedProperties.getSelected();

        $scope.go = function (item) {
            $scope.selected = item;
            sharedProperties.setSelected(item);
        };

        //$scope.shrink = true;

        //$scope.toggleShrinkStatus = function () {
            
        //    $scope.shrink = !$scope.shrink;            

        //    setTimeout(function() {
        //        $(window).trigger('resize');
        //    }, 100);

        //};

    }]);

})();