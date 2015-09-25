'use strict';
app.factory('pageService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    function makeid() {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (var i = 0; i < 20; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        return text;
    }
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var pageServiceFactory = {};
    var _getPagesLazy = function () {

        return $http.get(serviceBase + '/Pages/PageHeaders').then(function (results) {
            return results;
        });
    };
    var _getPage = function (id) {

        return $http.get(serviceBase + '/Pages/GetPage/' + id).then(function (results) {
            return results;
        });


    };

    pageServiceFactory.getPagesLazy = _getPagesLazy;
    pageServiceFactory.getPage = _getPage;
    return pageServiceFactory;

}]);