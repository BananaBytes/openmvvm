import "../../lib/base64.min";

class AndroidBridge {
    constructor() {
        this.start = this.start.bind(this);
        this.receiveMessage = this.receiveMessage.bind(this);
        this.sendMessage = this.sendMessage.bind(this);

        window.receiveMessage = this.receiveMessage;
    }

    start(service) {
        this.service = service;
        this.service.jsbind();
    }

    receiveMessage (context, message) {
        var jsonData = window.Base64.decode(message);
        var messageObject = JSON.parse(jsonData);
        this.service[messageObject.FunctionName].apply(this, messageObject.Params);
    }

    sendMessage (message) {
        window.NotifyCs.notify(JSON.stringify(message));
    }
}

export const bridge = new AndroidBridge();