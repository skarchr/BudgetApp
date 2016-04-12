(function() {
    'use strict';

    angular.module('budgetApp').controller('transactionController', ['$scope', 'transactionModel', 'commonService', function ($scope, transactionModel, commonService) {

        $scope.getIcon = function (input) {

            return commonService.getIconUrl(input);

        };
        $scope.predicate = 'date';
        $scope.reverse = true;

        $scope.model = transactionModel.model;

        $scope.urls = transactionModel.urls;

        $scope.today = new Date();

        $scope.findMax = function(list) {

            var array = [];


            var index = 0;

            for (var i = 0; i < list.length; i++) {

                if (list[i].category !== 'Salary' && list[i].category !== 'OtherIncome' ) {
                    array[index] = list[i].amount;
                    index++;
                }
            }

            return Math.max.apply(Math, array);

        };

        $scope.calcRowPercentage = function (target, other) {

            var onePercent = (other + target) / 100;

            var result = target / onePercent;

            return result.toFixed(0) * 10;

        };

        $scope.calcPercentage = function(target, other) {

            var onePercent = (other + target)  / 100;

            var result = target / onePercent;

            return 'width:'+result.toFixed(0)+'%';

        };

        $scope.currentPage = 1;

        $scope.numPerPage = 50;

        $scope.maxSize = 8;
    }]);

    angular.module('budgetApp').controller('addTransactionController', ['$scope', 'addTransactionModel', function ($scope, addTransactionModel) {

        $scope.model = addTransactionModel.model;

        $scope.opened = false;

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened = true;
        };

        $scope.today = new Date();

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        $scope.regexDouble = /^(\d+(?:[\.]\d{1,2})?)$/;

    }]);

    angular.module('budgetApp').controller('editTransactionController', ['$scope', 'editTransactionModel', function ($scope, editTransactionModel) {

        $scope.model = editTransactionModel.model;

        $scope.model.date = new Date(editTransactionModel.model.date);

        $scope.regexDouble = /^(\d+(?:[\.]\d{1,2})?)$/;

    }]);

    angular.module('budgetApp').controller('deleteTransactionController', ['$scope', 'deleteTransactionModel', 'commonService', function ($scope, deleteTransactionModel, commonService) {

        $scope.model = deleteTransactionModel.model;

        $scope.getIcon = function (input) {

            return commonService.getIconUrl(input);

        };

    }]);


})();