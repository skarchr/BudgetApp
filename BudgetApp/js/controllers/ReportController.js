﻿(function () {
    'use strict';

    angular.module('budgetApp').controller('reportController', ['$scope', '$http', '$timeout', 'reportFactory', 'sharedProperties', function ($scope, $http, $timeout, reportFactory, sharedProperties) {

        $scope.disableButton = false;

        $scope.urls = reportFactory.urls;

        $scope.selected = sharedProperties.getSelected();

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

        $scope.$watch(function () {
            return sharedProperties.getSelected();
        }, function (nv, ol) {
            $scope.selected = nv;
        },true);

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
        $scope.format = $scope.formats[0];

        $scope.disabled = function (date, mode) {
            return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
        };

    }]);

})();