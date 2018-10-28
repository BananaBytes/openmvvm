class ParserService {
    constructor(bridgeMapper, context) {
        this.elementCache = context.elementCache;
        this.collectionCache = context.collectionCache;
        this.bridgeMapper = bridgeMapper;
    }

    analyzeThis (child, dataContext, insideTemplate) {
        this.parseBind(child, dataContext, insideTemplate);

        this.parseActions(child, dataContext, insideTemplate);

        var foreachAttributeValue = child.getAttribute("data-repeat");
        if (foreachAttributeValue) {
            this.parseForeach(child, dataContext, insideTemplate);
        } else {
            this.analyzeNode(child, dataContext, insideTemplate);
        }
    }

    analyzeNode  (node, dataContext, insideTemplate) {
        var childNodes = node.children;

        for (var index = 0; index < childNodes.length; index++) {
            var child = childNodes[index];

            this.analyzeThis(child, dataContext, insideTemplate);
        }
    }

    globalizePath (relativePath, bindedPath) {
        var path = bindedPath;
        if (relativePath.slice(0, 2) === "..") {
            relativePath = relativePath.slice(2);
            path = path.slice(0, path.lastIndexOf("."));
        }
        while (relativePath[0] === ".") {
            relativePath = relativePath.slice(1);
            path = path.slice(0, path.lastIndexOf("."));
        }
        return path + "." + relativePath;
    }

    createHandler (bindingPath, param) {
        return (e) => {
            e.stopImmediatePropagation();
            this.bridgeMapper.fireCommand(bindingPath, param);
        };
    }

    createBindingInfo (element, key, insideTemplate, converter, parameter) {
        var binding = {
            element: element,
            key: key,
            insideTemplate: insideTemplate,
            converter: converter,
            parameter: parameter
        };

        return binding;
    }

    saveBinding (bindingPath, binding) {
        if (!this.elementCache[bindingPath]) {
            this.elementCache[bindingPath] = [binding];
        } else {
            this.elementCache[bindingPath].push(binding);
        }
    }

    parseActions (child, dataContext, insideTemplate) {
        var actionsAttributeValue = child.getAttribute("data-actions");
        if (actionsAttributeValue) {

            var context = dataContext;
            var bindingDescriptor = eval("(" + actionsAttributeValue + ")");
            for (var key in bindingDescriptor) {
                if (bindingDescriptor.hasOwnProperty(key)) {
                    var descriptor = bindingDescriptor[key];
                    var path = null;
                    var parameter = null;
                    if (typeof descriptor === "string" || descriptor instanceof String) {
                        path = descriptor;
                        parameter = null;
                    } else {
                        path = descriptor.command;
                        parameter = descriptor.parameter;
                    }

                    var binding = this.createBindingInfo(child, key, insideTemplate, null, parameter);

                    var overridenDataContext = child.getAttribute("data-context");

                    if (overridenDataContext) {
                        dataContext = overridenDataContext;
                    }

                    var bindingPath = this.globalizePath(path, dataContext);

                    this.saveBinding(bindingPath, binding);

                    var value = this.createHandler(bindingPath, parameter);
                    child.addEventListener(key, value);
                }
            }
        }
    }

    parseBind (child, dataContext, insideTemplate) {
        var bindAttributeValue = child.getAttribute("data-bind");
        if (bindAttributeValue) {
            var that = this;
            var context = dataContext;
            var bindingDescriptor = eval("(" + bindAttributeValue + ")");
            for (var key in bindingDescriptor) {
                if (bindingDescriptor.hasOwnProperty(key)) {
                    var descriptor = bindingDescriptor[key];
                    var path = null;
                    var converter = function (v) { return v; };
                    if (typeof descriptor === "string" || descriptor instanceof String) {
                        path = descriptor;
                    } else {
                        path = descriptor.path;
                        converter = descriptor.converter;
                    }

                    var binding = this.createBindingInfo(child, key, insideTemplate, converter);

                    var overridenDataContext = child.getAttribute("data-context");

                    if (overridenDataContext) {
                        dataContext = overridenDataContext;
                    }

                    var bindingPath = this.globalizePath(path, dataContext);

                    this.saveBinding(bindingPath, binding);
                    
                    if (key === "value") {
                        if (child.type === "radio") {
                            var bpath = bindingPath;
                            
                            child.addEventListener("change",
                                function () {
                                    if (child.checked) {
                                        var newValue = child.value;

                                        // converteri i ovde, convert back!
                                        var relativePath = child.getAttribute("data-selected-bind");

                                        var newPath = relativePath ? that.globalizePath(relativePath, bpath) : bpath;

                                        that.bridgeMapper.propertySet(newPath, newValue);
                                    }
                                });
                        }
                        else {
                            var bpath = bindingPath;
                            child.addEventListener("input",
                                function () {
                                    var newValue = child.value;

                                    // converteri i ovde, convert back!

                                    that.bridgeMapper.propertySet(bpath, newValue);
                                });
                        }
                    }
                    if (key === "checked") {
                        if (child.type === "checkbox") {
                            var bpath = bindingPath;
                            child.addEventListener("change",
                                function () {
                                    var newValue = child.checked;

                                    that.bridgeMapper.propertySet(bpath, newValue);
                                });
                        }
                    }
                    this.bridgeMapper.registerBinding(bindingPath, insideTemplate);
                }
            }
        }
    }

    parseForeach (child, dataContext, insideTemplate) {
        var foreachAttributeValue = child.getAttribute("data-repeat");
        if (foreachAttributeValue) {
            var collectionPath = dataContext + "." + foreachAttributeValue;

            var template = child.children[0];

            this.collectionCache[collectionPath] = {
                rootElement: child,
                template: template
            };

            child.removeChild(template);

            this.bridgeMapper.registerBinding(collectionPath, insideTemplate);
        }
    }
}

export default ParserService;