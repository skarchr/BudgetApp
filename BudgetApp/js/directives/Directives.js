(function() {
    'use strict';

    angular.module('budgetApp')
        .directive('selectCategories', [
            function() {

                return {
                    restrict: 'A',
                    scope: {
                        selectCategories: '@'
                    },
                    require:'ngModel',
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
        .directive('highchart', function() {

            var getIconUrl = function (value) {

                var url = "";

                switch (value) {
                    case "Fixed":
                        url = "../Content/images/two123.png";
                        break;
                    case "DebtReduction":
                        url = "../Content/images/temple18.png";
                        break;
                    case "Dental":
                        url = "../Content/images/teeth.png";
                        break;
                    case "Insurance":
                        url = "../Content/images/insurance.png";
                        break;
                    case "Medical":
                        url = "../Content/images/medical109.png";
                        break;
                    case "OtherFixed":
                        url = "../Content/images/protection3.png";
                        break;
                    case "Food":
                        url = "../Content/images/fork28.png";
                        break;
                    case "Groceries":
                        url = "../Content/images/shopping82.png";
                        break;
                    case "OtherFood":
                        url = "../Content/images/apple55.png";
                        break;
                    case "Restaurant":
                        url = "../Content/images/currency2.png";
                        break;
                    case "Treats":
                        url = "../Content/images/hot51.png";
                        break;
                    case "Personal":
                        url = "../Content/images/user91.png";
                        break;
                    case "Appearance":
                        url = "../Content/images/suit.png";
                        break;
                    case "Entertainment":
                        url = "../Content/images/videogame.png";
                        break;
                    case "Gifts":
                        url = "../Content/images/gift2.png";
                        break;
                    case "Hobby":
                        url = "../Content/images/letter11.png";
                        break;
                    case "OtherPersonal":
                        url = "../Content/images/jumping28.png";
                        break;
                    case "Phone":
                        url = "../Content/images/iphone26.png";
                        break;
                    case "Subscriptions":
                        url = "../Content/images/rss22.png";
                        break;
                    case "Travel":
                        url = "../Content/images/airplane73.png";
                        break;
                    case "Shelter":
                        url = "../Content/images/home168.png";
                        break;
                    case "Furniture":
                        url = "../Content/images/livingroom8.png";
                        break;
                    case "Interior":
                        url = "../Content/images/light14.png";
                        break;
                    case "Mortgage":
                        url = "../Content/images/real-estate.png";
                        break;
                    case "OtherShelter":
                        url = "../Content/images/tribal.png";
                        break;
                    case "Rent":
                        url = "../Content/images/house121.png";
                        break;
                    case "Utilities":
                        url = "../Content/images/lightning31.png";
                        break;
                    case "Transport":
                        url = "../Content/images/car95.png";
                        break;
                    case "Car":
                        url = "../Content/images/car95.png";
                        break;
                    case "CollectiveTransport":
                        url = "../Content/images/bus21.png";
                        break;
                    case "OtherTransportation":
                        url = "../Content/images/map29.png";
                        break;
                    case "Saving":
                        url = "../Content/images/piggy9.png";
                        break;
                    default:
                        return this.value;
                }

                return url;
            };


            return {
                restrict: 'A',
                scope: {
                    highchart: '@'
                },
                link: function(scope, elem, attrs) {

                    var model = JSON.parse(scope.highchart);

                    Highcharts.setOptions({ lang: { drillUpText: 'Back' } });

                    $(elem[0]).highcharts({
                        chart: {
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false,
                            events: {
                                drilldown: function (e) {
                                    this.setTitle({ text: e.point.name });
                                },
                                drillup: function (e) {
                                    this.setTitle({ text: model.title.text });
                                }
                            }
                        },
                        title: {
                            text: model.title.text,
                            style: {
                                fontSize: '18px',
                                color:'#313131'
                            }
                        },
                        tooltip: {
                            enabled:false,
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
                                useHTML: true,
                                formatter: function () {

                                    return '<div class="text-center" style="min-height:50px;"><img class="hs-image-label" title="' + this.value + '" src="' + getIconUrl(this.value) + '"/><br><span class="hidden-sm">' + this.value + '</span></div>';
                                }
                                //rotation: -45
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'Amount'
                            }
                        },
                        series: model.series,
                        drilldown : {
                            series: model.drilldown.series,
                            drillUpButton: {
                                position:{ y:-50 },
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
                                y: -181,
                                x:0,
                                text: 'Balance',
                                style: {
                                    fontSize: '18px',
                                    color: '#313131'
                                }
                            }
                        },

                        series: [{
                            name: ' ',
                            data: [exp],
                            dataLabels: {
                                y:-55,
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
                    }, 500);

                    
                }
            }

        })
    .directive('highstock', function () {
        return {
            restrict: 'A',
            scope: {
                highstock: '@'
            },
            link: function (scope, elem, attrs) {

                var model = JSON.parse(scope.highstock);

                $(elem[0]).highcharts('StockChart', {
                    chart: {
                        type:'column'
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
                        ordinal:false,
                        type: 'datetime'
                    },
                    yAxis: {
                        opposite:false,
                        title: {
                            text: 'Amount'
                        }
                    },
                    series: model.series
                });
            }
        };
    });

})();