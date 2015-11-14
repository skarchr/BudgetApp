(function() {
    'use strict';


    angular.module('budgetApp')
        .directive('getChartView', ['$http', '$timeout', function($http, $timeout) {
            return {
                restrict: 'A',
                scope:{},
                link: function(scope, elem) {


                    $(elem[0]).on('click', function () {
                        console.log(scope.$parent.chartModel);
                        $http({
                            method: 'POST',
                            data: JSON.stringify(scope.$parent.chartModel),
                            url: '/Report/CreateChart',
                            contentType: 'application/json; charset=utf-8',

                        }).then(function successCallback(response) {
                            $('#chartView').html(response.data);
                        }, function errorCallback(response) {
                            console.log('Something went wrong');
                        });
                    });

                }
            };
        }]);

})();
