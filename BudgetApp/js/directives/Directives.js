(function() {
    'use strict';

    angular.module('budgetApp')
        .directive('logo', [
            function() {

                return {
                    restrict: 'A',
                    scope: {
                        logo: '@'
                    },
                    link: function(scope, elem) {

                        var to = scope.logo === undefined ? '30' : scope.logo;

                        $(elem[0]).css({
                            '-webkit-transition': 'all 0.5s ease-in-out',
                            '-moz-transition': 'all 0.5s ease-in-out',
                            '-o-transition': 'all 0.5s ease-in-out',
                            'transition': 'all 0.5s ease-in-out'
                        });

                        setTimeout(function() {
                            $(elem[0]).css({
                                'width': to + '%',
                                'position': 'inherit'
                            });

                        }, 100);

                    }
                };

            }
        ])
        .directive('slideDown', [
            function() {

                return {
                    restrict: 'A',
                    scope: {
                        slideDown: '@',
                        prevent: '@'
                    },
                    link: function(scope, elem) {

                        if (scope.prevent === "False") {
                            return false;
                        }

                        $(elem[0]).css({
                            'display': 'none',
                        });

                        $(elem[0])
                            .delay(600)
                            .velocity('slideDown', {
                                duration: 300
                            });
                    }
                };

            }
        ])
        .directive('fadeIn', [
            function() {

                return {
                    restrict: 'A',
                    scope: {
                        fadeIn: '@',
                        slideDown: '@',
                        prevent: '@'
                    },
                    link: function(scope, elem) {

                        if (scope.prevent === "False") {
                            return false;
                        }

                        $(elem[0]).css({
                            'display': 'none',
                        });

                        $(elem[0])
                            .delay(scope.fadeIn === undefined ? 600 : scope.fadeIn)
                            .velocity('fadeIn', {
                                duration: 300
                            });
                    }
                };

            }
        ])
    .directive('highchart', function() {
        return {
            restrict: 'A',
            scope: {
                highchart: '@',
                titletext: '@'
            },
            link: function(scope, elem, attrs) {

                $(elem[0]).highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: null,
                        plotShadow: false
                    },
                    title: {
                        text: scope.titletext
                    },
                    tooltip: {
                        pointFormat: '{series.name}: <b>{point.y:.1f} </b>'
                    },
                    legend: {
                        align: 'right',
                        layout: 'vertical',
                        y: -25
                    },
                    credits: false,
                    plotOptions: {
                        column: {
                            showInLegend: false,
                            colorByPoint: true
                        }
                    },
                    xAxis: {
                        categories: ['Fixed', 'Food', 'Income', 'Personal', 'Shelter', 'Transport'],
                        labels: {
                            rotation: -90
                        }
                    },
                    yAxis: {
                        title: {
                            text: 'Amount'
                        }
                    },
                    series: [
                        {
                            type: 'column',
                            name: 'Category',
                            data: JSON.parse(scope.highchart)
                        }
                    ]
                });
            }
        }
    });

})();