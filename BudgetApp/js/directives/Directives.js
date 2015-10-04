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


    var formatCamelText = function(input) {
        
        var source = input.replace(/([A-Z])/g, " $1");


        return (source.charAt(0).toUpperCase() + source.slice(1)).trim();
    }

    angular.module('budgetApp')

        .directive('budgetGauge2', function () {

            var arrayGenerator = function (interval, amount, input) {

                var arr = [];

                for (var i = 0; i <= amount; i++) {

                    arr.push({ x : 25 + (interval*i), text: input*i });

                }

                return arr;

            };

            return {
                restrict: 'A',
                transclude: true,
                scope: {
                    budgetGauge2:'@',
                    gaugeIn: '@',
                    gaugeOut: '@',
                    gaugeGoal:'@'
                },

                link:function(scope, elem, attrs) {

                    var income = 0,
                        goal = 0,
                        expenses = 0;

                    if(scope.gaugeIn !== undefined)
                        income = JSON.parse(scope.gaugeIn);

                    if (scope.gaugeOut !== undefined)
                        expenses = JSON.parse(scope.gaugeOut);                    

                    if (scope.gaugeGoal !== '' && scope.gaugeGoal !== undefined && scope.gaugeGoal !== '0')
                        goal = JSON.parse(scope.gaugeGoal);


                    scope.displayIn = income;
                    scope.displayOut = expenses;
                    scope.displayGoal = goal;

                   
                    var max = Math.max(income, expenses, goal);

                    var roundedMax = Math.ceil(max / 100) * 100;

                    scope.intervals = arrayGenerator(100, 5, roundedMax / 5);

                    var pxLength = roundedMax / 24;


                    scope.income = 25 + (scope.gaugeIn * 10 / pxLength);

                    scope.expenses = 25 + (scope.gaugeOut * 10 / pxLength);


                    if (scope.gaugeGoal !== undefined)
                        scope.goal = (25 + (scope.gaugeGoal * 10 / pxLength)).toFixed(2);

                },
                templateUrl: '../views/shared/_Gauge2.html',
            };
        })

        .directive('budgetGauge', function() {


            return {
                restrict: 'A',
                transclude: true,
                scope: {
                    budgetGauge: '@',
                    goal: '@',
                    titleText: '@',
                    type:'@'
                },
                link: function(scope, elem) {

                    var gaugeVal = JSON.parse(scope.budgetGauge);

                    scope.gaugeVal = gaugeVal;

                    scope.zeroText = Math.floor(gaugeVal / 100) * 100;

                    if(scope.goal > 0.0)
                        scope.goalText = JSON.parse(scope.goal).toFixed(2);

                    scope.zeroValue = 140 - (gaugeVal - scope.zeroText);

                    if(scope.goal > 0.0)
                        scope.goalVal = 140 - (gaugeVal - JSON.parse(scope.goal));

                    if (scope.goalVal < 0)
                        scope.goalVal = 0;

                },
                templateUrl: '../views/shared/_Gauge.html',
            };
        })

        .directive('dropdown', function() {
            return {
                restrict : 'C',
                link : function(scope, elem) {
                    
                    $(elem[0]).on('show.bs.dropdown', function(e){
                        $(this).find('.dropdown-menu').first().stop(true, true).slideDown();
                    });

                    $(elem[0]).on('hide.bs.dropdown', function (e) {
                        $(this).find('.dropdown-menu').first().stop(true, true).slideUp();
                    });

                }
            };
        })

        .directive('topNav', [
            '$window', function($window) {
                return {
                    restrict: 'A',
                    link: function(scope, elem) {

                        if ($window.scrollY < 20) {
                            $(elem[0]).addClass('no-scroll-top');
                            $(elem[0]).addClass('scroll-top');
                        }


                        $($window).on('scroll', function() {

                            if (this.scrollY < 20) {
                                $(elem[0]).addClass('scroll-top');
                            } else {
                                $(elem[0]).removeClass('scroll-top');
                                $(elem[0]).addClass('no-scroll-top');
                            }

                        });

                    }
                };
            }
        ])
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
                        currency: '@'
                    },
                    link: function(scope, elem, attrs) {

                        var model = JSON.parse(scope.overviewHighchart);

                        Highcharts.setOptions({ lang: { drillUpText: 'Back' } });

                        $(elem[0]).highcharts({
                            chart: {
                                style: {
                                    fontFamily: "Tahoma, Geneva, sans-serif !important"
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
                                    showInLegend: true,
                                    states: {
                                        hover: {
                                            enabled: false
                                        }
                                    }
                                },
                                
                            },
                            xAxis: {
                                categories: model.xAxis[0].categories,
                                plotLines: scope.range !== 'Annual' ? model.xAxis[0].plotLines : null,
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
                                plotLines: scope.range === 'Month' || scope.range === 'Annual' ? model.yAxis[0].plotLines : null
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

                            var text = formatNumber(JSON.parse(scope.totalIncome.replace(',', '.')));

                            chart.renderer.label(text + currency, 70, 10, 'callout', 1)
                                .css({
                                    color: '#FFFFFF'
                                })
                                .attr({
                                    fill: 'rgba(72, 221, 184,0.70)', //'rgba(255, 0, 0, 0.90)',
                                    padding: 2,
                                    r: 4,
                                    zIndex: 6,
                                    title: 'Total income'
                                })
                                .add();
                        }

                        if (scope.totalExpenses !== undefined) {

                            var text1 = formatNumber(JSON.parse(scope.totalExpenses.replace(',', '.')));

                            chart.renderer.label(text1 + currency, 70, 32, 'callout', 1)
                                .css({
                                    color: '#FFFFFF'
                                })
                                .attr({
                                    fill: '#E87C7C',
                                    stroke: 'rgb(0,0,0)',
                                    padding: 2,
                                    r: 4,
                                    zIndex: 6,
                                    title: 'Total expenses'
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
                        currency: '@',
                        index : '@'
                    },
                    link: function(scope, elem, attrs) {

                        var model = JSON.parse(scope.highchart);

                        Highcharts.setOptions({ lang: { drillUpText: 'Back' } });

                        $(elem[0]).highcharts({
                            chart: {
                                style: {
                                    fontFamily: "Tahoma, Geneva, sans-serif !important"
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
                                enabled: false,
                                align: 'right',
                                layout: 'vertical',
                                y: -25
                            },
                            credits: false,
                            plotOptions: {
                                column: {
                                    showInLegend: false,
                                    colorByPoint: true,
                                    events: {
                                        click: function (e) {

                                            var _scope = angular.element('[ng-controller=transactionController]').scope();

                                            if (_scope !== undefined) {

                                                if (scope.index !== undefined) {

                                                    _scope.querystring[scope.index] = e.point.name !== undefined ? formatCamelText(e.point.name) : '';
                                                
                                                }
                                                _scope.$apply();
                                            }

                                            
                                        }
                                    }
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
                                        heigth:50,
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
                    chartTitle: '@',
                    inverted: '@'
                },
                link: function(scope, elem) {

                    var actual = parseFloat(scope.vuMeter);
                    var goal = scope.goal !== undefined ? parseFloat(scope.goal.replace(',', '.')) : actual;
                    var currency = scope.currency !== undefined ? scope.currency : '';

                    var from;
                    var to;

                    if (scope.inverted) {
                        from = 0;
                        to = goal;
                    } else {
                        from = goal;
                        to = (goal + (goal * 0.6));
                    }

                    $(elem[0]).addClass('reflow-chart');

                    $(elem[0]).highcharts({
                        chart: {
                            type: 'gauge',
                            height: 180,
                            style: {
                                fontFamily: "Tahoma, Geneva, sans-serif !important"
                            }
                        },

                        title: {
                            text: scope.chartTitle,
                            style: {
                                fontSize: '16px'
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
                                tickInterval: (goal / (goal % 2 === 0 ? 4 : 3)).toFixed(0),
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
                                        from: from,
                                        to: to,
                                        color: "#E87C7C",
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

                    var fill;

                    if (scope.inverted) {
                        fill = actual >= goal ? 'rgba(72, 221, 184,0.90)' : '#E87C7C';
                    } else {
                        fill = actual >= goal ? '#E87C7C' : 'rgba(72, 221, 184,0.90)';
                    }

                    chart.renderer.label(text, 90 - (text.length / 2), 160, 'callout', 1)
                        .css({
                            color: fill,
                            fontSize: '16px'
                        })
                        .attr({
                            zIndex: 6
                        })
                        .add();

                }
            };
        })
        .directive('reflowCharts', [
            function() {
                return {
                    restrict: 'A',
                    link: function(scope, elem, attrs) {

                        $(elem[0]).on('click', function() {

                            setTimeout(function() {
                                $('.reflow-chart').each(function() {

                                    var chart = $(this).highcharts();
                                    chart.reflow();

                                    chart.redraw();

                                });
                            }, 15);

                        });

                    }
                };
            }
        ])

        .directive('treemapChart', [function() {

            return {
                restrict: 'A',
                scope: {
                    treemapChart:'@'
                },
                link:function(scope, elem) {
                    
                    var model = JSON.parse(scope.treemapChart);

                    Highcharts.setOptions({ lang: { drillUpText: 'Back' } });

                    $(elem[0]).highcharts({
                        chart: {
                            style: {
                                fontFamily: "Tahoma, Geneva, sans-serif !important"
                            },
                            events: {
                                redraw: function () {

                                    if (this.series[0].rootNode === '')
                                        $(elem[0]).highcharts().setTitle({ text: 'Expenses' });
                                }
                            }
                        },
                        credits: false,
                        series: [{
                            events: {
                                click: function (e) {
                                    if(e.point.parent === undefined)
                                        $(elem[0]).highcharts().setTitle({ text: e.point.name});
                                }
                            },
                            type: 'treemap',
                            drillUpButton: {
                                position: { y: -30, x:-10, align:'right' },
                                theme: {
                                    fill: 'white',
                                    'stroke-width': 1,
                                    stroke: 'silver',
                                    heigth:50,
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
                            levels: [{
                                level: 1,
                                dataLabels: {
                                    enabled: true
                                },
                                borderWidth: 3
                            }],
                            data: model
                        }],
                        tooltip: {
                            enabled: true,
                            pointFormat: '<span style="color:{point.color}">\u25CF</span> {point.name}: <b>{point.value:.1f}</b><br/>'
                        },
                        title: {
                            text: 'Expenses',
                            style: {
                                fontSize: '18px',
                                color: '#000'
                            }
                        }
                    });

                }
            };

        }])

        .directive('progHighchart', [
            function() {

                return {
                    restrict: 'A',
                    scope: {
                        progHighchart: '@',
                        chartLabel: '@',
                        currency: '@',
                        legend:'@'
                    },
                    link: function(scope, elem, attrs) {

                        var model = JSON.parse(scope.progHighchart);

                        Highcharts.setOptions({ lang: { drillUpText: 'Back' } });

                        $(elem[0]).highcharts({
                            chart: {
                                type: 'line',
                                style: {
                                    fontFamily: "Tahoma, Geneva, sans-serif !important"
                                },
                                plotBackgroundColor: null,
                                plotBorderWidth: null,
                                plotShadow: false
                            },
                            title: {
                                text: model.title.text,
                                style: {
                                    fontSize: '18px',
                                    color: '#313131'
                                }
                            },
                            tooltip: {
                                enabled: true,
                                pointFormat: '{series.name}: <b>{point.y:.1f} </b>'
                            },
                            legend: {
                                enabled: scope.legend === undefined,
                                align: 'right',
                                verticalAlign: 'bottom',
                                layout: 'horizontal',
                                y: 20
                            },
                            credits: false,
                            plotOptions: {
                                series: {
                                    dataLabels: {
                                        enabled: false
                                    }
                                }

                            },
                            series: model.series,
                            xAxis: model.xAxis,
                            yAxis: model.yAxis
                        });

                    }
                }
            }
        ])
    .directive('burnRate', [function() {

        return {
            restrict: 'A',
            scope: {
                burnRate: '@'
            },
            link: function(scope, elem, attrs) {
                
                var model = JSON.parse(scope.burnRate);
                $(elem[0]).highcharts({
                    chart: {
                        zoomType: 'x'
                    },
                    credits: false,
                    title: {
                        text: ' ',
                    },
                    subtitle: {
                        text: ' '
                    },
                    xAxis: {
                        type: 'datetime',
                        tickInterval: null,
                        //only if there are atleast 3 days left
                        plotLines: model.xAxis[0].plotLines
                    },
                    yAxis: {
                        title: {
                            text: ' '
                        },
                        endOnTick: true
                    },
                    legend: {
                        enabled: false
                    },
                    plotOptions: {
                        area: {
                            marker: {
                                radius: 0
                            },
                            lineWidth: 1,
                            states: {
                                hover: {
                                    lineWidth: 2
                                }
                            },
                            threshold: 0
                        }
                    },
                    series: model.series
                });

            }
        };

    }]);

})();