
app.controller('appController', ['$scope','loadingService', 'sectionService', 'pageService','settingsService','$location', function ($scope,loadingService, sectionService, pageService,settingsService,$location) {
    $scope.headerUrl = "/web/app/views/header.html"
    $scope.footerUrl = "/web/app/views/footer.html"
    
    var vm = this;
    vm.scope = $scope;
    vm.scope.sections = [];
    vm.scope.pages = [];
    vm.scope.selectedSection = {};
    vm.scope.selectedPage = {};
    vm.scope.header = {};
    
    settingsService.getSettings().then(function (results) {
        vm.scope.header = results.data;
        vm.scope.headerloaded = true;
    }, function (error) {

        // invoke loggingAndErrorService
    });
    
    var applyExpanded = function (list, key) {
        if (key == '') { return list; }
        for (l in list) {
            if (list[l].sections) {
                for (s in list[l].sections) {
                    if (list[l].sections[s].urlKey) {
                        if (list[l].sections[s].urlKey.toLowerCase() == key) {
                            list[l].expanded = true;
                        }
                    }
                }
                l.sections = applyExpanded(l.sections, key);
            }
        }
        return list;
    }

    sectionService.getSectionsLazy().then(function (results) {
        var key = '';
        var path = window.location.pathname.split('/');
        if (path[1].toLowerCase() == 'section') {
            key = path[2].toLowerCase();
        }
        vm.scope.sections = applyExpanded(results.data, key);
    }, function (error) {
       
        // invoke loggingAndErrorService
    });
    
    pageService.getPagesLazy().then(function (results) {
        vm.scope.pages = results.data;
      
    }, function (error) {
       
        // invoke loggingAndErrorService
    });
   
    

    vm.scope.renderSection = function (section) {
        vm.scope.selectedSection = section;
    }
    vm.scope.search = function (term) {
        $location.path('/Search/' + term);
        vm.scope.searchTerm = '';
    }
    
}]);