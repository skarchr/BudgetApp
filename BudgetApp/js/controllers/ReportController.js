(function () {
    'use strict';

    angular.module('budgetApp').controller('reportController', ['$scope', 'reportFactory', function ($scope, reportFactory) {

        $scope.disableButton = false;

        $scope.selected = 'charts';

        $scope.chartModel = {
            name: '',
            fromDate: new Date( Date.parse(reportFactory.model.fromDate) ),
            toDate: new Date(Date.parse(reportFactory.model.toDate)),
            range: reportFactory.model.range,
            categories: [],
            isDrilldown: false,
            chartType: 'column'
        };

        $scope.today = new Date();

        $scope.tableModel = {};

    }]);

})();