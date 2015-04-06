(function() {
    'use strict';

    angular.module('budgetApp')
        .directive('logo', [
            function() {

                return {
                    restrict: 'A',
                    scope: {
                        logo: '@',
                        from: '@',
                        to: '@',
                        reload: '@'
                    },
                    link: function(scope, elem) {

                        var to = scope.to === undefined ? 1.0 : scope.to;
                        var from = scope.reload === "False" ? scope.to : scope.from === undefined ? 0.7 : scope.from;

                        $(elem[0]).css({
                            '-ms-transform': 'scale(' + from + ', ' + from + ')',
                            '-webkit-transform': 'scale(' + from + ', ' + from + ')',
                            'transform': 'scale(' + from + ', ' + from + ')',
                        });

                        setTimeout(function() {
                            $(elem[0]).css({
                                '-webkit-transition': '-webkit-transform 0.2s ease-out',
                                '-moz-transition': '-moz-transform 0.2s ease-out',
                                'transition': 'transform 0.2s ease-out',
                                'position': 'inherit'
                            });

                            $(elem[0])
                                .velocity({
                                    scale: to,
                                    marginTop: 0
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
                            .delay(300)
                            .velocity('slideDown', {
                                duration: 500
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
                            .delay(300)
                            .velocity('fadeIn', {
                                duration: 500
                            });
                    }
                };

            }
        ]);

})();