'use strict';
app.controller('searchController', ['$scope', 'searchService', '$routeParams', '$sce', '$location', function ($scope, searchService, $routeParams, $sce, $location) {
    var vm = this;
    vm.scope = $scope;
    vm.scope.terms = $routeParams.terms;
    vm.scope.results = [];
    searchService.search(vm.scope.terms).then(function (result) {
        vm.scope.results = result.data;
        console.log(vm.scope.results);
    }, function (error) {
        dhtmlx.message({ type: "error", text: "Error searching index" })
    });
   
    vm.scope.formatSearchLink = function (result) {
        return "/admin/" + result.type + "/" + result.urlKey;
    }

    vm.scope.formatSearchLinkWithDomain = function (result) {
        return window.location.host  + vm.scope.formatSearchLink(result);
    }
}]);