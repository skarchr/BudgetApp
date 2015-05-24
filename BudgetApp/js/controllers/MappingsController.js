(function() {
    'use strict';

    angular.module('budgetApp').controller('mappingsController', ['$scope', 'mappingModel', 'commonService', function ($scope, mappingModel, commonService) {

        $scope.model = mappingModel.model;
        $scope.urls = mappingModel.urls;

        $scope.filteredCategory = '';

        $scope.getIcon = function(input) {

            return commonService.getIconUrl(input);

        };

        $scope.categories = mappingModel.categories;

        $scope.addMapping = function(category) {
            alert('Helloooo');
        };

        $scope.setCategory = function(category) {
            $scope.filteredCategory = category;
        };

        $scope.countCategory = function(category) {

            var count = 0;

            var cat1 = category.replace(/([A-Z])/g, " $1");

            var cat2  = (cat1.charAt(0).toUpperCase() + cat1.slice(1)).trim();

            for (var i = 0; i < mappingModel.model.length; i++) {

                if (mappingModel.model[i].category.trim() === cat2) {
                    count += 1;
                }
            }
            return count;

        };

    }]);

})();