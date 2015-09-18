(function() {
    'use strict';

    angular.module('budgetApp').controller('adminController', ['$scope', 'adminModel', function ($scope, adminModel) {

        $scope.usernameSelected = 'testtest';

        $scope.urls = adminModel.urls;

    }]);

})();