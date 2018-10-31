class BridgeMapper {
    constructor(bridge) {
        this.bridge = bridge;

        this.registerBinding = this.registerBinding.bind(this);
        this.propertySet = this.propertySet.bind(this);
        this.fireCommand = this.fireCommand.bind(this);
        this.appReady = this.appReady.bind(this);
    }

    registerBinding (path, insideTemplate) {
        if (insideTemplate) {
            this.bridge.sendMessage({
                functionName: "RegisterBindingCollection",
                params: [path]
            });
        } else {
            this.bridge.sendMessage({
                functionName: "RegisterBinding",
                params: [path, false]
            });
        }
    }

    propertySet (path, newValue) {
        this.bridge.sendMessage({
            functionName: "PropertySet",
            params: [path, newValue]
        });
    }

    fireCommand (path, param) {
        var parameter = param ? param : "";

        this.bridge.sendMessage({
            functionName: "FireCommand",
            params: [path, parameter]
        });
    }

    appReady () {
        this.bridge.sendMessage({
            functionName: "Init",
            params: []
        });
    }
}

export default BridgeMapper;