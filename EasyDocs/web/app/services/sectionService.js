'use strict';
app.factory('sectionService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    function makeid() {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (var i = 0; i < 20; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        return text;
    }
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var sectionServiceFactory = {};
    var _getSectionsLazy = function () {

        return $http.get(serviceBase + '/Sections/SectionHeaders').then(function (results) {
            return results;
        });
    };
    var _getSection = function (id) {

        return $http.get(serviceBase + '/Sections/GetSection/' + id).then(function (results) {
            return results;
        });


    };

   

    sectionServiceFactory.getSectionsLazy = _getSectionsLazy;
    sectionServiceFactory.getSection = _getSection;

    return sectionServiceFactory;

}]);