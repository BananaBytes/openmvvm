class WpfBridge {
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
    
    receiveMessage(context, message) {
        var messageObject = JSON.parse(message);
        this.service[messageObject.FunctionName].apply(this, messageObject.Params);
    }

    sendMessage (message) {
        window.external.notify(JSON.stringify(message));
    }   
}

export const bridge = new WpfBridge();