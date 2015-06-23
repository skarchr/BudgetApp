(function() {
    'use strict';

    angular.module('budgetApp').controller('homeController', ['$scope', 'homeViewModel', function ($scope, homeViewModel) {

        $scope.model = homeViewModel.model;

        $scope.starCountPos = 0;

        function starCounterPos() {

            var result = 0;

            for (var i = 0; i < homeViewModel.model.savingGoals.length; i++) {
                
                if (homeViewModel.model.savingGoals[i].achieved) {
                    result++;
                }

            }

            $scope.starCountPos = result;

        }

        starCounterPos();

        $scope.starCountNeg = 0;

        function starCounterNeg() {

            var result = 0;

            for (var i = 0; i < homeViewModel.model.savingGoals.length; i++) {

                if (!homeViewModel.model.savingGoals[i].achieved) {
                    result++;
                }

            }

            $scope.starCountNeg = result;

        }

        starCounterNeg();


    }]);

})();