'use strict';
app.controller('searchController', ['$scope','loadingService', 'searchService', '$routeParams', '$sce', '$location', function ($scope,loadingService, searchService, $routeParams, $sce, $location) {
    var vm = this;
    vm.scope = $scope;
    vm.scope.terms = $routeParams.terms;
    vm.scope.results = [];
    loadingService.showLoader();
    searchService.search(vm.scope.terms).then(function (result) {
        vm.scope.results = result.data;
        loadingService.hideLoader();
    }, function (error) {
        loadingService.hideLoader();
        dhtmlx.message({ type: "error", text: "Error searching index" })
    });
   
    vm.scope.formatSearchLink = function (result) {
        return "/" + result.type + "/" + result.urlKey;
    }

    vm.scope.formatSearchLinkWithDomain = function (result) {
        return window.location.host  + vm.scope.formatSearchLink(result);
    }
}]);