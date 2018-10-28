var OpenMVVM = (
    function (openmvvm) {
        "use strict";

        var bridgeService = new openmvvm.BridgeService();

        var elementCache = {};
        var collectionCache = {};
        var frame = null;
        var viewCache = {};
        var currentView = "";
        var eventListeners = {};

        //var requestValueAndUpdates = function (path, insideTemplate) {
        //    bridgeService.registerBinding(path, insideTemplate);
        //};

        var broadcast = function (event, data) {
            // console.log('broadcast', event, data);
            for (var e in eventListeners) {
                if (e === event) {
                    for (var i = 0; i < eventListeners[e].length; i++) {
                        eventListeners[e][i](data);
                    }
                    break;
                }
            }
        }

        



        

        return openmvvm;
    }(OpenMVVM || {})
);