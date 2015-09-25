var app = angular.module('DocEasyApp', ['ngRoute']);

app.config(function ($routeProvider, $locationProvider) {
    // enable html5Mode for pushstate ('#'-less URLs)
    $locationProvider.html5Mode(true);
    $locationProvider.hashPrefix('!');
    $routeProvider.when("/admin", {
        controller: "homeController",
        templateUrl: "/admin/app/views/home.html",
        caseInsensitiveMatch: true,
        title: 'Home'
    });
    $routeProvider.when("/admin/Section/:sectionUrlKey", {
        controller: "homeController",
        templateUrl: "/admin/app/views/home.html",
        caseInsensitiveMatch: true,
        title: 'Section '
    });
    $routeProvider.when("/admin/Page/:pageUrlKey", {
        controller: "pageController",
        templateUrl: "/admin/app/views/page.html",
        caseInsensitiveMatch: true,
        title: 'Page '
    });
    $routeProvider.when("/admin/Search/:terms", {
        controller: "searchController",
        templateUrl: "/admin/app/views/search.html",
        caseInsensitiveMatch: true,
        title: 'Search '
    });
    $routeProvider.when("/admin/Users", {
        controller: "userController",
        templateUrl: "/admin/app/views/users.html",
        caseInsensitiveMatch: true,
        title: 'Users '
    });
    $routeProvider.otherwise({ redirectTo: "/admin" });

});

var serviceBase = '/api';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase
});

app.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});

app.directive('includeReplace', function () {
    return {
        require: 'ngInclude',
        restrict: 'A', /* optional */
        link: function (scope, el, attrs) {
            el.replaceWith(el.children());
        }
    };
});

app.run(function () {
    
});

app.factory('sectionCache', function ($cacheFactory) {
    return $cacheFactory('sectionCache', {});
})


