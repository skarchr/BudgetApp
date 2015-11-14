(function () {
    'use strict';

    angular.module('budgetApp').controller('reportController', ['$scope', 'reportFactory', function ($scope, reportFactory) {

        $scope.disableButton = false;

        $scope.selected = 'charts';

        $scope.chartModel = {
            name: reportFactory.model.name,
            fromDate: new Date( Date.parse(reportFactory.model.fromDate) ),
            toDate: new Date(Date.parse(reportFactory.model.toDate)),
            range: reportFactory.model.range,
            categories: reportFactory.model.categories,
            isDrilldown: false,
            chartType: 'column',
            byYear: reportFactory.model.byYear
        };

        $scope.toggleCategory = function(category) {

            console.log(category);

            var index = $scope.chartModel.categories.indexOf(category);

            if (index > -1) {
                $scope.chartModel.categories.splice(index, 1);
            } else {
                $scope.chartModel.categories.push(category);
            }
        };

        $scope.today = new Date();

        $scope.tableModel = {};

    }]);

})();