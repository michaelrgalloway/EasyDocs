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

    var _newSection = function (title,id,pid) {

        return $http.get(serviceBase + '/Sections/AddSection/' + title +'/'+id+'/'+pid).then(function (results) {
            return results;
        });

        
    };

    var _deleteSection = function (id) {

        return $http.get(serviceBase + '/Sections/DeleteSection/' + id).then(function (results) {
            return results;
        });


    };

    var _getSection = function (id) {

        return $http.get(serviceBase + '/Sections/GetSection/' + id).then(function (results) {
            return results;
        });


    };

    var _saveDraft = function (section) {
        return $http.post(
            serviceBase + '/Sections/SaveDraft/' + section.id,
            "=" + escape(section.draft),
            { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }
        ).then(function (results) {
            return results;
        });
    };

    var _publishSection = function (id) {

        return $http.get(serviceBase + '/Sections/Publish/' + id).then(function (results) {
            return results;
        });


    };

    var _saveSidebar = function (section) {
        var data = $.extend(true, {}, section);
        data.contents = '';
        return $http.post(serviceBase + '/Sections/SaveResoures', data).then(function (results) {
            return results;
        });
    };

    var _saveUrlKey = function (section) {
      
        return $http.post(serviceBase + '/Sections/SaveUrlKey/'+section.id+'/'+section.urlKey+'/'+section.active).then(function (results) {
            return results;
        });
    };


    sectionServiceFactory.getSectionsLazy = _getSectionsLazy;
    sectionServiceFactory.newSection = _newSection;
    sectionServiceFactory.deleteSection = _deleteSection;
    sectionServiceFactory.getSection = _getSection;
    sectionServiceFactory.saveDraft = _saveDraft;
    sectionServiceFactory.publishSection = _publishSection;
    sectionServiceFactory.saveSidebar = _saveSidebar;
    sectionServiceFactory.saveUrlKey = _saveUrlKey;
    

    return sectionServiceFactory;

}]);