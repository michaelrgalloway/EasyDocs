'use strict';
app.controller('homeController', ['$scope', 'sectionService', '$routeParams', '$sce', '$location', function ($scope, sectionService, $routeParams, $sce, $location) {
    var vm = this;
    vm.sectionUrlKey = $routeParams.sectionUrlKey;
    vm.scope = $scope;
    vm.scope.section = {};
    
    sectionService.getSection(vm.sectionUrlKey).then(function (result) {
        console.log(result);
        vm.scope.section = result.data;
        vm.scope.content = $sce.trustAsHtml(vm.scope.section.content + vm.scope.postRender());
       
    }, function (error) {
        console.log(error);
        dhtmlx.message({ type: "error", text: "Error Getting Section" })
    });

    vm.scope.postRender = function () {
        return "<script>$('pre code').each(function (i, block) { hljs.highlightBlock(block);});</script>";
    }
    vm.scope.$on("$destroy", function () {
        if (CKEDITOR.instances.editor1) {
            CKEDITOR.instances.editor1.destroy();
        }
    });
    vm.scope.editSection = function (s) {
        vm.scope.$parent.selectedSection = vm.scope.section;
        var config = {
            extraPlugins: 'codesnippet',
            codeSnippet_theme: 'ir_black',
            height: 500
        };
        CKEDITOR.replace('editor1', config);
        if (s.draft == '' || s.draft == null) {
            s.draft = s.content;
        }
        CKEDITOR.instances.editor1.setData(s.draft);
        $('.editSection')
       .modal("setting", {
           closable: false,
           onDeny: function () {

           },
           onApprove: function (elem) {

               if (elem.hasClass('approve')) {
                   s.draft = CKEDITOR.instances.editor1.getData();
                   sectionService.saveDraft(s).then(function (result) {
                       dhtmlx.message("Draft Saved.")
                       sectionService.publishSection(s.id).then(function (result) {
                           if (result.data == true) {
                               s.content = s.draft;
                               s.draft = null;
                               vm.scope.content = $sce.trustAsHtml(vm.scope.section.content + vm.scope.postRender());
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
                   s.draft = CKEDITOR.instances.editor1.getData();
                   sectionService.saveDraft(s).then(function (result) {
                       dhtmlx.message("Draft Saved.")
                   }, function (error) {
                       dhtmlx.message({ type: "error", text: "Error Saving Draft" })
                   });
               }
           },
           onHidden: function () {
               if (CKEDITOR.instances.editor1) {
                   CKEDITOR.instances.editor1.destroy();
               }
           }
       }).modal("show");


    }

    vm.scope.editSidebar = function () {
        vm.scope.$parent.selectedSection = vm.scope.section;
        var bak = $.extend(true, {}, vm.scope.section);
        $('.ui.modal.editSectionSidebar')
        .modal("setting", {
            closable: false,
            onDeny: function () {
               
                vm.scope.section = bak;
                vm.scope.$apply();
                bak = null;
                $('.ui.modal.editSectionSidebar').modal("hide");
            },
            onApprove: function () {

                sectionService.saveSidebar(vm.scope.section).then(function (result) {
                    var obj = result.data;
                    if (obj == true) {

                        $('.ui.modal.editSectionSidebar').modal("hide")
                    }
                    dhtmlx.message("Sidebar Saved.")
                }, function (error) {
                    vm.scope.section = bak;
                    dhtmlx.message({ type: "error", text: "Error Saving Sidebar" })
                });
            }
        }).modal("show");
        return;


    }
    vm.scope.editUrlKey = function () {
        vm.scope.$parent.selectedSection = vm.scope.section;
        var bak = $.extend(true, {}, vm.scope.section);
        $('.ui.modal.editUrlKeySection')
        .modal("setting", {
            closable: false,
            onDeny: function () {
               
                vm.scope.section = bak;
                vm.scope.$apply();
                bak = null;
                $('.ui.modal.editUrlKeySection').modal("hide");
            },
            onApprove: function () {
                vm.scope.section.urlKey = vm.scope.section.urlKey.split(' ').join('-')
                sectionService.saveUrlKey(vm.scope.section).then(function (result) {
                    var obj = result.data;
                    if (obj == true) {

                        $('.ui.modal.editUrlKeySection').modal("hide")
                        $location.path('/admin/Section/' + vm.scope.section.urlKey);
                    }
                    dhtmlx.message("Url Key Saved.")

                }, function (error) {
                    vm.scope.section = bak;
                    dhtmlx.message({ type: "error", text: "Error Saving Url Key" })
                });
            }
        }).modal("show");
        return;


    }
    
}]);