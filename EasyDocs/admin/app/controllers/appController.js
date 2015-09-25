
app.controller('appController', ['$scope', 'sectionService', 'pageService','settingsService','$location','searchService', function ($scope, sectionService, pageService,settingsService,$location,searchService) {
    $scope.headerUrl = "/admin/app/views/header.html"
    $scope.footerUrl = "/admin/app/views/footer.html"
    $scope.editUrl = "/admin/app/views/templates/editSection.html"
    
    var vm = this;
    vm.scope = $scope;
    vm.scope.sections = [];
    vm.scope.pages = [];
    vm.scope.selectedSection = {};
    vm.scope.selectedPage = {};
    vm.scope.header = {};

    sectionService.getSectionsLazy().then(function (results) {
        vm.scope.sections = results.data;

    }, function (error) {
        dhtmlx.message({ type: "error", text: "Error Getting Section List" })
    });
    pageService.getPagesLazy().then(function (results) {
        vm.scope.pages = results.data;

    }, function (error) {
        dhtmlx.message({ type: "error", text: "Error Getting Page List" })
    });
    settingsService.getSettings().then(function (results) {
        vm.scope.header = results.data;

    }, function (error) {
        dhtmlx.message({ type: "error", text: "Error Getting Data" })
    });
    vm.scope.rebuildSearch = function () {
        searchService.rebuildSearch().then(function (results) {
            dhtmlx.message("Search rebuild completed.")

        }, function (error) {
            dhtmlx.message({ type: "error", text: "Error rebuilding search." })
        });
    };

    vm.scope.removeNodeGeneric = function (arr, obj) {
        removeNodeGeneric(arr, obj);
    }
    vm.scope.cancelAll = function(arr){
        arr.forEach(function (a) {
            if (a.sections) vm.scope.cancelAll(a.sections);
            a.titleIsEdit = false;
          
        });
    }
    vm.scope.renderSection = function (section) {
        vm.scope.selectedSection = section;
    }
    vm.scope.addSection = function (s) {


        vm.scope.cancelAll(vm.scope.sections);

        var section = new Object();
        section.title = "";
        section.titleIsEdit = true;
        section.isNew = true;
        section.parent = s;
        section.id = 0;
        if (s == null) {
            section.parentid = null;
            vm.scope.sections.push(section);
           
        }
        else {
            
            section.parentid = s.id;
            s.expanded = true;
            s.sections.push(section);
        }

        //var section = s;

        //if (s == null) {
        //    sectionService.newSection(null).then(function (result) {
        //        var obj = result.data;
        //        obj.sections = [];
        //        vm.scope.sections.push(obj);
        //    }, function (error) {
        //        // invoke loggingAndErrorService
        //    });
        //}
        //else {
        //    sectionService.newSection(section.id).then(function (result) {
        //        var obj = result.data;
        //        obj.sections = [];
        //        section.sections.push(obj);
        //    }, function (error) {
        //        // invoke loggingAndErrorService
        //    });
        //}


    }
    vm.scope.addPage = function () {

        var page = new Object();
        page.title = "";
        page.titleIsEdit = true;
        page.isNew = true;
        vm.scope.pages.push(page);

    }
    vm.scope.addeditPageCommit = function (p) {
        p.titleIsEdit = false;
        if (p.title == '') {
            delete p;
            return;
        }
        pageService.newPage(p.title, p.id).then(function (result) {
            if (p.id) {
                
                //TOASTER
            } else {
                var obj = result.data;
                var i = vm.scope.pages.indexOf(p);
                vm.scope.pages.splice(i, 1);
                vm.scope.pages.push(obj);
                //TOASTER
            }
            dhtmlx.message("Page Saved.")
        }, function (error) {
            dhtmlx.message({ type: "error", text: "Error Saving Page" })
        });

    }
    vm.scope.addeditSectionCommit = function (s) {
        s.titleIsEdit = false;
        if (s.title == '') {
            delete s;
            return;
        }
      
            sectionService.newSection(s.title,s.id,s.parentid).then(function (result) {
                if (s.id == 0) {
                    var obj = result.data;
                    obj.sections = [];

                    if (s.parent == null) {

                        removeNodeGeneric(vm.scope.sections, s);
                        vm.scope.sections.push(obj);
                    }
                    else {
                        removeNodeGeneric(s.parent.sections, s);
                        s.parent.sections.push(obj);
                    }
                    $location.path('/admin/Section/' + obj.urlKey);
                }
                dhtmlx.message("Section Saved.")
                
            }, function (error) {
                dhtmlx.message({ type: "error", text: "Error Saving Section" })
                if (s.parent == null) {

                    removeNodeGeneric(vm.scope.sections, s);
                   
                }
                else {
                    removeNodeGeneric(s.parent.sections, s);
                   
                }
            });
        


    }
    vm.scope.moveSection = function (dir, curIndex) {
        vm.scope.sections.move(curIndex, curIndex + dir)
    }
    vm.scope.deleteSection = function (s, arr) {

        $('.ui.basic.modal.deleteNode')
        .modal("setting", {
            closable: false,
            onDeny: function () {
                $('.ui.basic.modal.deleteNode').modal("hide")
            },
            onApprove: function () {
                var section = s;
                var array = arr;
                sectionService.deleteSection(s.id).then(function (result) {
                    var obj = result.data;
                    if (obj == true) {
                        removeNode(vm.scope.sections, section);
                        $('.ui.basic.modal.deleteNode').modal("hide")
                    }
                    dhtmlx.message("Section Deleted.")
                }, function (error) {
                    dhtmlx.message({ type: "error", text: "Error Deleting Section" })
                });
            }
        }).modal("show");
        return;


    }
    vm.scope.deletePage = function (p, arr) {

        $('.ui.basic.modal.deletePage')
        .modal("setting", {
            closable: false,
            onDeny: function () {
                $('.ui.basic.modal.deletePage').modal("hide")
            },
            onApprove: function () {
                var page = p;
                var array = arr;
                pageService.deletePage(p.id).then(function (result) {
                    var obj = result.data;
                    if (obj == true) {
                        var i = vm.scope.pages.indexOf(p);
                        vm.scope.pages.splice(i, 1);
                        vm.scope.pages.push(obj);
                        $('.ui.basic.modal.deletePage').modal("hide")
                    }
                    dhtmlx.message("Page Deleted.")
                }, function (error) {
                    dhtmlx.message({ type: "error", text: "Error Deleting Page" })
                });
            }
        }).modal("show");
        return;


    }
    vm.scope.editHeader = function () {
        var bak = $.extend(true, {}, vm.scope.header);
        $('.ui.modal.editHeader')
        .modal("setting", {
            closable: false,
            onDeny: function () {
                vm.scope.header = bak;
                vm.scope.$apply();
                bak = null;
                $('.ui.modal.editHeader').modal("hide");
            },
            onApprove: function () {
               
                settingsService.saveSettings(vm.scope.header).then(function (result) {
                    var obj = result.data;
                    if (obj == true) {
                       
                        $('.ui.modal.editHeader').modal("hide")
                    }
                    dhtmlx.message("Header Saved.")
                }, function (error) {
                    vm.scope.header = bak;
                    dhtmlx.message({ type: "error", text: "Error Saving Header" })
                });
            }
        }).modal("show");
        return;


    }
    vm.scope.removeHeaderLink = function (link) {
        var i = vm.scope.header.headerLinks.indexOf(link);
        vm.scope.header.headerLinks.splice(i, 1);
    }
    vm.scope.search = function(term){
        $location.path('/admin/Search/' + term);
        vm.scope.searchTerm = '';
    }

}]);