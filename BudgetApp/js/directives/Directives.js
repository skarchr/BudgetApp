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


    angular.module('budgetApp')

        .directive('topNav', ['$window', function($window) {
            return {
                restrict: 'A',
                link : function(scope, elem) {

                    if ($window.scrollY < 20) {
                        $(elem[0]).addClass('no-scroll-top');
                        $(elem[0]).addClass('scroll-top');
                    }
                        

                    $($window).on('scroll', function () {

                        if (this.scrollY < 20) {
                            $(elem[0]).addClass('scroll-top');
                        } else {
                            $(elem[0]).removeClass('scroll-top');
                            $(elem[0]).addClass('no-scroll-top');
                        }

                    });

                }
            };
        }])

        .directive('setWidth', [
            function() {

                return {
                    restrict: 'A',
                    scope: {
                        setWidth: '@'
                    },
                    link: function(scope, elem) {
                        $(elem[0]).attr('width', scope.setWidth);
                    }
                }

            }
        ])
        .directive('setRange', [
            function() {
                return {
                    restrict: 'A',
                    scope: {
                        setRange: '@'
                    },
                    link: function(scope, elem, attrs) {

                        $(elem[0]).on('click', function() {

                            $('#CurrentPage').val(0);
                            $('#Range').val(scope.setRange);

                            $('#submitBtn').trigger('click');

                        });

                    }
                };
            }
        ])
        .directive('setPage', [
            function() {
                return {
                    restrict: 'A',
                    scope: {
                        setPage: '@'
                    },
                    link: function(scope, elem, attrs) {

                        $(elem[0]).on('click', function() {

                            $('#CurrentPage').val(scope.setPage);

                            $('#submitBtn').trigger('click');

                        });

                    }
                };
            }
        ])
        .directive('selectCategories', [
            function() {

                return {
                    restrict: 'A',
                    scope: {
                        selectCategories: '@'
                    },
                    require: 'ngModel',
                    template: '<select class="form-control" ng-model="selectCategories"><option value=""></option><optgroup label="Fixed" value="Fixed"><option value="DebtReduction">Debt reduction</option><option value="Dental">Dental</option><option value="Insurance">Insurance</option><option value="Medical">Medical</option><option value="OtherFixed">Other fixed</option></optgroup><optgroup label="Food" value="Food"><option value="Groceries">Groceries</option><option value="Restaurant">Restaurant</option><option value="Treats">Treats</option><option value="OtherFood">Other food</option></optgroup><optgroup label="Income" value="Income"><option value="Salary">Salary</option><option value="OtherIncome">Other income</option></optgroup><optgroup label="Personal" value="Personal"><option value="Appearance">Appearance</option><option value="Entertainment">Entertainment</option><option value="Gifts">Gifts</option><option value="Hobby">Hobby</option><option value="Phone">Phone</option><option value="Subscriptions">Subscriptions</option><option value="Travel">Travel</option><option value="OtherPersonal">Other personal</option></optgroup><optgroup label="Saving" value="Saving"><option value="Saving">Saving</option></optgroup><optgroup label="Shelter" value="Shelter"><option value="Furniture">Furniture</option><option value="Interior">Interior</option><option value="Mortgage">Mortgage</option><option value="Rent">Rent</option><option value="Utilities">Utilities</option><option value="OtherShelter">Other shelter</option></optgroup><optgroup label="Transport" value="Transport"><option value="Car">Car</option><option value="CollectiveTransport">Collective transport</option><option value="OtherTransportation">Other transport</option></optgroup></select>'
                };

            }
        ])
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
        .directive('overviewHighchart', [
            function() {

                return {
                    restrict: 'A',
                    scope: {
                        overviewHighchart: '@',
                        range: '@',
                        id: '@',
                        totalIncome: '@',
                        totalExpenses: '@',
                        currency:'@'
                    },
                    link: function(scope, elem, attrs) {

                        var model = JSON.parse(scope.overviewHighchart);

                        Highcharts.setOptions({ lang: { drillUpText: 'Back' } });

                        $(elem[0]).highcharts({
                            chart: {
                                style: {
                                    fontFamily: "'Franklin Gothic Medium', 'Franklin Gothic', 'ITC Franklin Gothic', Arial, sans-serif !important"
                                },
                                type: 'column',
                                plotBackgroundColor: null,
                                plotBorderWidth: null,
                                plotShadow: false,
                            },
                            title: {
                                text: ' ',
                                style: {
                                    fontSize: '18px',
                                    color: '#313131'
                                }
                            },
                            tooltip: {
                                enabled: true,
                                formatter: function() {
                                    return '<div><strong>' + this.series.name + '</strong><br>' + this.x + ': ' + formatNumber(this.y) + ' ' + model.currency + '</div>';
                                }
                            },
                            legend: {
                                enabled: true,
                                verticalAlign: 'top',
                                align: 'right',
                                layout: 'vertical',
                                floating: true,
                                y: 20
                            },
                            credits: false,
                            plotOptions: {
                                column: {
                                    showInLegend: true
                                }
                            },
                            xAxis: {
                                categories: model.categories,
                                plotLines: scope.range !== 'Annual' ? model.plotLinesX : null,
                                type: 'category',
                                title: {
                                    text: scope.range !== undefined && scope.range === 'Week' ? scope.range : ''
                                },
                                labels: {
                                    useHTML: true,
                                    formatter: function() {
                                        return '<div class="chart-btn">' + this.value + '</div>';
                                    }
                                }
                            },
                            yAxis: {
                                title: {
                                    text: model.currency
                                },
                                plotLines: scope.range === 'Month' || scope.range === 'Annual' ? model.plotLinesY : null
                            },
                            series: model.series
                        });

                        $('#' + scope.id + ' .highcharts-container > .highcharts-axis-labels > span').each(function(e) {

                            var that = this;

                            $(this).on('click', function() {

                                var active = $(this).hasClass('active');

                                $('.highcharts-axis-labels > span').each(function() {
                                    
                                    $(this).removeClass('active');
                                });

                                if (!active) {
                                    $(this).addClass('active');

                                    scope.$apply(function() {
                                        scope.$parent.rangeElem = e;
                                    });

                                } else {

                                    scope.$apply(function() {
                                        scope.$parent.rangeElem = '';
                                    });
                                }


                            });

                        });

                        var chart = $(elem[0]).highcharts();
                        var currency = scope.currency !== undefined ? ' ' + scope.currency : '';

                        if (scope.totalIncome !== undefined) {

                            var text = formatNumber(JSON.parse(scope.totalIncome.replace(',','.')));

                            chart.renderer.label('Total: ' + text + currency, 70, 10, 'callout', 1)
                                .css({
                                    color: '#FFFFFF'
                                })
                                .attr({
                                    fill: 'rgba(72, 221, 184,0.70)',//'rgba(255, 0, 0, 0.90)',
                                    padding: 2,
                                    r: 4,
                                    zIndex: 6
                                })
                                .add();
                        }

                        if (scope.totalExpenses !== undefined) {

                            var text1 = formatNumber(JSON.parse(scope.totalExpenses.replace(',', '.')));

                            chart.renderer.label('Total: ' + text1 + currency, 70, 32, 'callout', 1)
                                .css({
                                    color: '#FFFFFF'
                                })
                                .attr({
                                    fill: 'rgba(255, 0, 0, 0.70)',
                                    stroke:'rgb(0,0,0)',
                                    padding: 2,
                                    r: 4,
                                    zIndex: 6
                                })
                                .add();
                        }

                    }
                }
            }
        ])
        .directive('highchart', [
            'commonService', function(commonService) {

                return {
                    restrict: 'A',
                    scope: {
                        highchart: '@',
                        chartLabel: '@',
                        currency: '@'
                    },
                    link: function(scope, elem, attrs) {

                        var model = JSON.parse(scope.highchart);

                        Highcharts.setOptions({ lang: { drillUpText: 'Back' } });

                        $(elem[0]).highcharts({
                            chart: {
                                style: {
                                    fontFamily: "'Franklin Gothic Medium', 'Franklin Gothic', 'ITC Franklin Gothic', Arial, sans-serif !important"
                                },
                                plotBackgroundColor: null,
                                plotBorderWidth: null,
                                plotShadow: false,
                                events: {
                                    drilldown: function(e) {
                                        this.setTitle({ text: e.point.name });
                                    },
                                    drillup: function(e) {
                                        this.setTitle({ text: model.title.text });
                                    }
                                }
                            },
                            title: {
                                text: ' ',
                                style: {
                                    fontSize: '18px',
                                    color: '#313131'
                                }
                            },
                            tooltip: {
                                enabled: false,
                                pointFormat: '{series.name}: <b>{point.y:.1f} </b>'
                            },
                            legend: {
                                enabled:false,
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
                                        var input = this.value.replace(/([A-Z])/g, " $1");

                                        var textLabel = '<span class="hidden-sm">' + (input.charAt(0).toUpperCase() + input.slice(1)).trim() + '</span>';

                                        var img = '<img class="hs-image-label" title="' + (input.charAt(0).toUpperCase() + input.slice(1)).trim() + '" src="' + commonService.getIconUrl(this.value) + '"/>';

                                        var style = model.type === 'bar' ? 'min-height:50px;min-width:80px;' : 'min-height:50px;';

                                        return '<div class="text-center" style = "' + style + '">' + img + '<br>' + textLabel + '</div>';
                                    }
                                    //rotation: -45
                                }
                            },
                            yAxis: {
                                title: {
                                    text: model.currency
                                }
                            },
                            series: model.series,
                            drilldown: {
                                series: model.drilldown !== null ? model.drilldown.series : null,
                                drillUpButton: {
                                    position: { y: -50 },
                                    theme: {
                                        fill: 'white',
                                        'stroke-width': 1,
                                        stroke: 'silver',
                                        r: 0,
                                        states: {
                                            hover: {
                                                fill: '#cccccc'
                                            },
                                            select: {
                                                stroke: '#039',
                                                fill: '#bada55'
                                            }
                                        }
                                    }
                                }

                            }
                        });

                        var chart = $(elem[0]).highcharts();

                        setTimeout(function() {
                            chart.reflow();
                        }, 350);

                        if (scope.chartLabel !== undefined) {

                            var text = formatNumber(JSON.parse(scope.chartLabel));

                            var currency = scope.currency !== undefined ? ' ' + scope.currency : '';

                            chart.renderer.label('Total: ' + text + currency, chart.chartWidth - text.length * 18, chart.chartHeight - text.length * 10, 'callout', 1)
                                .css({
                                    color: '#FFFFFF'
                                })
                                .attr({
                                    fill: 'rgba(255, 0, 0, 0.90)',
                                    padding: 10,
                                    r: 4,
                                    zIndex: 6
                                })
                                .add();
                        }

                        
                    }
                }
            }
        ])
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
                        credits: false,
                        title: null,

                        pane: {
                            center: ['50%', '85%'],
                            size: '100%',
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
                                enabled: false,
                                y: 16
                            },
                            plotLines: [
                                {
                                    value: 50,
                                    width: 1,
                                    color: '#777',
                                    zIndex: 100,
                                    dashStyle: 'dash'
                                }
                            ]
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
                                y: -181,
                                x: 0,
                                text: 'Balance',
                                style: {
                                    fontSize: '18px',
                                    color: '#313131'
                                }
                            }
                        },

                        series: [
                            {
                                name: ' ',
                                data: [exp],
                                dataLabels: {
                                    y: -55,
                                    format: '<div style="text-align:center; background:white; z-index:110" >' +
                                        '<span style="font-size:22px;color:black" title="Expenses:   ' + parseFloat(scope.expense).toFixed(1) + '\nIncome:   ' + parseFloat(scope.income).toFixed(1) + '"> ' +
                                        + tot +
                                        '</span><br/>' +
                                        '<span style="font-size:14px;color:#777"> ' +
                                        + exp.toFixed(1) + '%  / ' + inc.toFixed(1) +
                                        '%</span><br/>' +
                                        '</div>'
                                }
                            }
                        ]

                    }));

                    setTimeout(function() {
                        $(elem[0]).highcharts().reflow();
                    }, 500);


                }
            }

        })
        .directive('highstock', function() {
            return {
                restrict: 'A',
                scope: {
                    highstock: '@'
                },
                link: function(scope, elem, attrs) {

                    var model = JSON.parse(scope.highstock);

                    $(elem[0]).highcharts('StockChart', {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: model.title.text
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
                            ordinal: false,
                            type: 'datetime'
                        },
                        yAxis: {
                            opposite: false,
                            title: {
                                text: model.currency
                            }
                        },
                        series: model.series
                    });
                }
            };
        })
        .directive('sortableTableHeader', function() {
            return {
                restrict: 'A',
                transclude: true,
                scope: {
                    sortableTableHeader: '@'
                },
                template: '<span style="cursor:pointer;" ng-transclude></span><i class="glyphicon glyphicon-sort-by-alphabet-alt" style="" ng-show="$parent.predicate == sortableTableHeader && $parent.reverse"></i><i class="glyphicon glyphicon-sort-by-alphabet" style="" ng-show="$parent.predicate == sortableTableHeader && !$parent.reverse"></i>',
                link: function(scope, element, attrs, controller) {

                    $(element[0]).on('click', function() {

                        scope.$apply(function() {
                            if (scope.$parent.predicate != scope.sortableTableHeader) {
                                scope.$parent.reverse = false;
                            } else {
                                scope.$parent.reverse = !scope.$parent.reverse;
                            }
                            scope.$parent.predicate = scope.sortableTableHeader;
                        });

                    });

                }
            };
        })
        .directive('vuMeter', function() {
            return {
                restrict: 'A',
                scope: {
                    vuMeter: '@',
                    goal: '@',
                    currency: '@',
                    chartTitle:'@'
                },
                link: function(scope, elem) {

                    var actual = parseFloat(scope.vuMeter);
                    var goal = scope.goal !== undefined ? parseFloat(scope.goal.replace(',', '.')) : actual;
                    var currency = scope.currency !== undefined ? scope.currency : '';

                    $(elem[0]).highcharts({
                        chart: {
                            type: 'gauge',
                            height: 180,
                            style: {
                                fontFamily: "'Franklin Gothic Medium', 'Franklin Gothic', 'ITC Franklin Gothic', Arial, sans-serif !important"
                            }
                        },

                        title: {
                            text: scope.chartTitle,
                            style: {
                                fontSize:'16px'
                            }
                        },

                        tooltip: {
                            enabled: false
                        },

                        pane: [
                            {
                                startAngle: -45,
                                endAngle: 45,
                                background: null,
                                center: ['50%', '145%'],
                                size: 300
                            }
                        ],
                        credits: false,
                        yAxis: [
                            {
                                tickInterval: (goal / 4).toFixed(0),
                                minorTickInterval: (goal * 0.1),
                                minorTickWidth: 1,
                                minorTickLength: 12,
                                min: 0,
                                max: (goal + (goal * 0.6)),
                                minorTickPosition: 'outside',
                                tickPosition: 'inside',
                                tickColor: 'rgb(0,0,0)',
                                minorTickColor: 'rgb(0,0,0)',
                                lineColor: 'rgb(0,0,0)',
                                labels: {
                                    rotation: 'auto',
                                    distance: 20
                                },

                                plotBands: [
                                    {
                                        from: goal + 1,
                                        to: (goal + (goal * 0.6)),
                                        color: '#FF0000',
                                        innerRadius: '100%',
                                        outerRadius: '108%'
                                    }
                                ],
                                pane: 0,

                            }
                        ],

                        plotOptions: {
                            gauge: {
                                dataLabels: {
                                    enabled: false
                                },

                                dial: {
                                    radius: '100%',
                                    backgroundColor: 'rgb(255, 255, 255)',
                                    borderColor: 'rgb(0,0,0)',
                                    borderWidth: 2,
                                    baseWidth: 8,
                                    topWidth: 1,
                                }
                            },

                        },


                        series: [
                            {
                                data: [actual],
                                yAxis: 0,
                                overshoot: 3
                            }
                        ]

                    });

                    var chart = $(elem[0]).highcharts();

                    var text = formatNumber(actual) + ' ' + currency;

                    

                    chart.renderer.label(text, 100 - (text.length / 2), 140, 'callout', 1)
                        .css({
                            color: '#FFFFFF'
                        })
                        .attr({
                            fill: actual >= goal ? 'rgba(255, 0, 0, 0.90)' : 'rgba(72, 221, 184,0.90)',
                            padding: 10,
                            r: 4,
                            zIndex: 6
                        })
                        .add();

                }
            };
        });

})();