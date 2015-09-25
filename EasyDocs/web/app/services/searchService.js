'use strict';
app.factory('searchService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var searchServiceFactory = {};

    var _search = function (terms) {
      
        return $http.get(serviceBase + '/Search/Search/'+terms).then(function (results) {
            return results;
        });
    };
  
    searchServiceFactory.search = _search;

    return searchServiceFactory;


}]);