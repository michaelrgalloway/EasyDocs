'use strict';
app.factory('userService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var userServiceFactory = {};
    var _getUsers = function () {

        return $http.get(serviceBase + '/Users/GetUsers').then(function (results) {
            return results;
        });
    };

    var _addUser = function (user) {
      
        return $http.post(serviceBase + '/Users/AddUser', user).then(function (results) {
            return results;
        });
    };
  
    var _toggleUser = function (id,active) {

        return $http.post(serviceBase + '/Users/ToggleUser/'+id+'/'+active).then(function (results) {
            return results;
        });
    };
    userServiceFactory.getUsers = _getUsers;
    userServiceFactory.addUser = _addUser;
    userServiceFactory.toggleUser = _toggleUser;
    return userServiceFactory;


}]);