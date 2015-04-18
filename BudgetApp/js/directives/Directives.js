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
                    highchart: '@'
                },
                link: function(scope, elem, attrs) {

                    var model = JSON.parse(scope.highchart);

                    $(elem[0]).highcharts({
                        chart: {
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false
                        },
                        title: {
                            text: model.title.text,
                            style: {
                                fontSize: '14px',
                                color:'#777'
                            }
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
                            type: 'category',
                            labels: {
                                //formatter:function(){ return image based on this.value }
                                rotation: -45
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'Amount'
                            }
                        },
                        series: model.series,
                        drilldown : model.drilldown
                    });

                    setTimeout(function () {
                        $(elem[0]).highcharts().reflow();
                    }, 350);
                }
            }
        })
        .directive('gauge', function() {

            return {
                restrict: 'A',
                scope: {
                    income: '@',
                    expense: '@'
                },
                link: function(scope, elem) {
                    var gaugeOptions = {

                        chart: {
                            type: 'solidgauge'
                        },
                        credits:false,
                        title: null,

                        pane: {
                            center: ['50%', '85%'],
                            size: '120%',
                            startAngle: -90,
                            endAngle: 90,
                            background: {
                                backgroundColor: '#90ed7d',
                                innerRadius: '60%',
                                outerRadius: '100%',
                                shape: 'arc'
                            }
                        },

                        tooltip: {
                            enabled: false
                        },

                        // the value axis
                        yAxis: {
                            stops: [
                                [0.9, '#DF5353'] // red
                            ],
                            lineWidth: 0,
                            minorTickInterval: null,
                            tickPixelInterval: 400,
                            tickWidth: 0,
                            title: {
                                y: -70
                            },
                            labels: {
                                enabled:false,
                                y: 16
                            },
                            plotLines: [{
                                value: 50,
                                width: 1,
                                color: '#777',
                                zIndex: 100,
                                dashStyle:'dash'
                            }]
                        },

                        plotOptions: {
                            solidgauge: {
                                dataLabels: {
                                    y: 5,
                                    borderWidth: 0,
                                    useHTML: true
                                }
                            }
                        }
                    };

                    var onePercent = ((parseFloat(scope.income) + parseFloat(scope.expense)) / 100);

                    var exp = parseFloat(scope.expense) / onePercent;
                    var inc = parseFloat(scope.income) / onePercent;
                    var tot = (parseFloat(scope.income) - parseFloat(scope.expense)).toFixed(1);

                    $(elem[0]).highcharts(Highcharts.merge(gaugeOptions, {
                        yAxis: {
                            min: 0,
                            max: 100,
                            title: {
                                y: -140,
                                x:-6,
                                text: 'Expenses vs Income  ',
                                style: {
                                    fontSize: '14px'
                                }
                            }
                        },

                        series: [{
                            name: ' ',
                            data: [exp],
                            dataLabels: {
                                format: '<div style="text-align:center; background:white; z-index:110" >' +
                                            '<span style="font-size:22px;color:black" title="Expenses:   ' + parseFloat(scope.expense).toFixed(1) + '\nIncome:   ' + parseFloat(scope.income).toFixed(1) + '"> ' +
                                                + tot +
                                            '</span><br/>' +
                                            '<span style="font-size:14px;color:#777"> ' +
                                                + exp.toFixed(1) +'%  / '+ inc.toFixed(1) +
                                            '%</span><br/>' +
                                        '</div>'
                            }
                        }]

                    }));

                    setTimeout(function() {
                        $(elem[0]).highcharts().reflow();
                    }, 400);

                    
                }
            }

        });

})();