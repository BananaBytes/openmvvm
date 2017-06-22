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
    var messageObject = JSON.parse(message);
    this[context][messageObject.FunctionName].apply(this, messageObject.Params);
}

var sendMessage = function (message) {
    NotifyCs.notify(JSON.stringify(message));
}