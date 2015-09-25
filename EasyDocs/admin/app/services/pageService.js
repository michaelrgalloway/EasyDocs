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

    var _newPage = function (name,id) {
        return $http.get(serviceBase + '/Pages/AddPage/' + name+'/'+id).then(function (results) {
            return results;
        });
    };

    var _saveDraft = function (page) {
        return $http.post(
            serviceBase + '/Pages/SaveDraft/' + page.id,
            "=" + escape(page.draft),
            {headers: {'Content-Type': 'application/x-www-form-urlencoded'}}
        ).then(function (results) {
            return results;
        });
    };

    var _saveSidebar = function (page,data) {
        return $http.post(
            serviceBase + '/Pages/SaveSidebar/' + page.id,
            "=" + escape(data),
            { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }
        ).then(function (results) {
            return results;
        });
    };
    

    var _deletePage = function (id) {

        return $http.get(serviceBase + '/Pages/DeletePage/' + id).then(function (results) {
            return results;
        });


    };

    var _publishPage = function (id) {

        return $http.get(serviceBase + '/Pages/PublishPage/' + id).then(function (results) {
            return results;
        });


    };

    var _getPage = function (id) {

        return $http.get(serviceBase + '/Pages/GetPage/' + id).then(function (results) {
            return results;
        });


    };

    var _saveUrlKey = function (page) {

        return $http.post(serviceBase + '/Pages/SaveUrlKey/' + page.id + '/' + page.urlKey + '/' + page.active).then(function (results) {
            return results;
        });
    };


    pageServiceFactory.getPagesLazy = _getPagesLazy;
    pageServiceFactory.newPage = _newPage;
    pageServiceFactory.deletePage = _deletePage;
    pageServiceFactory.getPage = _getPage;
    pageServiceFactory.saveDraft = _saveDraft;
    pageServiceFactory.saveSidebar= _saveSidebar;
    pageServiceFactory.publishPage = _publishPage;
    pageServiceFactory.saveUrlKey = _saveUrlKey;


    return pageServiceFactory;

}]);