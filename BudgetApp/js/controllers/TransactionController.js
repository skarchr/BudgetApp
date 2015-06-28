(function() {
    'use strict';

    angular.module('budgetApp').controller('transactionController', ['$scope', 'transactionModel', 'commonService', function ($scope, transactionModel, commonService) {

        $scope.filter = transactionModel.filter;

        $scope.rangeElem = '';
        
        $scope.querystring = {};

        $scope.getIcon = function (input) {

            return commonService.getIconUrl(input);

        };
        $scope.predicate = 'amount';
        $scope.reverse = true;

        $scope.model = transactionModel.model;

        $scope.urls = transactionModel.urls;

        $scope.overviewGraph = transactionModel.overview;

        $scope.showBalance = false;

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

    }]);

    angular.module('budgetApp').controller('addTransactionController', ['$scope', 'addTransactionModel', function ($scope, addTransactionModel) {

        $scope.model = addTransactionModel.model;

        $scope.regexDouble = /^(\d+(?:[\.\,]\d{1,2})?)$/;

    }]);

    angular.module('budgetApp').controller('editTransactionController', ['$scope', 'editTransactionModel', function ($scope, editTransactionModel) {

        $scope.model = editTransactionModel.model;

        $scope.model.date = new Date(editTransactionModel.model.date);


    }]);

    angular.module('budgetApp').controller('deleteTransactionController', ['$scope', 'deleteTransactionModel', 'commonService', function ($scope, deleteTransactionModel, commonService) {

        $scope.model = deleteTransactionModel.model;

        $scope.getIcon = function (input) {

            return commonService.getIconUrl(input);

        };

    }]);


})();