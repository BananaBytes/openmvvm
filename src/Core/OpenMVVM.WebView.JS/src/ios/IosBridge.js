import "./mt";

class IosBridge {
    constructor() {
        this.start = this.start.bind(this);
        this.receiveMessage = this.receiveMessage.bind(this);
        this.sendMessage = this.sendMessage.bind(this);

        window.Mt.App.addEventListener("receiveMessage",
            (data) => {
                this.receiveMessage(data[0], data[1]);
            });
    }

    start(service) {
        this.service = service;
        this.service.jsbind();
    }
    
    receiveMessage (context, message) {
        var messageObject = JSON.parse(message);
        this.service[messageObject.FunctionName].apply(this, messageObject.Params);
    }

    sendMessage (message) {
        window.Mt.App.fireEvent("doNativeStuff", message);
    }   
}

export const bridge = new IosBridge();