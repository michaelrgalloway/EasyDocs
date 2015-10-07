'use strict';
app.controller('pageController', ['$scope', 'loadingService', 'pageService', '$routeParams', '$sce', '$location', function ($scope, loadingService, pageService, $routeParams, $sce, $location) {
    var vm = this;
    vm.pageUrlKey = $routeParams.pageUrlKey;
    vm.scope = $scope;
    vm.scope.content = '';
    vm.scope.page = {};

    if (typeof vm.pageUrlKey == "undefined") {
        vm.pageUrlKey = "_home";
    }
    loadingService.showLoader();
    pageService.getPage(vm.pageUrlKey).then(function (result) {
        vm.scope.page = result.data;
        vm.scope.content = $sce.trustAsHtml(vm.scope.page.content + vm.scope.postRender());
        vm.scope.sidebarContent = $sce.trustAsHtml(vm.scope.page.sideBarContent);
        loadingService.hideLoader();
    }, function (error) {
        dhtmlx.message({ type: "error", text: "Error Getting Page" })
        loadingService.hideLoader();
    });

    vm.scope.postRender = function () {
        return "<script>$('pre code').each(function (i, block) { hljs.highlightBlock(block);});</script>";
    }
    vm.scope.$on("$destroy", function () {
        if (CKEDITOR.instances.editor2) {
            CKEDITOR.instances.editor2.destroy();
        }
        if (CKEDITOR.instances.editor3) {
            CKEDITOR.instances.editor3.destroy();
        }
    });
    vm.scope.editPage = function (s) {
        vm.scope.$parent.selectedPage = s;
        var config = {
            extraPlugins: 'codesnippet',
            codeSnippet_theme: 'ir_black',
            height: 500
        };
        CKEDITOR.replace('editor2', config);
        if (s.draft == '' || s.draft == null) {
            s.draft = s.content;
        }
        CKEDITOR.instances.editor2.setData(s.draft);
        $('.editPage')
       .modal("setting", {
           closable: false,
           onDeny: function () {

           },
           onApprove: function (elem) {

               if (elem.hasClass('approve')) {
                   s.draft = CKEDITOR.instances.editor2.getData();
                   pageService.saveDraft(s).then(function (result) {
                       dhtmlx.message("Draft Saved.")
                       pageService.publishPage(s.id).then(function (result) {
                           if (result.data == true) {
                               s.content = s.draft;
                               s.draft = null;
                               vm.scope.content = $sce.trustAsHtml(vm.scope.page.content + vm.scope.postRender());
                               dhtmlx.message("Draft Published.")
                            
                           }
                       }, function (error) {
                           dhtmlx.message({ type: "error", text: "Error Publishing" })
                       });
                       ////
                       //toaster 'SAVED'
                   }, function (error) {
                       dhtmlx.message({ type: "error", text: "Error Saving Draft" })
                   });

               }
               else {
                   s.draft = CKEDITOR.instances.editor2.getData();
                   pageService.saveDraft(s).then(function (result) {
                       dhtmlx.message("Draft Saved.")
                   }, function (error) {
                       dhtmlx.message({ type: "error", text: "Error Saving Draft" })
                   });
               }
           },
           onHidden: function () {
               if (CKEDITOR.instances.editor2) {
                   CKEDITOR.instances.editor2.destroy();
               }
           }
       }).modal("show");


    }

    vm.scope.editSidebar = function (s) {


        CKEDITOR.replace('editor3');

        CKEDITOR.instances.editor3.setData(s.sideBarContent||"");
        $('.editPageSidebar').modal("setting", {
            closable: false,
            onDeny: function () {

            },
            onApprove: function (elem) {

                if (elem.hasClass('approve')) {

                    var data = CKEDITOR.instances.editor3.getData();
                    pageService.saveSidebar(s, data).then(function (result) {
                        if (result.data == true) {
                            s.sideBarContent = data;
                            vm.scope.sidebarContent = $sce.trustAsHtml(data);
                          
                            
                        }
                        dhtmlx.message("Sidebar Saved.")
                    }, function (error) {
                        dhtmlx.message({ type: "error", text: "Error Saving Sidebar" })
                    });
                }

            },
            onHidden: function () {
                if (CKEDITOR.instances.editor3) {
                    CKEDITOR.instances.editor3.destroy();
                }
            }
        }).modal("show");


    }
    vm.scope.editUrlKey = function () {
        vm.scope.$parent.selectedPage = vm.scope.page;
        var bak = $.extend(true, {}, vm.scope.page);
        $('.ui.modal.editUrlKeyPage')
        .modal("setting", {
            closable: false,
            onDeny: function () {

                vm.scope.page = bak;
                vm.scope.$apply();
                bak = null;
                $('.ui.modal.editUrlKeyPage').modal("hide");
            },
            onApprove: function () {
                vm.scope.page.urlKey = vm.scope.page.urlKey.split(' ').join('-')
                pageService.saveUrlKey(vm.scope.page).then(function (result) {
                    var obj = result.data;
                    if (obj == true) {

                        $('.ui.modal.editUrlKeyPage').modal("hide")
                        $location.path('/admin/Page/' + vm.scope.page.urlKey);
                    }
                    dhtmlx.message("Url Key Saved.")
                }, function (error) {
                    vm.scope.page = bak;
                    dhtmlx.message({ type: "error", text: "Error Saving Url Key" })
                });
            }
        }).modal("show");
        return;


    }

}]);