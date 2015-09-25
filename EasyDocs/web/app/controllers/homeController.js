'use strict';
app.controller('homeController', ['$scope','loadingService', 'sectionService', '$routeParams', '$sce', '$location', function ($scope,loadingService, sectionService, $routeParams, $sce, $location) {
    var vm = this;
    vm.sectionUrlKey = $routeParams.sectionUrlKey;
    vm.scope = $scope;
    vm.scope.section = {};
    loadingService.showLoader();
   
    
    sectionService.getSection(vm.sectionUrlKey).then(function (result) {
        
        vm.scope.section = result.data;
        vm.scope.content = $sce.trustAsHtml(vm.scope.section.content + vm.scope.postRender());
        loadingService.hideLoader();
    }, function (error) {
        loadingService.hideLoader();
        // invoke loggingAndErrorService
    });

    vm.scope.postRender = function () {
        return "<script>$('pre code').each(function (i, block) { hljs.highlightBlock(block);});</script>";
    }
    
}]);