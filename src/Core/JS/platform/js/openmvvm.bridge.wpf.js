var OpenMVVM = (
    function (openmvvm) {
        "use strict";

        openmvvm.loadView = function (viewName, callback) {
            
            var xhr = new ActiveXObject("Msxml2.XMLHTTP");
            xhr.open('GET', 'Views/' + viewName + '.html', true);

            xhr.onreadystatechange = function () {

                if (xhr.readyState !== 4) return null;

                var div = document.createElement('div');
                div.innerHTML = xhr.responseText;
                var element = div.getElementsByTagName('div')[0];

                callback(element);
                return null;
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

var sendMessage = function(message) {
    window.external.Notify(JSON.stringify(message));
}
