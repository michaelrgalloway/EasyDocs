'use strict';
app.controller('pageController', ['$scope','loadingService', 'pageService', '$routeParams', '$sce', '$location', function ($scope,loadingService, pageService, $routeParams, $sce, $location) {
    var vm = this;
    vm.pageUrlKey = $routeParams.pageUrlKey;
    vm.scope = $scope;
    vm.scope.content = '';
    vm.scope.page = {};
    
    if (typeof vm.pageUrlKey == "undefined") {
        vm.pageUrlKey = "_home";
    }

    loadingService.showLoader();
    pageService.getPage(vm.pageUrlKey).then(function (result) {
        vm.scope.page = result.data;
        vm.scope.content = $sce.trustAsHtml(vm.scope.page.content + vm.scope.postRender());
        vm.scope.sidebarContent = $sce.trustAsHtml(vm.scope.page.sideBarContent);
        loadingService.hideLoader();
       
    }, function (error) {
        loadingService.hideLoader();
        // invoke loggingAndErrorService
    });

    vm.scope.postRender = function () {
        return "<script>$('pre code').each(function (i, block) { hljs.highlightBlock(block);});</script>";
    }
   

}]);