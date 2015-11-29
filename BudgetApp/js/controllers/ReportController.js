(function () {
    'use strict';

    angular.module('budgetApp').controller('reportController', ['$scope', '$http', '$timeout', 'reportFactory', function ($scope,$http,$timeout,reportFactory) {

        $scope.disableButton = false;

        $scope.urls = reportFactory.urls;

        $scope.selected = 'charts';

        $scope.searchResult = [];
        $scope.searchStorage = [];

        $scope.searching = false;

        var start = 0;
        var stop = 50;

        $scope.searchStorage = [];

        $scope.chartModel = {
            name: reportFactory.model.name,
            fromDate:  reportFactory.model.fromDate,
            toDate: reportFactory.model.toDate,
            range: reportFactory.model.range,
            categories: reportFactory.model.categories,
            isDrilldown: false,
            chartType: 'column',
            byYear: reportFactory.model.byYear,
            searchString : ''
        };

        $scope.search = function () {

            $scope.searchResult = [];
            $scope.searching = true;
            start = 0;
            stop = 50;

            $timeout(function() {
                
                $http({
                    method: 'POST',
                    data: JSON.stringify($scope.chartModel),
                    url: '/Report/SearchTransactions',
                    contentType: 'application/json; charset=utf-8',

                }).then(function successCallback(response) {

                    $scope.searchStorage = JSON.parse(response.data);

                    $scope.searchResult = $scope.searchStorage.slice(start, stop);

                    $scope.searching = false;

                    start += 50;
                    stop += 50;

                }, function errorCallback(response) {
                    console.log('Something went wrong');
                    $scope.searching = false;
                });

            }, 500);            
        };

        $scope.loadMore = function() {
            $scope.searchResult = $scope.searchResult.concat($scope.searchStorage.slice(start, stop));

            start += 50;
            stop += 50;
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

        $scope.opened = false;
        $scope.opened2 = false;

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened2 = false;
            $scope.opened = true;
        };

        $scope.open2 = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened = false;
            $scope.opened2 = true;
        };

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        $scope.formats = ['yyyy-MM-dd', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[2];

        $scope.disabled = function (date, mode) {
            return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
        };

    }]);

})();