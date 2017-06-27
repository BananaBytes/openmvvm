var OpenMVVM = (
    function (openmvvm) {
        "use strict";

        openmvvm.BridgeService = function () {
            this.registerBinding = function (path, insideTemplate) {
                if (insideTemplate) {
                    sendMessage({
                        functionName: 'RegisterBindingCollection',
                        params: [path]
                    });
                } else {
                    sendMessage({
                        functionName: 'RegisterBinding',
                        params: [path, false]
                    });
                }
            };

            this.propertySet = function (path, newValue) {
                sendMessage({
                    functionName: 'PropertySet',
                    params: [path, newValue]
                });
            };

            this.fireCommand = function (path, param) {
                var parameter = param ? param : "";

                sendMessage({
                    functionName: 'FireCommand',
                    params: [path, parameter]
                });
            }

            this.appReady = function () {
                sendMessage({
                    functionName: 'Init',
                    params: []
                });
            }
        };

        var bridgeService = new openmvvm.BridgeService();

        var elementCache = {};
        var collectionCache = {};
        var frame = null;
        var viewCache = {};
        var currentView = "";

        var requestValueAndUpdates = function (path, insideTemplate) {
            bridgeService.registerBinding(path, insideTemplate);
        };

        openmvvm.setValue = function (bindingPath, value) {
            var affectedList = elementCache[bindingPath];
            if (affectedList) {
                for (var i = 0; i < affectedList.length; i++) {
                    var affectedNodeBindings = affectedList[i];
                    var bindings = affectedNodeBindings.bindings;

                    for (var bindingKey in bindings) {
                        if (bindings.hasOwnProperty(bindingKey)) {
                            var affectedElement = affectedNodeBindings.element;

                            switch (bindingKey) {
                                case 'mvvmvisible':
                                    affectedElement.style.display = affectedNodeBindings.converter(value);
                                    break;
                                case 'mvvmbgcolor':
                                    affectedElement.style.backgroundColor = affectedNodeBindings.converter(value);
                                    break;
                                case 'mvvmbgimage':
                                    affectedElement.style.backgroundImage = affectedNodeBindings.converter(value);
                                    break;
                                default:
                                    affectedElement[bindingKey] = affectedNodeBindings.converter(value);
                            }
                        }
                    }
                }
            }

            var collectionList = collectionCache[bindingPath];
            if (collectionList) {

                var template = collectionList.template;

                renderForeach(value, bindingPath, template, collectionList.rootElement);
            }

            var otherAffected = Object.keys(elementCache).filter(function (k) {
                return k.indexOf(bindingPath + ".") === 0;
            }).reduce(function (newData, k) {
                newData[k] = elementCache[k];
                return newData;
            },
                {});

            if (otherAffected) {
                for (var item in otherAffected) {
                    if (otherAffected.hasOwnProperty(item)) {
                        var affectedSubPath = item;
                        applyValue(affectedSubPath);
                    }
                }
            }

        };

        var applyValue = function (bindingPath, insideTemplate) {
            var affectedList = elementCache[bindingPath];
            for (var i = 0; i < affectedList.length; i++) {
                var affectedNodeBindings = affectedList[i];
                var bindings = affectedNodeBindings.bindings;

                for (var bindingKey in bindings) {
                    if (bindings.hasOwnProperty(bindingKey)) {
                        requestValueAndUpdates(bindingPath, insideTemplate);
                    }
                }
            }
        };

        var createHandler = function (bindingPath, param) {
            return function (e) {
                e.stopImmediatePropagation();
                bridgeService.fireCommand(bindingPath, param);
            }
        }

        var parseActions = function (child, dataContext, insideTemplate) {
            var actionsAttributeValue = child.getAttribute('data-actions');
            if (actionsAttributeValue) {

                var context = dataContext;
                var bindingDescriptor = eval('(' + actionsAttributeValue + ')');
                for (var key in bindingDescriptor) {
                    if (bindingDescriptor.hasOwnProperty(key)) {
                        var descriptor = bindingDescriptor[key];
                        var path = null;
                        var parameter = null;
                        if (typeof descriptor === 'string' || descriptor instanceof String) {
                            path = descriptor;
                            parameter = null;
                        } else {
                            path = descriptor.command;
                            parameter = descriptor.parameter;
                        }

                        var overridenDataContext = child.getAttribute('data-context');

                        if (overridenDataContext) {
                            dataContext = overridenDataContext;
                        }

                        var bindingPath = dataContext + "." + path;

                        var binding = {
                            element: child,
                            bindings: bindingDescriptor,
                            insideTemplate: insideTemplate,
                            parameter: parameter
                        }

                        if (!elementCache[bindingPath]) {
                            elementCache[bindingPath] = [binding];
                        } else {
                            elementCache[bindingPath].push(binding);
                        }

                        var bindings = binding.bindings;

                        for (var bindingKey in bindings) {
                            if (bindings.hasOwnProperty(bindingKey)) {
                                var affectedElement = binding.element;

                                var value = createHandler(bindingPath, parameter);
                                affectedElement.addEventListener(bindingKey, value);
                            }
                        }
                    }
                }
            }
        };

        var parseBind = function (child, dataContext, insideTemplate) {
            var bindAttributeValue = child.getAttribute('data-bind');
            if (bindAttributeValue) {

                var bindingDescriptor = eval('(' + bindAttributeValue + ')');
                for (var key in bindingDescriptor) {
                    if (bindingDescriptor.hasOwnProperty(key)) {
                        var descriptor = bindingDescriptor[key];
                        var path = null;
                        var converter = function (v) { return v };
                        if (typeof descriptor === 'string' || descriptor instanceof String) {
                            path = descriptor;
                        } else {
                            path = descriptor.path;
                            converter = descriptor.converter;
                        }

                        var overridenDataContext = child.getAttribute('data-context');

                        if (overridenDataContext) {
                            dataContext = overridenDataContext;
                        }

                        var bindingPath = dataContext + "." + path;

                        var binding = {
                            element: child,
                            bindings: bindingDescriptor,
                            insideTemplate: insideTemplate,
                            converter: converter
                        }

                        if (!elementCache[bindingPath]) {
                            elementCache[bindingPath] = [binding];
                        } else {
                            elementCache[bindingPath].push(binding);
                        }

                        if (key === "value") {
                            child.addEventListener('input',
                                function (event) {
                                    var newValue = child.value;
                                    var path = bindingPath;

                                    // this is the place where 'convertBack' can be applied

                                    bridgeService.propertySet(path, newValue);
                                });
                        }

                        applyValue(bindingPath, insideTemplate);
                    }
                }
            }
        };

        var parseForeach = function (child, dataContext, insideTemplate) {
            var foreachAttributeValue = child.getAttribute('data-repeat');
            if (foreachAttributeValue) {
                var collectionPath = dataContext + "." + foreachAttributeValue;

                var template = child.children[0];

                collectionCache[collectionPath] = {
                    rootElement: child,
                    template: template
                };

                child.removeChild(template);

                requestValueAndUpdates(collectionPath, insideTemplate);

            }
        };

        var getNextPath = function (rootPath, currentPath, indexAlterFunction) {
            var affectedSubPath = currentPath;
            var posOfIndex = rootPath.length + 1;
            var tempWithoutPrefix = affectedSubPath.substring(posOfIndex);
            var indexLength = tempWithoutPrefix.indexOf("]");
            var tempBeforeIndex = affectedSubPath.substr(0, posOfIndex);
            var tempAfterIndex = tempWithoutPrefix.substr(indexLength);
            var indexString = affectedSubPath.substr(rootPath.length + 1, indexLength);
            var index = parseInt(indexString);

            var newIndex = indexAlterFunction(index);

            var nextPath = tempBeforeIndex + newIndex + tempAfterIndex;
            return {
                nextPath: nextPath,
                index: index,
                newIndex: newIndex
            }
        };

        var renderCollectionItemFromTemplate = function (collectionPath, template, child, newIndex) {
            var clone = template.cloneNode(true);

            if (newIndex < child.children.length) {
                child.insertBefore(clone, child.children[newIndex]);

                var otherAffected = Object.keys(elementCache).filter(function (k) {
                    return k.indexOf(collectionPath + "[") === 0;
                }).reduce(function (newData, k) {
                    newData[k] = elementCache[k];
                    return newData;
                },
                    {});

                var temp = {};

                for (var item in otherAffected) {
                    if (otherAffected.hasOwnProperty(item)) {
                        var pathInfo = getNextPath(collectionPath,
                            item,
                            function (indx) {
                                return indx + 1;
                            });

                        if (pathInfo.index >= newIndex) {
                            temp[pathInfo.nextPath] = elementCache[item];
                            delete elementCache[item];
                        }
                    }
                }

                for (var tempItem in temp) {
                    if (temp.hasOwnProperty(tempItem)) {
                        elementCache[tempItem] = temp[tempItem];
                    }
                }
            } else {
                child.appendChild(clone);
            }

            analyzeThis(clone, collectionPath + "[" + newIndex + "]", true);
        }

        var renderForeach = function (items, collectionPath, template, child) {
            while (child.firstChild) {
                child.removeChild(child.firstChild);
            }

            for (var i = 0; i < items.length; i++) {
                renderCollectionItemFromTemplate(collectionPath, template, child, i);
            }
        };

        var analyzeThis = function (child, dataContext, insideTemplate) {
            parseBind(child, dataContext, insideTemplate);

            parseActions(child, dataContext, insideTemplate);

            var foreachAttributeValue = child.getAttribute('data-repeat');
            if (foreachAttributeValue) {
                parseForeach(child, dataContext, insideTemplate);
            } else {
                analyzeNode(child, dataContext, insideTemplate);
            }
        }

        var analyzeNode = function (node, dataContext, insideTemplate) {

            var childNodes = node.children;

            for (var index = 0; index < childNodes.length; index++) {
                var child = childNodes[index];

                analyzeThis(child, dataContext, insideTemplate);
            }
        };

        openmvvm.handleCollectionChange = function (path, parameter) {
            var current = collectionCache[path];
            var rootElement = current.rootElement;
            var template = current.template;

            // need to implement all actions
            switch (parameter.Action) {
                case 0: // add
                    renderCollectionItemFromTemplate(path, template, rootElement, parameter.NewStartingIndex);
                    break;
                case 4:
                    var otherAffected = Object.keys(elementCache).filter(function (k) {
                        return k.indexOf(current + "[") === 0;
                    }).reduce(function (newData, k) {
                        newData[k] = elementCache[k];
                        return newData;
                    },
                        {});
                    for (var item in otherAffected) {
                        if (otherAffected.hasOwnProperty(item)) {
                            var affected = otherAffected[item];
                            for (var i = 0; i < affected.length; i++) {
                                var affectedNodeBinding = affected[i];
                                if (affectedNodeBinding.insideTemplate) {
                                    // this can be done better
                                    delete elementCache[item];
                                }
                            }
                        }
                    }
                    break;
            }
        };

        openmvvm.navigateTo = function (viewName) {
            if (!viewCache[viewName]) {
                openmvvm.loadView(viewName, function (element) {
                    viewCache[viewName] = {
                        element: element,
                        initialized: false
                    }
                    openmvvm.navigateTo(viewName);
                });
                return;
            }

            var startViewElement = viewCache[viewName].element;

            if (!viewCache[viewName].initialized) {
                var root = startViewElement;
                var currentDataContext = root.getAttribute('data-context');
                analyzeNode(root, currentDataContext);

                viewCache[viewName].initialized = true;
            }

            while (frame.firstChild) {
                frame.removeChild(frame.firstChild);
            }

            frame.appendChild(startViewElement);
          
            currentView = viewName;
        }

        openmvvm.jsBind = function () {

            var bind = function () {

                frame = document.querySelector('[data-mvvmstart]');
                
                var viewElements = document.querySelectorAll('[data-view]');

                for (var i = 0; i < viewElements.length; i++) {
                    var currentElement = viewElements[i];
                    var viewName = currentElement.getAttribute('data-view');

                    viewCache[viewName] = {
                        element: currentElement,
                        initialized: false
                    }
                    currentElement.parentElement.removeChild(currentElement);
                }

                // all view templates are removed, can initialize
                bridgeService.appReady();
            }();

            return {
                bind: bind
            }
        };

        return openmvvm;
    }(OpenMVVM || {})
);

OpenMVVM.jsBind();