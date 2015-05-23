(function() {
    'use strict';

    angular.module('budgetApp').controller('mappingsController', ['$scope', 'mappingModel', 'commonService', function ($scope, mappingModel, commonService) {

        $scope.model = mappingModel.model;
        $scope.urls = mappingModel.urls;

        $scope.getIcon = function(input) {

            return commonService.getIconUrl(input);

        };

        $scope.categories = mappingModel.categories;

        $scope.countCategory = function(category) {

            var count = 0;

            var cat1 = category.replace(/([A-Z])/g, " $1");

            var cat2  = (cat1.charAt(0).toUpperCase() + cat1.slice(1)).toString();

            for (var i = 0; i < mappingModel.model.length; i++) {
                //TODO: Fix counter
                if (mappingModel.model[i].category.toString() == cat2) {
                    count += 1;
                    console.log('++');
                }
            }
            return count;

        };

    }]);

})();