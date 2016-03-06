(function() {
    'use strict';

    var formatNumber = function (number) {
        var numb = number.toFixed(2) + '';
        var x = numb.split(' ');
        var x1 = x[0];
        var x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ' ' + '$2');
        }
        return x1 + x2;
    };

    var formatCamelText = function (input) {

        var source = input.replace(/([A-Z])/g, " $1");


        return (source.charAt(0).toUpperCase() + source.slice(1)).trim();
    }

    angular.module('budgetApp')


        .directive('balanceReload', function() {
            return {
                restrict: 'A',
                ngModel: 'required',
                scope: {
                    model:'=ngModel'
                },
                link:function(scope, elem) {

                    var renderGraph = function(model) {

                        var series = model.series;                        

                        for (var i = 0; i < model.series.length; i++) {
                            series[i].dataGrouping.units = [['month', [1]]];
                            series[i].turboThreshold = 10000;
                        }

                        $(elem[0]).highcharts('StockChart', {
			
                            chart: {
                                style: {
                                    fontFamily: "Tahoma, Geneva, sans-serif !important"
                                },
                                width: 380,
                                height: 300
                            },
                            rangeSelector : {
                                enabled : false
                            },
                            legend: {
                                enabled:true
                            },
                            credits:false,
                            title : model.title,
                            series: series
                        });

                    };

                    renderGraph(scope.model);


                    scope.$watch('model', function (nv) {
                        renderGraph(nv);
                    });
                }
            };
        })

        .directive('highchartReload', [
            'commonService', function (commonService) {

                return {
                    restrict: 'A',
                    ngModel:'required',
                    scope: {
                        model: '=ngModel'
                    },
                    link: function (scope, elem, attrs) {

                        var renderGraph = function(model) {
                            Highcharts.setOptions({ lang: { drillUpText: 'Back' } });

                            $(elem[0]).highcharts({
                                chart: {
                                    style: {
                                        fontFamily: "Tahoma, Geneva, sans-serif !important"
                                    },
                                    width: 380,
                                    height: 300,
                                    events: {
                                        drilldown: function (e) {
                                            this.setTitle({ text: e.point.name });
                                        },
                                        drillup: function (e) {
                                            this.setTitle({ text: model.title.text });
                                        }
                                    }
                                },
                                title: model.title,
                                tooltip: {
                                    enabled: false,
                                    pointFormat: '{series.name}: <b>{point.y:.1f} </b>'
                                },
                                legend: {
                                    enabled: false,
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
                                        useHTML: true,
                                        formatter: function () {


                                            var textLabel = '<span class="hidden-sm">' + formatCamelText(this.value) + '</span>';

                                            var img = '<img class="hs-image-label" title="' + formatCamelText(this.value) + '" src="' + commonService.getIconUrl(this.value) + '"/>';

                                            var style = model.type === 'bar' ? 'min-height:50px;min-width:80px;' : 'min-height:50px;';

                                            return '<div class="text-center" style = "' + style + '">' + img + '<br>' + textLabel + '</div>';
                                        }
                                        //rotation: -45
                                    }
                                },
                                yAxis: {
                                    title: {
                                        text: ' '
                                    },
                                    labels: {
                                        align: 'left'
                                    }

                                },
                                series: model.series,
                                drilldown: {
                                    series: model.drilldown !== null ? model.drilldown.series : null,
                                    drillUpButton: {
                                        position: { y: -20 },
                                        theme: {
                                            fill: 'white',
                                            'stroke-width': 1,
                                            stroke: 'silver',
                                            heigth: 50,
                                            r: 4,
                                            states: {
                                                hover: {
                                                    fill: '#cccccc',
                                                    stroke: '#adadad !important',
                                                },
                                                select: {
                                                    stroke: '#adadad',
                                                    fill: '#bada55'
                                                }
                                            }
                                        }
                                    }

                                }
                            });

                            var chart = $(elem[0]).highcharts();

                            if (scope.chartLabel !== undefined) {

                                var text = formatNumber(JSON.parse(scope.chartLabel));

                                var currency = scope.currency !== undefined ? ' ' + scope.currency : '';

                                chart.renderer.label('Total: ' + text + currency, chart.chartWidth - text.length * 18, chart.chartHeight - text.length * 10, 'callout', 1)
                                    .css({
                                        color: '#FFFFFF'
                                    })
                                    .attr({
                                        fill: '#E87C7C',
                                        padding: 10,
                                        r: 4,
                                        zIndex: 6
                                    })
                                    .add();
                            }
                        }

                        renderGraph(scope.model);

                        scope.$watch('model', function(nv) {
                            renderGraph(nv);
                        });

                    }
                }
            }
        ])

        .directive('treemapReload', [
            function() {

                return {
                    restrict: 'A',
                    require: 'ngModel',
                    scope: {
                        model: '=ngModel'
                    },
                    link: function(scope, elem) {

                        var renderGraph = function(model) {
                            
                            Highcharts.setOptions({ lang: { drillUpText: 'Back' } });

                            var total = 0;

                            for (var i = 0; i < model.length; i++) {

                                if (model[i].parent === undefined)
                                    total += model[i].value;
                            }

                            $(elem[0]).highcharts({
                                chart: {
                                    width:390,
                                    height:300,
                                    style: {
                                        fontFamily: "Tahoma, Geneva, sans-serif !important"
                                    },
                                    events: {
                                        redraw: function () {

                                            if (this.series[0].rootNode === '') {
                                                $(elem[0]).highcharts().setTitle({ text: 'Expenses' }, { text: formatNumber(total) }, false);
                                            }

                                        }
                                    }
                                },
                                credits: false,
                                series: [
                                    {
                                        events: {
                                            click: function (e) {
                                                if (e.point.parent === undefined)
                                                    $(elem[0]).highcharts().setTitle({ text: e.point.name }, { text: formatNumber(e.point.value) }, false);
                                            }
                                        },
                                        type: 'treemap',
                                        drillUpButton: {
                                            position: { y: -30, x: -10, align: 'right' },
                                            theme: {
                                                fill: 'white',
                                                'stroke-width': 1,
                                                stroke: 'silver',
                                                heigth: 50,
                                                r: 4,
                                                states: {
                                                    hover: {
                                                        fill: '#cccccc',
                                                        stroke: '#adadad !important',
                                                    },
                                                    select: {
                                                        stroke: '#adadad',
                                                        fill: '#bada55'
                                                    }
                                                }
                                            }
                                        },
                                        layoutAlgorithm: 'squarified',
                                        allowDrillToNode: true,
                                        dataLabels: {
                                            enabled: false
                                        },
                                        levelIsConstant: false,
                                        levels: [
                                            {
                                                level: 1,
                                                dataLabels: {
                                                    enabled: true
                                                },
                                                borderWidth: 3
                                            }
                                        ],
                                        data: model
                                    }
                                ],
                                plotOptions: {
                                    treemap: {
                                        borderColor: '#fff',
                                        dataLabels: {
                                            useHTML: true,
                                            align: 'center',
                                            formatter: function () {
                                                if (this.point.shapeArgs.width < 20 || this.point.shapeArgs.height < 20) {
                                                    return '';

                                                } else if (this.point.shapeArgs.width < 50 || this.point.shapeArgs.height < 50) {
                                                    return '<div style="text-align:center;"><span>' + this.key + '</span></div>';
                                                } else {
                                                    return '<div style="text-align:center;"><span>' + this.key + '</span><br/><span style="font-size:10px;">' + formatNumber(this.point.value) + '</span></div>';
                                                }
                                            }
                                        }
                                    }
                                },
                                tooltip: {
                                    enabled: true,
                                    followPointer: true,
                                    useHTML: true,
                                    pointFormat: '<span style="z-index:9998 !important;"><span style="color:{point.color}">\u25CF</span> {point.name}: <b>{point.value:.1f}</b><br/></span>'
                                },
                                title: {
                                    text: 'Expenses'
                                },
                                subtitle: {
                                    text: formatNumber(total)
                                }
                            });
                        };

                        renderGraph(scope.model);                        

                        scope.$watch('model', function (nv, ov) {
                            renderGraph(nv);
                        });

                    }
                };

            }
        ]);

})();