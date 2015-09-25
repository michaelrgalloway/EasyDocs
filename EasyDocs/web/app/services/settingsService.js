'use strict';
app.factory('settingsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    function makeid() {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (var i = 0; i < 20; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        return text;
    }
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var settingServiceFactory = {};
    var _getSettings = function () {

        return $http.get(serviceBase + '/Settings/GetSettings').then(function (results) {
            return results;
        });
    };
    

    settingServiceFactory.getSettings = _getSettings;

    return settingServiceFactory;

}]);