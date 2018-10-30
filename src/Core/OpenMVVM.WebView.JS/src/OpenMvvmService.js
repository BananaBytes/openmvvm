class OpenMvvmService {
    constructor(bridgeMapper, viewLoaderService, parserService, rendererService, context) {
        this.viewCache = {};
        this.eventListeners = {};
        this.collectionCache = context.collectionCache;
        this.elementCache = context.elementCache;
        this.bridgeMapper = bridgeMapper;
        this.viewLoaderService = viewLoaderService;
        this.parserService = parserService;
        this.rendererService = rendererService;

        this.jsbind = this.jsbind.bind(this);
        this.navigateTo = this.navigateTo.bind(this);
        this.handleCollectionChange = this.handleCollectionChange.bind(this);
        this.setValue = this.setValue.bind(this);
    }

    on (event, callback) {
        if (this.eventListeners[event] === undefined) {
            this.eventListeners[event] = [];
        }
        this.eventListeners[event].push(callback);
    }

    jsbind () {

        this.frame = document.querySelector("[data-mvvmstart]");

        var viewElements = document.querySelectorAll("[data-view]");

        for (var i = 0; i < viewElements.length; i++) {
            var currentElement = viewElements[i];
            var viewName = currentElement.getAttribute("data-view");

            this.viewCache[viewName] = {
                element: currentElement,
                initialized: false
            };
            currentElement.parentElement.removeChild(currentElement);
        }

        // all view templates are removed, can initialize
        this.bridgeMapper.appReady();
    }

    navigateTo (viewName) {
        if (!this.viewCache[viewName]) {
            this.viewLoaderService.loadView(viewName, (element) => {
                this.viewCache[viewName] = {
                    element: element,
                    initialized: false
                };
                this.navigateTo(viewName);
            });
            return;
        }

        //elementCache = {};

        var startViewElement = this.viewCache[viewName].element;

        if (!this.viewCache[viewName].initialized) {
            var root = startViewElement;
            var currentDataContext = root.getAttribute("data-context");
            this.parserService.analyzeNode(root, currentDataContext);

            this.viewCache[viewName].initialized = true;
        }

        while (this.frame.firstChild) {
            this.frame.removeChild(this.frame.firstChild);
        }

        this.frame.appendChild(startViewElement);

        this.broadcast("ViewChange", { prev: this.currentView, next: viewName });

        this.currentView = viewName;
    }

    handleCollectionChange (path, parameter) {
        var current = this.collectionCache[path];
        var rootElement = current.rootElement;
        var template = current.template;

        switch (parameter.Action) {
        case 0: // add
            this.rendererService.renderCollectionItemFromTemplate(path, template, rootElement, parameter.NewStartingIndex);
            break;
        case 4:
            var otherAffected = Object.keys(this.elementCache).filter((k) => {
                return k.indexOf(current + "[") === 0;
            }).reduce((newData, k) => {
                newData[k] = this.elementCache[k];
                return newData;
            },
            {});
            for (var item in otherAffected) {
                if (otherAffected.hasOwnProperty(item)) {
                    var affected = otherAffected[item];
                    for (var i = 0; i < affected.length; i++) {
                        var affectedNodeBinding = affected[i];
                        if (affectedNodeBinding.insideTemplate) {
                            // ovo izmeniti
                            delete this.elementCache[item];
                        }
                    }
                }
            }
            break;
        }
    }

    setValue (bindingPath, value) {
        var listOfBindingsForPath = this.elementCache[bindingPath];
        if (listOfBindingsForPath) {
            for (var i = 0; i < listOfBindingsForPath.length; i++) {
                var bindingInfo = listOfBindingsForPath[i];
                var bindingKey = bindingInfo.key;                
                var affectedElement = bindingInfo.element;

                switch (bindingKey) {
                case "mvvmvisible":
                    affectedElement.style.display = bindingInfo.converter(value);
                    break;
                case "mvvmbgcolor":
                    affectedElement.style.backgroundColor = bindingInfo.converter(value);
                    break;
                case "mvvmbgimage":
                    affectedElement.style.backgroundImage = bindingInfo.converter(value);
                    break;
                case "mvvmsubview":
                    if (affectedElement[bindingKey]) {
                        if (affectedElement[bindingKey] !== value) {
                            this.broadcast("SubviewChange", { el: affectedElement, oldVal: affectedElement[bindingKey], newVal: value });
                        }
                    } else {
                        this.broadcast("SubviewChange", { el: affectedElement, oldVal: null, newVal: value });
                    }
                    affectedElement[bindingKey] = value;
                    break;
                default:
                    affectedElement[bindingKey] = bindingInfo.converter(value);
                }
            }
        }

        var collectionList = this.collectionCache[bindingPath];
        if (collectionList) {

            var template = collectionList.template;

            this.rendererService.renderForeach(value, bindingPath, template, collectionList.rootElement);
        }

        var otherAffected = Object.keys(this.elementCache).filter((k) => {
            return k.indexOf(bindingPath + ".") === 0;
        }).reduce((newData, k) => {
            newData[k] = this.elementCache[k];
            return newData;
        }, {});

        if (otherAffected) {
            for (var item in otherAffected) {
                if (otherAffected.hasOwnProperty(item)) {
                    var affectedSubPath = item;
                    this.bridgeMapper.registerBinding(affectedSubPath, false);
                }
            }
        }
    }

    broadcast (event, data) {
        for (var e in this.eventListeners) {
            if (e === event) {
                for (var i = 0; i < this.eventListeners[e].length; i++) {
                    this.eventListeners[e][i](data);
                }
                break;
            }
        }
    }
}

export default OpenMvvmService;