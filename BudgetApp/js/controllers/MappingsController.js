(function() {
    'use strict';

    angular.module('budgetApp').controller('mappingsController', ['$scope', '$window', 'mappingModel', 'commonService', function ($scope, $window, mappingModel, commonService) {

        $scope.model = mappingModel.model;
        $scope.urls = mappingModel.urls;

        $scope.go = function (path) {
            if (path === 'All') {
                $window.location.href = 'Mappings/Create';
            } else {
                $window.location.href = 'Mappings/Create?mapping=' + path;
            }            
        };

        $scope.filteredCategory = '';

        $scope.getIcon = function(input) {

            return commonService.getIconUrl(input);

        };

        $scope.categories = mappingModel.categories;

        $scope.setCategory = function(category) {
            $scope.filteredCategory = category;
        };

        $scope.countCategory = function(category) {

            var count = 0;

            for (var i = 0; i < mappingModel.model.length; i++) {
                
                if (mappingModel.model[i].category.trim() === category) {
                    count += 1;
                }
            }
            return count;

        };

    }]);

    angular.module('budgetApp').controller('addMappingController', ['$scope', 'addMappingModel', 'commonService', function ($scope, addMappingModel, commonService) {

        $scope.model = addMappingModel.model;

        $scope.getIcon = function (input) {

            return commonService.getIconUrl(input);

        };

    }]);

    angular.module('budgetApp').controller('deleteMappingController', ['$scope', 'commonService', 'deleteMappingModel', function ($scope, commonService, deleteMappingModel) {

        $scope.model = deleteMappingModel.model;

        $scope.getIcon = function (input) {

            return commonService.getIconUrl(input);

        };

    }]);

    angular.module('budgetApp').controller('editMappingController', ['$scope', 'editMappingModel', 'commonService', function ($scope, editMappingModel, commonService) {

        $scope.model = editMappingModel.model;

        $scope.getIcon = function (input) {

            return commonService.getIconUrl(input);

        };

    }]);

})();