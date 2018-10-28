class SignalrBridge {

    constructor() {
        this.start = this.start.bind(this);
        this.receiveMessage = this.receiveMessage.bind(this);
        this.sendMessage = this.sendMessage.bind(this);
    }

    start(service) {
        this.service = service;
        this.jsBind = service.jsbind;

        this.connection = new window.signalR.HubConnectionBuilder().withUrl("/openMvvmHub").build();

        this.connection.on("receiveMessage", this.receiveMessage);

        function guid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                    .toString(16)
                    .substring(1);
            }

            return s4() +
                s4() +
                "-" +
                s4() +
                "-" +
                s4() +
                "-" +
                s4() +
                "-" +
                s4() +
                s4() +
                s4();
        }

        this.id = guid();

        this.connection.start().then(() => {
            this.connection.invoke("register", this.id);

            this.jsBind();
        }).catch(function(ex) {
            //alert("signalr error");
            alert(ex);
        });
    }


    receiveMessage (context, message) {
        //var messageObject = JSON.parse(message);
        this.service[message.functionName].apply(this, message.params);
    }

    sendMessage (message) {
        this.connection.invoke("fromJs", this.id, JSON.stringify(message));
    }
}


export const bridge = new SignalrBridge();