'use strict';
app.controller('userController', ['$scope', 'userService', '$routeParams', '$sce', '$location', function ($scope, userService, $routeParams, $sce, $location) {
    var vm = this;
    vm.scope = $scope;
    vm.scope.users = [];
    userService.getUsers().then(function (result) { 
        vm.scope.users= result.data;
    }, function (error) {
        // invoke loggingAndErrorService
    });
    vm.scope.addUser = function () {
        var newUser = {};
        newUser.isAdd = true;
        newUser.id = 0;
        vm.scope.users.push(newUser);
    }

    vm.scope.commitUser = function (user) {
        userService.addUser(user).then(function (result) {
            removeNodeGeneric(vm.scope.users,user);
            vm.scope.users.push(result.data);
            dhtmlx.message("User Saved.")
        }, function (error) {
            dhtmlx.message({ type: "error", text: "Error Saving User" })
        });
    }

    vm.scope.toggleUser = function (user) {
        userService.toggleUser(user.id,!user.active).then(function (result) {
            user.active = result.data;
            dhtmlx.message("User Saved.")
        }, function (error) {
            dhtmlx.message({ type: "error", text: "Error Saving User" })
        });
    }

}]);