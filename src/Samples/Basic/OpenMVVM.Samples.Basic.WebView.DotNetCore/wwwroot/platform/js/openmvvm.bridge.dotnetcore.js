var OpenMVVM = (
    function (openmvvm) {
        "use strict";

        openmvvm.loadView = function (viewName, callback) {
            var xhr = new XMLHttpRequest();
            xhr.open('GET', 'Views/' + viewName + '.html', true);
            xhr.onreadystatechange = function () {
                if (this.readyState !== 4) return null;
                if (this.status !== 200) return null;
                var div = document.createElement('div');
                div.innerHTML = this.responseText;
                var element = div.firstChild;
                callback(element);
            };
            xhr.send();
        }

        return openmvvm;
    }(OpenMVVM || {})
);

var receiveMessage = function (context, message) {
    //var messageObject = JSON.parse(message);
    window[context][message.FunctionName].apply(this, message.Params);
}

var sendMessage = function (message) {
    openMvvmHubProxy.server.fromJs(id, JSON.stringify(message));
}

var openMvvmHubProxy = $.connection.openMvvmHub;
openMvvmHubProxy.client.receiveMessage = receiveMessage;


function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
        s4() + '-' + s4() + s4() + s4();
}

var id = guid();

$.connection.hub.start()
    .done(function () {

        openMvvmHubProxy.server.register(id);

        OpenMVVM.jsBind();
    })