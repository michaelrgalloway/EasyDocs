'use strict';
app.factory('loadingService', [ function () {

    var loadingServiceFactory = {};
    var show = true;
    var _showLoader = function () {
        
        setTimeout(function () { if (show) { $(".loader").show(); } else { show = true; } }, 300);
    };

    var _hideLoader = function () {
        show = false;
        $(".loader").hide();
    };

    loadingServiceFactory.showLoader = _showLoader;
    loadingServiceFactory.hideLoader = _hideLoader;
    return loadingServiceFactory;

}]);